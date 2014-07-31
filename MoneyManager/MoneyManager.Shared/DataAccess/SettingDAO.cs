using MoneyTracker.Models;
using MoneyTracker.Src;
using System;
using System.Globalization;
using System.Linq;

namespace MoneyTracker.ViewModels
{
    public class SettingDAO
    {
        private const string DbVersionKeyname = "DbVersion";
        private const string CurrencyKeyname = "Currency";

        private const int DbVersionKeydefault = 1;
        private const string CurrencyKeydefault = "CHF";

        #region Properties

        public int Dbversion
        {
            get
            {
                return GetValueOrDefault(DbVersionKeyname, DbVersionKeydefault);
            }
            set
            {
                AddOrUpdateValue(DbVersionKeyname, value);
            }
        }

        public string Currency
        {
            get
            {
                return GetValueOrDefault(CurrencyKeyname, CurrencyKeydefault);
            }
            set
            {
                AddOrUpdateValue(CurrencyKeyname, value);
            }
        }

        #endregion Properties

        private void AddOrUpdateValue(string key, Object value)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                if (dbConn.Table<Setting>().All(item => item.Key != key))
                {
                    var t = new Setting { Key = key, Value = value.ToString() };
                    dbConn.Insert(t, typeof(Setting));
                }

                var setting = dbConn.Table<Setting>().Single(item => item.Key == key);
                setting.Value = value.ToString();
                dbConn.Update(setting, typeof(Setting));
            }
        }

        private valueType GetValueOrDefault<valueType>(string key, valueType defaultValue)
        {
            valueType value;

            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                if (dbConn.Table<Setting>().Any(item => item.Key == key))
                {
                    var selectedItem = dbConn.Table<Setting>().Single(item => item.Key == key);
                    value = (valueType)Convert.ChangeType(selectedItem.Value, typeof(valueType), CultureInfo.InvariantCulture);
                }
                else
                {
                    value = defaultValue;
                }
            }

            return value;
        }
    }
}