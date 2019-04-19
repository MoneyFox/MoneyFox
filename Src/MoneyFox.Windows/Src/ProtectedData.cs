using Windows.ApplicationModel;
using Windows.Security.Credentials;
using MoneyFox.ServiceLayer.Interfaces;

namespace MoneyFox.Windows
{
    public class ProtectedData : IProtectedData
    {
        public void Protect(string key, string value)
        {
            var vault = new PasswordVault();
            vault.Add(new PasswordCredential(
                Package.Current.Id.Name, key, value));
        }

        public string Unprotect(string key)
        {
            try
            {
                var vault = new PasswordVault();
                return vault.Retrieve(Package.Current.Id.Name, key).Password;
            }
            catch
            {
                return null;
            }
        }

        public void Remove(string key)
        {
            var vault = new PasswordVault();
            var passwordCredential = vault.Retrieve(Package.Current.Id.Name, key);
            vault.Remove(passwordCredential);
        }
    }
}