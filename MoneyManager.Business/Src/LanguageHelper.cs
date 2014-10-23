using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Windows.Globalization;
using Windows.System.UserProfile;
using Windows.UI.Popups;

namespace MoneyManager.Src
{
    internal class LanguageHelper
    {
        private static TransactionDataAccess transactionData
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        private static AccountDataAccess accountData
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        public static List<String> GetSupportedLanguages()
        {
            return GlobalizationPreferences.Languages.ToList();
        }

        public async static void SetPrimaryLanguage(string lang)
        {
            ApplicationLanguages.PrimaryLanguageOverride = lang;

            var dialog = new MessageDialog(Utilities.GetTranslation("ChangeAllEntitiesMessage"),
                Utilities.GetTranslation("ChangeAllEntitiesTitle"));
            dialog.Commands.Add(new UICommand(Utilities.GetTranslation("YesLabel")));
            dialog.Commands.Add(new UICommand(Utilities.GetTranslation("NoLabel")));
            dialog.DefaultCommandIndex = 1;

            var result = await dialog.ShowAsync();

            if (result.Label == Utilities.GetTranslation("YesLabel"))
            {
                ChangeTransactions();
                ChangeAccounts();
            }
        }

        private static void ChangeTransactions()
        {
            foreach (var transaction in transactionData.AllTransactions)
            {
                transaction.CurrencyCulture = CultureInfo.CurrentCulture.Name;
                transactionData.Update(transaction);
            }
        }

        private static void ChangeAccounts()
        {
            foreach (var account in accountData.AllAccounts)
            {
                account.CurrencyCulture = CultureInfo.CurrentCulture.Name;
                accountData.Update(account);
            }
        }

        public static string GetPrimaryLanguage()
        {
            return ApplicationLanguages.Languages.First();
        }
    }
}