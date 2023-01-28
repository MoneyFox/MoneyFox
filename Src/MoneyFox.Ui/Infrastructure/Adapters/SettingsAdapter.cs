namespace MoneyFox.Ui.Infrastructure.Adapters;

using Core.Interfaces;
using Serilog;

public class SettingsAdapter : ISettingsAdapter
{
    public bool GetValue(string key, bool defaultValue)
    {
        try
        {
            return Preferences.Get(key: key, defaultValue: defaultValue);
        }
        catch (InvalidCastException)
        {
            Log.Error(messageTemplate: "Value {key} couldn't be parsed to bool", propertyValue: key);
            Preferences.Set(key: key, value: defaultValue);

            return defaultValue;
        }
    }

    public string GetValue(string key, string defaultValue)
    {
        try
        {
            return Preferences.Get(key: key, defaultValue: defaultValue);
        }
        catch (InvalidCastException)
        {
            Log.Error(messageTemplate: "Value {key} couldn't be parsed to string", propertyValue: key);
            Preferences.Set(key: key, value: defaultValue);

            return defaultValue;
        }
    }

    public int GetValue(string key, int defaultValue)
    {
        try
        {
            return Preferences.Get(key: key, defaultValue: defaultValue);
        }
        catch (InvalidCastException)
        {
            Log.Error(messageTemplate: "Value {Key} couldn't be parsed to int", propertyValue: key);
            Preferences.Set(key: key, value: defaultValue);

            return defaultValue;
        }
    }

    public void AddOrUpdate(string key, bool value)
    {
        Preferences.Set(key: key, value: value);
    }

    public void AddOrUpdate(string key, string value)
    {
        Preferences.Set(key: key, value: value);
    }

    public void AddOrUpdate(string key, int value)
    {
        Preferences.Set(key: key, value: value);
    }

    public void Remove(string key)
    {
        Preferences.Remove(key);
    }
}
