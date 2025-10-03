using Gamey.Api.Configuration;
using Gamey.Api.Interfaces;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Gamey.Api.Logic
{
    public class SecureEncryption : ISecureEncryption
    {
        private readonly TokenEncryptionOptions _tokenEncryption;

        public SecureEncryption(IOptions<TokenEncryptionOptions> tokenEncryptionOptions)
        {
            _tokenEncryption = tokenEncryptionOptions.Value;
        }

        /// <summary>
        /// Encrypts plaintext using AES-256-GCM with a random salt.
        /// </summary>
        public async Task<string> Encrypt(string plainText)
        {
            // Generate random salt
            byte[] salt = RandomNumberGenerator.GetBytes(_tokenEncryption.SaltSize);

            // Derive key from password + salt
            byte[] key = new Rfc2898DeriveBytes(_tokenEncryption.Password, salt, 100_000, HashAlgorithmName.SHA256).GetBytes(_tokenEncryption.KeySize);

            using var aes = new AesGcm(key);
            byte[] nonce = RandomNumberGenerator.GetBytes(_tokenEncryption.NonceSize);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = new byte[plainBytes.Length];
            byte[] tag = new byte[_tokenEncryption.TagSize];

            aes.Encrypt(nonce, plainBytes, cipherBytes, tag);

            // Combine salt + nonce + tag + ciphertext
            byte[] combined = new byte[_tokenEncryption.SaltSize + _tokenEncryption.NonceSize + _tokenEncryption.TagSize + cipherBytes.Length];
            Buffer.BlockCopy(salt, 0, combined, 0, _tokenEncryption.SaltSize);
            Buffer.BlockCopy(nonce, 0, combined, _tokenEncryption.SaltSize, _tokenEncryption.NonceSize);
            Buffer.BlockCopy(tag, 0, combined, _tokenEncryption.SaltSize + _tokenEncryption.NonceSize, _tokenEncryption.TagSize);
            Buffer.BlockCopy(cipherBytes, 0, combined, _tokenEncryption.SaltSize + _tokenEncryption.NonceSize + _tokenEncryption.TagSize, cipherBytes.Length);

            return Convert.ToBase64String(combined);
        }

        /// <summary>
        /// Decrypts ciphertext using AES-256-GCM with salt.
        /// </summary>
        public async Task<string> Decrypt(string cipherText)
        {
            byte[] combined = Convert.FromBase64String(cipherText);

            // Extract salt, nonce, tag, and ciphertext
            byte[] salt = new byte[_tokenEncryption.SaltSize];
            byte[] nonce = new byte[_tokenEncryption.NonceSize];
            byte[] tag = new byte[_tokenEncryption.TagSize];
            byte[] cipherBytes = new byte[combined.Length - _tokenEncryption.SaltSize - _tokenEncryption.NonceSize - _tokenEncryption.TagSize];

            Buffer.BlockCopy(combined, 0, salt, 0, _tokenEncryption.SaltSize);
            Buffer.BlockCopy(combined, _tokenEncryption.SaltSize, nonce, 0, _tokenEncryption.NonceSize);
            Buffer.BlockCopy(combined, _tokenEncryption.SaltSize + _tokenEncryption.NonceSize, tag, 0, _tokenEncryption.TagSize);
            Buffer.BlockCopy(combined, _tokenEncryption.SaltSize + _tokenEncryption.NonceSize + _tokenEncryption.TagSize, cipherBytes, 0, cipherBytes.Length);

            // Derive key using same salt
            byte[] key = new Rfc2898DeriveBytes(_tokenEncryption.Password, salt, 100_000, HashAlgorithmName.SHA256).GetBytes(_tokenEncryption.KeySize);

            using var aes = new AesGcm(key);
            byte[] plainBytes = new byte[cipherBytes.Length];

            aes.Decrypt(nonce, cipherBytes, tag, plainBytes);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
