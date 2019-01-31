namespace MoneyFox.ServiceLayer.Interfaces
{
    /// <summary>
    ///     Wrapper object for IMvxProtectedData to provide a nicer access.
    /// </summary>
    public interface IPasswordStorage
    {
        /// <summary>
        ///     Saves a password to the secure storage of the current platform
        /// </summary>
        /// <param name="password">Password to save.</param>
        void SavePassword(string password);

        /// <summary>
        ///     Loads the password from the secure storage.
        /// </summary>
        /// <returns>Loaded password.</returns>
        string LoadPassword();

        /// <summary>
        ///     Removes the password from the secure storage.
        /// </summary>
        void RemovePassword();

        /// <summary>
        ///     Validates a given string against the saved password.
        /// </summary>
        /// <param name="passwordToValidate">String to verify.</param>
        /// <returns>Boolean if password matched.</returns>
        bool ValidatePassword(string passwordToValidate);
    }
}