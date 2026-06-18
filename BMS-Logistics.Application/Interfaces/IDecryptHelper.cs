namespace BMS_Logistics.Application.Interfaces
{
    public interface IDecryptHelper
    {
        string Decrypt(string encrypted, bool? backend = false);
        string DecryptBackend(string cipherText);
        string Encrypt(string text);
    }
}
