namespace Gamey.Api.Interfaces
{
    public interface ISecureEncryption
    {
        Task<string> Encrypt(string plainText);
        Task<string> Decrypt(string cipherText);
    }
}