namespace MoneyFox.Core.Interfaces;

public interface ISettingsAdapter
{
    /// <summary>
    ///     Selects bool value of the settings with the passed key.
    /// </summary>
    /// <param name="key">Settings Key.</param>
    /// <param name="defaultValue">Default value in case nothing is found.</param>
    /// <returns>value</returns>
    bool GetValue(string key, bool defaultValue);

    /// <summary>
    ///     Selects string value of the settings with the passed key.
    /// </summary>
    /// <param name="key">Settings Key.</param>
    /// <param name="defaultValue">Default value in case nothing is found.</param>
    /// <returns>value</returns>
    string GetValue(string key, string defaultValue);

    /// <summary>
    ///     Selects int value of the settings with the passed key.
    /// </summary>
    /// <param name="key">Settings Key.</param>
    /// <param name="defaultValue">Default value in case nothing is found.</param>
    /// <returns>value</returns>
    int GetValue(string key, int defaultValue);

    /// <summary>
    ///     Adds a setting with a bool value or updates it if the key already exists.
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    void AddOrUpdate(string key, bool value);

    /// <summary>
    ///     Adds a setting with a string value or updates it if the key already exists.
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    void AddOrUpdate(string key, string value);

    /// <summary>
    ///     Adds a setting with a int value or updates it if the key already exists.
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    void AddOrUpdate(string key, int value);

    /// <summary>
    ///     Removes the setting with the passed key.
    /// </summary>
    /// <param name="key">Settings key.</param>
    void Remove(string key);
}
