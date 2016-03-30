namespace MoneyFox.Core.Interfaces
{
    public interface IProtectedData
    {
        void Protect(string key, string value);
        string Unprotect(string key);
        void Remove(string key);
    }
}