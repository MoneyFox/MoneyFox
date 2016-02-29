using System;
using Windows.ApplicationModel;
using Windows.Security.Credentials;
using MoneyManager.Foundation.Interfaces;

namespace MoneyFox.Core.Authentication
{
    /// <summary>
    ///     Wrapper object for IMvxProtectedData to provide a nicer access.
    /// </summary>
    public class PasswordStorage : IPasswordStorage
    {
        private const string PASSWORD_KEY = "password";

        /// <summary>
        ///     Saves a password to the secure storage of the current platform
        /// </summary>
        /// <param name="password">Password to save.</param>
        public void SavePassword(string password)
        {
            var vault = new PasswordVault();
            vault.Add(new PasswordCredential(Package.Current.Id.Name, PASSWORD_KEY, password));
        }

        /// <summary>
        ///     Loads the password from the secure storage.
        /// </summary>
        /// <returns>Loaded password.</returns>
        public string LoadPassword()
        {
            try
            {
                return new PasswordVault().Retrieve(Package.Current.Id.Name, PASSWORD_KEY).Password;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Removes the password from the secure storage.
        /// </summary>
        public void RemovePassword()
        {
            // If there where no element to remove it will throw a com exception who we handle.
            try
            {
                var vault = new PasswordVault();
                var passwordCredential = vault.Retrieve(Package.Current.Id.Name, PASSWORD_KEY);
                vault.Remove(passwordCredential);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        ///     Validates a given string against the saved password.
        /// </summary>
        /// <param name="passwordToValidate">String to verify.</param>
        /// <returns>Boolean if password matched.</returns>
        public bool ValidatePassword(string passwordToValidate)
        {
            return LoadPassword() == passwordToValidate;
        }
    }
}