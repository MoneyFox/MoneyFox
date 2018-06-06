using MoneyFox.Foundation.Interfaces;
using Plugin.SecureStorage;

namespace MoneyFox.iOS
{
    public class ProtectedData : IProtectedData
    {
        public void Protect(string key, string value)
        {
            CrossSecureStorage.Current.SetValue(key, value);
        }

        public string Unprotect(string key)
        {
            return CrossSecureStorage.Current.GetValue(key);
        }

        public void Remove(string key)
        {
            CrossSecureStorage.Current.DeleteKey(key);
        }
    }
}