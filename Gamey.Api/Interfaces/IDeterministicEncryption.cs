namespace Gamey.Api.Interfaces
{
    public interface IDeterministicEncryption
    {
        Task<string> Encrypt(string plaintext);
        Task<string> Decrypt(string base64);
    }
}