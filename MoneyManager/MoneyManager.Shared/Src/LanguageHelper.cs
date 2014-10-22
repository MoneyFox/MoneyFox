using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Windows.Globalization;
using Windows.System.UserProfile;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;

namespace MoneyManager.Src
{
    public class LanguageHelper
    {
        private static IEnumerable<FinancialTransaction> allTransactions
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>().AllTransactions; }
        } 

        private static IEnumerable<Account> allAccounts
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().AllAccounts; }
        } 

        public static List<String> GetSupportedLanguages()
        {
            return GlobalizationPreferences.Languages.ToList();
        }

        public static void SetPrimaryLanguage(string lang)
        {
            ApplicationLanguages.PrimaryLanguageOverride = lang;

            ChangeTransactions();
            ChangeAccounts();
        }

        private static void ChangeTransactions()
        {
            foreach (var transaction in allTransactions)
            {
                transaction.CurrencyCulture = CultureInfo.CurrentCulture.Name;
            }
        }
        private static void ChangeAccounts()
        {
            foreach (var accounts in allAccounts)
            {
                accounts.CurrencyCulture = CultureInfo.CurrentCulture.Name;
            }
        }

        public static string GetPrimaryLanguage()
        {
            return ApplicationLanguages.Languages.First();
        }
    }
}
