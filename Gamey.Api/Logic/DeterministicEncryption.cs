using Gamey.Api.Configuration;
using Gamey.Api.Interfaces;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

public class DeterministicEncryption : IDeterministicEncryption
{
    private readonly byte[] masterKey;
    private readonly byte[]? aad = null;

    public DeterministicEncryption(IOptions<EncryptionOptions> encryptionOptions)
    {
        masterKey = Encoding.UTF8.GetBytes(encryptionOptions.Value.MasterKey);
    }

    // Provide a 32-byte master key from secure storage (NOT a low-entropy password).
    // If you must start from a password, run PBKDF2 once at startup with a fixed app salt.
    public async Task<string> Encrypt(string plaintext)
    {
        if (masterKey is null || (masterKey.Length != 16 && masterKey.Length != 32))
            throw new ArgumentException("masterKey must be 16 or 32 bytes.", nameof(masterKey));

        // Derive independent keys for encryption and nonce derivation
        var encKey = HkdfExpandSha256(masterKey, "det-aesgcm:enc", 32);
        var nonceKey = HkdfExpandSha256(masterKey, "det-aesgcm:nonce", 32);

        byte[] plain = Encoding.UTF8.GetBytes(plaintext);
        byte[] nonce = DeriveNonce(nonceKey, plain, aad); // 12 bytes

        using var aes = new AesGcm(encKey);
        byte[] cipher = new byte[plain.Length];
        byte[] tag = new byte[16];

        if (aad is null)
            aes.Encrypt(nonce, plain, cipher, tag);
        else
            aes.Encrypt(nonce, plain, cipher, tag, aad);

        // [nonce | tag | ciphertext]
        byte[] output = new byte[12 + 16 + cipher.Length];
        Buffer.BlockCopy(nonce, 0, output, 0, 12);
        Buffer.BlockCopy(tag, 0, output, 12, 16);
        Buffer.BlockCopy(cipher, 0, output, 28, cipher.Length);

        CryptographicOperations.ZeroMemory(encKey);
        CryptographicOperations.ZeroMemory(nonceKey);
        return Convert.ToBase64String(output);
    }

    public async Task<string> Decrypt(string base64)
    {
        if (masterKey is null || (masterKey.Length != 16 && masterKey.Length != 32))
            throw new ArgumentException("masterKey must be 16 or 32 bytes.", nameof(masterKey));

        byte[] data = Convert.FromBase64String(base64);
        if (data.Length < 12 + 16) throw new CryptographicException("Ciphertext too short.");

        byte[] nonce = new byte[12];
        byte[] tag = new byte[16];
        byte[] cipher = new byte[data.Length - 28];

        Buffer.BlockCopy(data, 0, nonce, 0, 12);
        Buffer.BlockCopy(data, 12, tag, 0, 16);
        Buffer.BlockCopy(data, 28, cipher, 0, cipher.Length);

        var encKey = HkdfExpandSha256(masterKey, "det-aesgcm:enc", 32);

        using var aes = new AesGcm(encKey);
        byte[] plain = new byte[cipher.Length];

        if (aad is null)
            aes.Decrypt(nonce, cipher, tag, plain);
        else
            aes.Decrypt(nonce, cipher, tag, plain, aad);

        CryptographicOperations.ZeroMemory(encKey);
        return Encoding.UTF8.GetString(plain);
    }

    // ---- Helpers ----

    private static byte[] DeriveNonce(byte[] nonceKey, byte[] plaintext, byte[]? aad)
    {
        // nonce = Truncate_12( HMAC-SHA256(nonceKey, aad || 0x00 || plaintext) )
        using var h = new HMACSHA256(nonceKey);
        if (aad is not null) h.TransformBlock(aad, 0, aad.Length, null, 0);
        h.TransformBlock(new byte[] { 0x00 }, 0, 1, null, 0);
        h.TransformFinalBlock(plaintext, 0, plaintext.Length);
        byte[] full = h.Hash!;
        byte[] nonce = new byte[12];
        Buffer.BlockCopy(full, 0, nonce, 0, 12);
        CryptographicOperations.ZeroMemory(full);
        return nonce;
    }

    private static byte[] HkdfExpandSha256(byte[] ikm, string info, int length)
    {
        // Simple HKDF with empty salt = HMAC-SHA256(zeros)
        byte[] prk;
        using (var hmac = new HMACSHA256(new byte[32])) // salt = zeros
        {
            prk = hmac.ComputeHash(ikm);
        }

        using var hmac2 = new HMACSHA256(prk);
        byte[] t = Array.Empty<byte>();
        int n = (int)Math.Ceiling(length / 32.0);
        byte[] okm = new byte[length];
        int offset = 0;

        for (int i = 1; i <= n; i++)
        {
            hmac2.Initialize();
            if (t.Length > 0) hmac2.TransformBlock(t, 0, t.Length, null, 0);
            var infoBytes = Encoding.UTF8.GetBytes(info);
            if (infoBytes.Length > 0) hmac2.TransformBlock(infoBytes, 0, infoBytes.Length, null, 0);
            hmac2.TransformFinalBlock(new byte[] { (byte)i }, 0, 1);
            t = hmac2.Hash!;
            int toCopy = Math.Min(32, length - offset);
            Buffer.BlockCopy(t, 0, okm, offset, toCopy);
            offset += toCopy;
        }

        CryptographicOperations.ZeroMemory(prk);
        CryptographicOperations.ZeroMemory(t);
        return okm;
    }
}
