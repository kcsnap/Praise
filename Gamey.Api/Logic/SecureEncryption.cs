using Gamey.Api.Configuration;
using Gamey.Api.Interfaces;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Gamey.Api.Logic
{
    public class SecureEncryption : ISecureEncryption
    {
        private readonly EncryptionOptions _encryptionOptions;

        public SecureEncryption(IOptions<EncryptionOptions> encryptionOptions)
        {
            _encryptionOptions = encryptionOptions.Value;
        }

        /// <summary>
        /// Encrypts plaintext using AES-256-GCM with a random salt.
        /// </summary>
        public async Task<string> Encrypt(string plainText)
        {
            // Generate random salt
            byte[] salt = RandomNumberGenerator.GetBytes(_encryptionOptions.SaltSize);

            // Derive key from password + salt
            byte[] key = new Rfc2898DeriveBytes(_encryptionOptions.Password, salt, 100_000, HashAlgorithmName.SHA256).GetBytes(_encryptionOptions.KeySize);

            using var aes = new AesGcm(key);
            byte[] nonce = RandomNumberGenerator.GetBytes(_encryptionOptions.NonceSize);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = new byte[plainBytes.Length];
            byte[] tag = new byte[_encryptionOptions.TagSize];

            aes.Encrypt(nonce, plainBytes, cipherBytes, tag);

            // Combine salt + nonce + tag + ciphertext
            byte[] combined = new byte[_encryptionOptions.SaltSize + _encryptionOptions.NonceSize + _encryptionOptions.TagSize + cipherBytes.Length];
            Buffer.BlockCopy(salt, 0, combined, 0, _encryptionOptions.SaltSize);
            Buffer.BlockCopy(nonce, 0, combined, _encryptionOptions.SaltSize, _encryptionOptions.NonceSize);
            Buffer.BlockCopy(tag, 0, combined, _encryptionOptions.SaltSize + _encryptionOptions.NonceSize, _encryptionOptions.TagSize);
            Buffer.BlockCopy(cipherBytes, 0, combined, _encryptionOptions.SaltSize + _encryptionOptions.NonceSize + _encryptionOptions.TagSize, cipherBytes.Length);

            return Convert.ToBase64String(combined);
        }

        /// <summary>
        /// Decrypts ciphertext using AES-256-GCM with salt.
        /// </summary>
        public async Task<string> Decrypt(string cipherText)
        {
            byte[] combined = Convert.FromBase64String(cipherText);

            // Extract salt, nonce, tag, and ciphertext
            byte[] salt = new byte[_encryptionOptions.SaltSize];
            byte[] nonce = new byte[_encryptionOptions.NonceSize];
            byte[] tag = new byte[_encryptionOptions.TagSize];
            byte[] cipherBytes = new byte[combined.Length - _encryptionOptions.SaltSize - _encryptionOptions.NonceSize - _encryptionOptions.TagSize];

            Buffer.BlockCopy(combined, 0, salt, 0, _encryptionOptions.SaltSize);
            Buffer.BlockCopy(combined, _encryptionOptions.SaltSize, nonce, 0, _encryptionOptions.NonceSize);
            Buffer.BlockCopy(combined, _encryptionOptions.SaltSize + _encryptionOptions.NonceSize, tag, 0, _encryptionOptions.TagSize);
            Buffer.BlockCopy(combined, _encryptionOptions.SaltSize + _encryptionOptions.NonceSize + _encryptionOptions.TagSize, cipherBytes, 0, cipherBytes.Length);

            // Derive key using same salt
            byte[] key = new Rfc2898DeriveBytes(_encryptionOptions.Password, salt, 100_000, HashAlgorithmName.SHA256).GetBytes(_encryptionOptions.KeySize);

            using var aes = new AesGcm(key);
            byte[] plainBytes = new byte[cipherBytes.Length];

            aes.Decrypt(nonce, cipherBytes, tag, plainBytes);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
