namespace MoneyFox.Desktop.Infrastructure;

using System;

public static partial class Preferences
{
    // overloads

    public static bool ContainsKey(string key)
    {
        return ContainsKey(key: key, sharedName: null);
    }

    public static void Remove(string key)
    {
        Remove(key: key, sharedName: null);
    }

    public static void Clear()
    {
        Clear(null);
    }

    public static string Get(string key, string defaultValue)
    {
        return Get(key: key, defaultValue: defaultValue, sharedName: null);
    }

    public static bool Get(string key, bool defaultValue)
    {
        return Get(key: key, defaultValue: defaultValue, sharedName: null);
    }

    public static int Get(string key, int defaultValue)
    {
        return Get(key: key, defaultValue: defaultValue, sharedName: null);
    }

    public static double Get(string key, double defaultValue)
    {
        return Get(key: key, defaultValue: defaultValue, sharedName: null);
    }

    public static float Get(string key, float defaultValue)
    {
        return Get(key: key, defaultValue: defaultValue, sharedName: null);
    }

    public static long Get(string key, long defaultValue)
    {
        return Get(key: key, defaultValue: defaultValue, sharedName: null);
    }

    public static void Set(string key, string value)
    {
        Set(key: key, value: value, sharedName: null);
    }

    public static void Set(string key, bool value)
    {
        Set(key: key, value: value, sharedName: null);
    }

    public static void Set(string key, int value)
    {
        Set(key: key, value: value, sharedName: null);
    }

    public static void Set(string key, double value)
    {
        Set(key: key, value: value, sharedName: null);
    }

    public static void Set(string key, float value)
    {
        Set(key: key, value: value, sharedName: null);
    }

    public static void Set(string key, long value)
    {
        Set(key: key, value: value, sharedName: null);
    }

    // shared -> platform

    public static bool ContainsKey(string key, string sharedName)
    {
        return PlatformContainsKey(key: key, sharedName: sharedName);
    }

    public static void Remove(string key, string sharedName)
    {
        PlatformRemove(key: key, sharedName: sharedName);
    }

    public static void Clear(string sharedName)
    {
        PlatformClear(sharedName);
    }

    public static string Get(string key, string defaultValue, string sharedName)
    {
        return PlatformGet(key: key, defaultValue: defaultValue, sharedName: sharedName);
    }

    public static bool Get(string key, bool defaultValue, string sharedName)
    {
        return PlatformGet(key: key, defaultValue: defaultValue, sharedName: sharedName);
    }

    public static int Get(string key, int defaultValue, string sharedName)
    {
        return PlatformGet(key: key, defaultValue: defaultValue, sharedName: sharedName);
    }

    public static double Get(string key, double defaultValue, string sharedName)
    {
        return PlatformGet(key: key, defaultValue: defaultValue, sharedName: sharedName);
    }

    public static float Get(string key, float defaultValue, string sharedName)
    {
        return PlatformGet(key: key, defaultValue: defaultValue, sharedName: sharedName);
    }

    public static long Get(string key, long defaultValue, string sharedName)
    {
        return PlatformGet(key: key, defaultValue: defaultValue, sharedName: sharedName);
    }

    public static void Set(string key, string value, string sharedName)
    {
        PlatformSet(key: key, value: value, sharedName: sharedName);
    }

    public static void Set(string key, bool value, string sharedName)
    {
        PlatformSet(key: key, value: value, sharedName: sharedName);
    }

    public static void Set(string key, int value, string sharedName)
    {
        PlatformSet(key: key, value: value, sharedName: sharedName);
    }

    public static void Set(string key, double value, string sharedName)
    {
        PlatformSet(key: key, value: value, sharedName: sharedName);
    }

    public static void Set(string key, float value, string sharedName)
    {
        PlatformSet(key: key, value: value, sharedName: sharedName);
    }

    public static void Set(string key, long value, string sharedName)
    {
        PlatformSet(key: key, value: value, sharedName: sharedName);
    }

    // DateTime

    public static DateTime Get(string key, DateTime defaultValue)
    {
        return Get(key: key, defaultValue: defaultValue, sharedName: null);
    }

    public static void Set(string key, DateTime value)
    {
        Set(key: key, value: value, sharedName: null);
    }

    public static DateTime Get(string key, DateTime defaultValue, string sharedName)
    {
        return DateTime.FromBinary(PlatformGet(key: key, defaultValue: defaultValue.ToBinary(), sharedName: sharedName));
    }

    public static void Set(string key, DateTime value, string sharedName)
    {
        PlatformSet(key: key, value: value.ToBinary(), sharedName: sharedName);
    }
}
