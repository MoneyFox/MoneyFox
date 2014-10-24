using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Windows.Globalization;
using Windows.System.UserProfile;
using Windows.UI.Popups;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;

namespace MoneyManager.Business.Src
{
    internal class LanguageLogic
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

        public static async void SetPrimaryLanguage(string lang)
        {
            ApplicationLanguages.PrimaryLanguageOverride = lang;

            var dialog = new MessageDialog(Translation.GetTranslation("ChangeAllEntitiesMessage"),
                Translation.GetTranslation("ChangeAllEntitiesTitle"));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("YesLabel")));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("NoLabel")));
            dialog.DefaultCommandIndex = 1;

            IUICommand result = await dialog.ShowAsync();

            if (result.Label == Translation.GetTranslation("YesLabel"))
            {
                ChangeTransactions();
                ChangeAccounts();
            }
        }

        private static void ChangeTransactions()
        {
            foreach (FinancialTransaction transaction in transactionData.AllTransactions)
            {
                transaction.CurrencyCulture = CultureInfo.CurrentCulture.Name;
                transactionData.Update(transaction);
            }
        }

        private static void ChangeAccounts()
        {
            foreach (Account account in accountData.AllAccounts)
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