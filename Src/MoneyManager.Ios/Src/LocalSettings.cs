using System;
using System.Globalization;
using Foundation;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Ios
{
    public class LocalSettings : ILocalSettings
    {
        private readonly object locker = new object();

        /// <summary>
        ///     Gets the current value or the default that you specify.
        /// </summary>
        /// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <returns>Value or default</returns>
        public T GetValueOrDefault<T>(string key, T defaultValue = default(T))
        {
            lock (locker)
            {
                var defaults = NSUserDefaults.StandardUserDefaults;

                if (defaults.ValueForKey(new NSString(key)) == null)
                    return defaultValue;

                var typeOf = typeof (T);
                if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof (Nullable<>))
                {
                    typeOf = Nullable.GetUnderlyingType(typeOf);
                }
                object value = null;
                var typeCode = Type.GetTypeCode(typeOf);
                switch (typeCode)
                {
                    case TypeCode.Decimal:
                        var savedDecimal = defaults.StringForKey(key);
                        value = Convert.ToDecimal(savedDecimal, CultureInfo.InvariantCulture);
                        break;
                    case TypeCode.Boolean:
                        value = defaults.BoolForKey(key);
                        break;
                    case TypeCode.Int64:
                        var savedInt64 = defaults.StringForKey(key);
                        value = Convert.ToInt64(savedInt64, CultureInfo.InvariantCulture);
                        break;
                    case TypeCode.Double:
                        value = defaults.DoubleForKey(key);
                        break;
                    case TypeCode.String:
                        value = defaults.StringForKey(key);
                        break;
                    case TypeCode.Int32:
#if __UNIFIED__
                        value = (int) defaults.IntForKey(key);
#else
            value = defaults.IntForKey(key);
#endif
                        break;
                    case TypeCode.Single:
#if __UNIFIED__
                        value = defaults.FloatForKey(key);
#else
            value = defaults.FloatForKey(key);
#endif
                        break;

                    case TypeCode.DateTime:
                        var savedTime = defaults.StringForKey(key);
                        var ticks = string.IsNullOrWhiteSpace(savedTime)
                            ? -1
                            : Convert.ToInt64(savedTime, CultureInfo.InvariantCulture);
                        if (ticks == -1)
                            value = defaultValue;
                        else
                            value = new DateTime(ticks);
                        break;
                    default:

                        if (defaultValue is Guid)
                        {
                            var outGuid = Guid.Empty;
                            var savedGuid = defaults.StringForKey(key);
                            if (string.IsNullOrWhiteSpace(savedGuid))
                            {
                                value = outGuid;
                            }
                            else
                            {
                                Guid.TryParse(savedGuid, out outGuid);
                                value = outGuid;
                            }
                        }
                        else
                        {
                            throw new ArgumentException($"Value of type {value.GetType().Name} is not supported.");
                        }

                        break;
                }


                return null != value ? (T) value : defaultValue;
            }
        }

        /// <summary>
        ///     Adds or updates a value
        /// </summary>
        /// <param name="key">key to update</param>
        /// <param name="value">value to set</param>
        /// <returns>True if added or update and you need to save</returns>
        public bool AddOrUpdateValue<T>(string key, T value)
        {
            var typeOf = typeof (T);
            if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof (Nullable<>))
            {
                typeOf = Nullable.GetUnderlyingType(typeOf);
            }
            var typeCode = Type.GetTypeCode(typeOf);
            return AddOrUpdateValue(key, value, typeCode);
        }

        /// <summary>
        ///     Removes a desired key from the settings
        /// </summary>
        /// <param name="key">Key for setting</param>
        public void Remove(string key)
        {
            lock (locker)
            {
                var defaults = NSUserDefaults.StandardUserDefaults;
                try
                {
                    var nsString = new NSString(key);
                    if (defaults.ValueForKey(nsString) != null)
                    {
                        defaults.RemoveObject(key);
                        defaults.Synchronize();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to remove: " + key, " Message: " + ex.Message);
                }
            }
        }

        private bool AddOrUpdateValue(string key, object value, TypeCode typeCode)
        {
            lock (locker)
            {
                var defaults = NSUserDefaults.StandardUserDefaults;
                switch (typeCode)
                {
                    case TypeCode.Decimal:
                        defaults.SetString(Convert.ToString(value, CultureInfo.InvariantCulture), key);
                        break;
                    case TypeCode.Boolean:
                        defaults.SetBool(Convert.ToBoolean(value), key);
                        break;
                    case TypeCode.Int64:
                        defaults.SetString(Convert.ToString(value, CultureInfo.InvariantCulture), key);
                        break;
                    case TypeCode.Double:
                        defaults.SetDouble(Convert.ToDouble(value, CultureInfo.InvariantCulture), key);
                        break;
                    case TypeCode.String:
                        defaults.SetString(Convert.ToString(value), key);
                        break;
                    case TypeCode.Int32:
                        defaults.SetInt(Convert.ToInt32(value, CultureInfo.InvariantCulture), key);
                        break;
                    case TypeCode.Single:
                        defaults.SetFloat(Convert.ToSingle(value, CultureInfo.InvariantCulture), key);
                        break;
                    case TypeCode.DateTime:
                        defaults.SetString(Convert.ToString(Convert.ToDateTime(value).Ticks), key);
                        break;
                    default:
                        if (value is Guid)
                        {
                            if (value == null)
                                value = Guid.Empty;

                            defaults.SetString(((Guid) value).ToString(), key);
                        }
                        else
                        {
                            throw new ArgumentException(string.Format("Value of type {0} is not supported.",
                                value.GetType().Name));
                        }
                        break;
                }
                try
                {
                    defaults.Synchronize();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to save: " + key, " Message: " + ex.Message);
                }
            }


            return true;
        }
    }
}