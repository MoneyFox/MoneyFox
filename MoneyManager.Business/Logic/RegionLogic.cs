#region

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Globalization;
using Windows.System.UserProfile;
using Windows.UI.Popups;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;

#endregion

namespace MoneyManager.Business.Logic {
    public class RegionLogic {
        private static TransactionDataAccess transactionData {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        private static AccountDataAccess accountData {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        private static SettingDataAccess settings {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); }
        }

        public static List<String> GetSupportedLanguages() {
            return GlobalizationPreferences.Languages.ToList();
        }

        public static void SetPrimaryLanguage(string lang) {
            ApplicationLanguages.PrimaryLanguageOverride = lang;
        }

        public static async void SetNewCurrency(string currencyId) {
            settings.DefaultCurrency = currencyId;

            var dialog = new MessageDialog(Translation.GetTranslation("ChangeAllEntitiesMessage"),
                Translation.GetTranslation("ChangeAllEntitiesTitle"));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("YesLabel")));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("NoLabel")));
            dialog.DefaultCommandIndex = 1;

            IUICommand result = await dialog.ShowAsync();

            if (result.Label == Translation.GetTranslation("YesLabel")) {
                ChangeTransactions();
                ChangeAccounts();
            }
        }

        private static void ChangeTransactions() {
            foreach (FinancialTransaction transaction in transactionData.AllTransactions) {
                transaction.Currency = settings.DefaultCurrency;
                transactionData.Update(transaction);
            }
        }

        private static void ChangeAccounts() {
            foreach (Account account in accountData.AllAccounts) {
                account.Currency = settings.DefaultCurrency;
                accountData.Update(account);
            }
        }

        public static string GetPrimaryLanguage() {
            return ApplicationLanguages.Languages.First();
        }
    }
}