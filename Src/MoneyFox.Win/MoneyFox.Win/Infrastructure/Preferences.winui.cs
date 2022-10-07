﻿namespace MoneyFox.Win.Infrastructure;

using Windows.Storage;

public static partial class Preferences
{
    private static readonly object locker = new();

    private static bool PlatformContainsKey(string key, string sharedName)
    {
        lock (locker)
        {
            var appDataContainer = GetApplicationDataContainer(sharedName);

            return appDataContainer.Values.ContainsKey(key);
        }
    }

    private static void PlatformRemove(string key, string sharedName)
    {
        lock (locker)
        {
            var appDataContainer = GetApplicationDataContainer(sharedName);
            if (appDataContainer.Values.ContainsKey(key))
            {
                appDataContainer.Values.Remove(key);
            }
        }
    }

    private static void PlatformClear(string sharedName)
    {
        lock (locker)
        {
            var appDataContainer = GetApplicationDataContainer(sharedName);
            appDataContainer.Values.Clear();
        }
    }

    private static void PlatformSet<T>(string key, T value, string sharedName)
    {
        lock (locker)
        {
            var appDataContainer = GetApplicationDataContainer(sharedName);
            if (Equals(objA: value, objB: default(T)))
            {
                if (appDataContainer.Values.ContainsKey(key))
                {
                    appDataContainer.Values.Remove(key);
                }

                return;
            }

            appDataContainer.Values[key] = value;
        }
    }

    private static T PlatformGet<T>(string key, T defaultValue, string sharedName)
    {
        lock (locker)
        {
            var appDataContainer = GetApplicationDataContainer(sharedName);
            if (appDataContainer.Values.ContainsKey(key))
            {
                var tempValue = appDataContainer.Values[key];
                if (tempValue != null)
                {
                    return (T)tempValue;
                }
            }
        }

        return defaultValue;
    }

    private static ApplicationDataContainer GetApplicationDataContainer(string sharedName)
    {
        var localSettings = ApplicationData.Current.LocalSettings;
        if (string.IsNullOrWhiteSpace(sharedName))
        {
            return localSettings;
        }

        if (!localSettings.Containers.ContainsKey(sharedName))
        {
            localSettings.CreateContainer(name: sharedName, disposition: ApplicationDataCreateDisposition.Always);
        }

        return localSettings.Containers[sharedName];
    }
}
