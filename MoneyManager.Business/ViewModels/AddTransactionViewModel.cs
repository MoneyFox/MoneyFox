#region

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using PropertyChanged;

#endregion

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class AddTransactionViewModel
    {
        public AddTransactionViewModel()
        {
            IsNavigationBlocked = true;
        }

        #region Properties

        public FinancialTransaction SelectedTransaction
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction; }
            set { ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction = value; }
        }

        public ObservableCollection<Account> AllAccounts
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().AllAccounts; }
        }

        public ObservableCollection<Category> AllCategories
        {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>().AllCategories; }
        }

        public SettingDataAccess Settings
        {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); }
        }

        public DateTime EndDate { get; set; }

        public bool IsEndless { get; set; }

        public bool IsEdit { get; set; }

        public int Recurrence { get; set; }

        public bool IsTransfer { get; set; }

        public bool RefreshRealtedList { get; set; }

        #endregion Properties

        public string Title
        {
            get
            {
                var text = IsEdit
                    ? Translation.GetTranslation("EditTitle")
                    : Translation.GetTranslation("AddTitle");

                var type = TransactionTypeLogic.GetViewTitleForType(SelectedTransaction.Type);

                return String.Format(text, type);
            }
        }

        public string AmountString
        {
            get { return AmountWithoutExchange.ToString(); }
            set
            {
                double amount;
                if (Double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentUICulture , out amount))
                {
                    AmountWithoutExchange = amount;
                }
            }
        }

        public double AmountWithoutExchange
        {
            get { return SelectedTransaction.AmountWithoutExchange; }
            set
            {
                SelectedTransaction.AmountWithoutExchange = value;
                CalculateNewAmount(value);
            }
        }

        public bool IsNavigationBlocked { get; set; }

        private void CalculateNewAmount(double value)
        {
            if (Math.Abs(SelectedTransaction.ExchangeRatio) < 0.5)
            {
                SelectedTransaction.ExchangeRatio = 1;
            }

            SelectedTransaction.Amount = SelectedTransaction.ExchangeRatio*value;
        }

        public async void SetCurrency(string currency)
        {
            SelectedTransaction.Currency = currency;
            await LoadCurrencyRatio();
            SelectedTransaction.IsExchangeModeActive = true;
            CalculateNewAmount(AmountWithoutExchange);
        }

        public async Task LoadCurrencyRatio()
        {
            SelectedTransaction.ExchangeRatio =
                await CurrencyLogic.GetCurrencyRatio(Settings.DefaultCurrency, SelectedTransaction.Currency);
        }

        public async void Save()
        {
            if (IsEdit)
            {
                await TransactionLogic.UpdateTransaction(SelectedTransaction);
            }
            else
            {
                await TransactionLogic.SaveTransaction(SelectedTransaction, RefreshRealtedList);
            }

            ((Frame) Window.Current.Content).GoBack();
        }

        public async void Cancel()
        {
            if (IsEdit)
            {
                await AccountLogic.AddTransactionAmount(SelectedTransaction);
            }

            ((Frame) Window.Current.Content).GoBack();
        }
    }
}