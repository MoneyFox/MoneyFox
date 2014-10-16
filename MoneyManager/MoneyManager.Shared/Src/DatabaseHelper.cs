using System;
using System.Globalization;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using System.Threading.Tasks;

namespace MoneyManager.Src
{
    public class DatabaseHelper
    {
        public async static Task CreateDatabase()
        {
            var dbConn = ConnectionFactory.GetAsyncDbConnection();

            await dbConn.CreateTableAsync<Account>();
            await dbConn.CreateTableAsync<FinancialTransaction>();
            await dbConn.CreateTableAsync<RecurringTransaction>();
            await dbConn.CreateTableAsync<Group>();
            await dbConn.CreateTableAsync<Setting>();

            CheckCultureCurrency();
        }

        private static SettingDataAccess settings
        {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); }
        }

        private static void CheckCultureCurrency()
        {
            if (settings.CurrencyCulture == String.Empty)
            {
                settings.CurrencyCulture = CultureInfo.CurrentCulture.Name;
            }
        }
    }
}