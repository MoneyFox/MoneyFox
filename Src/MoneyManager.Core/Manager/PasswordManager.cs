using Beezy.MvvmCross.Plugins.SecureStorage;

namespace MoneyManager.Core.Manager
{
    public class PasswordManager
    {
        private const string PASSWORD_KEY = "password";
        private readonly IMvxProtectedData protectedData;

        public PasswordManager(IMvxProtectedData protectedData)
        {
            this.protectedData = protectedData;
        }

        /// <summary>
        ///     Saves a password to the secure storage of the current platform
        /// </summary>
        /// <param name="password">Password to save.</param>
        public void SavePassword(string password)
        {
            protectedData.Protect(PASSWORD_KEY, password);
        }

        /// <summary>
        ///     Loads the password from the secure storage.
        /// </summary>
        /// <returns>Loaded password.</returns>
        public string LoadPassword()
        {
            return protectedData.Unprotect(PASSWORD_KEY);
        }

        /// <summary>
        ///     Removes the password from the secure storage.
        /// </summary>
        public void RemovePassword()
        {
            protectedData.Remove(PASSWORD_KEY);
        }

        /// <summary>
        ///     Validates a given string against the saved password.
        /// </summary>
        /// <param name="passwordToValidate">String to verify.</param>
        /// <returns>Boolean if password matched.</returns>
        public bool ValidatePassword(string passwordToValidate)
        {
            return protectedData.Unprotect(PASSWORD_KEY) == passwordToValidate;
        }
    }
}