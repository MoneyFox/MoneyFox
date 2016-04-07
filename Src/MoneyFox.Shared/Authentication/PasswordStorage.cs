using System;
using MoneyFox.Shared.Interfaces;

namespace MoneyFox.Shared.Authentication
{
    /// <summary>
    ///     Wrapper object for IMvxProtectedData to provide a nicer access.
    /// </summary>
    public class PasswordStorage : IPasswordStorage
    {
        private const string PASSWORD_KEY = "password";
        private readonly IProtectedData protectedData;

        public PasswordStorage(IProtectedData protectedData)
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
            // If there where no element to remove it will throw a com exception who we handle.
            try
            {
                protectedData.Remove(PASSWORD_KEY);
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