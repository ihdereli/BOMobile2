namespace BOMobile2.Util
{
    public interface IStringEncyrption
    {
        string EncryptText(string text);
        string DecryptText(string cipherText);
    }
}
