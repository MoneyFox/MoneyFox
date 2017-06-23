using System;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Business.Authentication
{
    /// <inheritdoc />
    public class PasswordStorage : IPasswordStorage
    {
        private const string PASSWORD_KEY = "password";
        private readonly IProtectedData protectedData;

        /// <summary>
        ///     Constructor
        /// </summary>
        public PasswordStorage(IProtectedData protectedData)
        {
            this.protectedData = protectedData;
        }

        /// <inheritdoc />
        public void SavePassword(string password)
        {
            protectedData.Protect(PASSWORD_KEY, password);
        }

        /// <inheritdoc />
        public string LoadPassword() => protectedData.Unprotect(PASSWORD_KEY);

        /// <inheritdoc />
        public void RemovePassword()
        {
            // If there where no element to remove it will throw a com exception who we handle.
            try
            {
                protectedData.Remove(PASSWORD_KEY);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <inheritdoc />
        public bool ValidatePassword(string passwordToValidate) => LoadPassword() == passwordToValidate;
    }
}