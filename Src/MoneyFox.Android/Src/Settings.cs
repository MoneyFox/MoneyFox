using System;
using Android.Content;
using Android.Preferences;
using MoneyFox.Business;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace MoneyFox.Droid
{
    public class Settings : ISettings
    {
        private static string SETTINGS_FILE_NAME;
        private readonly object locker = new object();

        private static ISharedPreferences SharedPreferences
        {
            get
            {
                var context = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity.ApplicationContext;

                //If file name is empty use defaults
                if (string.IsNullOrEmpty(SETTINGS_FILE_NAME))
                    return PreferenceManager.GetDefaultSharedPreferences(context);

                return context.ApplicationContext.GetSharedPreferences(SETTINGS_FILE_NAME,
                    FileCreationMode.Append);
            }
        }

        public Settings(string settingsFileName = null) { SETTINGS_FILE_NAME = settingsFileName; }

        public T GetValue<T>(string key, T defaultValue = default(T), bool roaming = false)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", nameof(key));

            lock (locker)
            {
                var type = typeof(T);
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);

                using (var sharedPrefs = SharedPreferences)
                {
                    object returnVal;
                    switch (Type.GetTypeCode(type))
                    {
                        case TypeCode.Boolean:
                            returnVal = sharedPrefs.GetBoolean(key, Convert.ToBoolean(defaultValue));
                            break;
                        case TypeCode.Int64:
                            returnVal = sharedPrefs.GetLong(key, Convert.ToInt64(defaultValue));
                            break;
                        case TypeCode.Int32:
                            returnVal = sharedPrefs.GetInt(key, Convert.ToInt32(defaultValue));
                            break;
                        case TypeCode.Single:
                            returnVal = sharedPrefs.GetFloat(key, Convert.ToSingle(defaultValue));
                            break;
                        case TypeCode.String:
                            returnVal = sharedPrefs.GetString(key, Convert.ToString(defaultValue));
                            break;
                        case TypeCode.DateTime:
                            {
                                var ticks = sharedPrefs.GetLong(key, -1);
                                if (ticks == -1)
                                    returnVal = defaultValue;
                                else
                                    returnVal = new DateTime(ticks);
                                break;
                            }
                        default:
                            if (type.Name == typeof(DateTimeOffset).Name)
                            {
                                var ticks = sharedPrefs.GetString(key, "");
                                if (string.IsNullOrEmpty(ticks))
                                    returnVal = defaultValue;
                                else
                                    returnVal = DateTimeOffset.Parse(ticks);
                                break;
                            }
                            if (type.Name == typeof(Guid).Name)
                            {

                                var guid = sharedPrefs.GetString(key, "");
                                if (!string.IsNullOrEmpty(guid))
                                {
                                    Guid outGuid;
                                    Guid.TryParse(guid, out outGuid);
                                    returnVal = outGuid;
                                } else
                                    returnVal = defaultValue;
                                break;
                            }

                            throw new ArgumentException($"Type {type} is not supported",
                                nameof(defaultValue));
                    }
                    return (T)returnVal;
                }
            }
        }

        public bool AddOrUpdateValue<T>(string key, T value = default(T), bool roaming = false)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", nameof(key));

            lock (locker)
            {
                using (var sharedPrefs = SharedPreferences)
                using (var editor = sharedPrefs.Edit())
                {
                    var type = value.GetType();
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        type = Nullable.GetUnderlyingType(type);

                    switch (Type.GetTypeCode(type))
                    {
                        case TypeCode.Boolean:
                            editor.PutBoolean(key, Convert.ToBoolean(value));
                            break;
                        case TypeCode.Int64:
                            editor.PutLong(key, Convert.ToInt64(value));
                            break;
                        case TypeCode.Int32:
                            editor.PutInt(key, Convert.ToInt32(value));
                            break;
                        case TypeCode.Single:
                            editor.PutFloat(key, Convert.ToSingle(value));
                            break;
                        case TypeCode.String:
                            editor.PutString(key, Convert.ToString(value));
                            break;
                        case TypeCode.DateTime:
                            editor.PutLong(key, ((DateTime)(object)value).Ticks);
                            break;
                        default:
                            if (type.Name == typeof(DateTimeOffset).Name)
                            {
                                editor.PutString(key, ((DateTimeOffset)(object)value).ToString("o"));
                                break;
                            }
                            if (type.Name == typeof(Guid).Name)
                            {
                                var g = value as Guid?;
                                if (g.HasValue)
                                    editor.PutString(key, g.Value.ToString());
                                break;
                            }
                            throw new ArgumentException(
                                $"Type {type} is not supported", nameof(value));

                    }
                    return editor.Commit();
                }
            }
        }

        public bool DeleteValue(string key, bool roaming = false)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", nameof(key));

            lock (locker)
            {
                using (var sharedPrefs = SharedPreferences)
                using (var editor = sharedPrefs.Edit())
                {
                    editor.Remove(key);
                    return editor.Commit();
                }
            }
        }

        public bool Contains(string key, bool roaming = false)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", nameof(key));

            lock (locker)
            {
                using (var sharedPrefs = SharedPreferences)
                    return sharedPrefs.Contains(key);
            }
        }

        public bool ClearAllValues(bool roaming = false)
        {
            lock (locker)
            {
                using (var sharedPrefs = SharedPreferences)
                using (var editor = sharedPrefs.Edit())
                {
                    editor.Clear();
                    return editor.Commit();
                }
            }
        }
    }
}