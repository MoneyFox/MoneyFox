namespace MoneyFox.ServiceLayer.Interfaces
{
    public interface IProtectedData
    {
        /// <summary>
        ///     Saves the value with the key into the platform specific secure storage
        /// </summary>
        /// <param name="key">Key for the data Entry</param>
        /// <param name="value">value to store</param>
        void Protect(string key, string value);

        /// <summary>
        ///     Loads the value associated to the passed key from the platform specific secure storage.
        /// </summary>
        /// <param name="key">Key of the entry to load.</param>
        /// <returns>Encrypted Value.</returns>
        string Unprotect(string key);

        /// <summary>
        ///     Removes the value associated to the passed key from the platform specific secure storage.
        /// </summary>
        /// <param name="key">Key of the entry to remove.</param>
        void Remove(string key);
    }
}
