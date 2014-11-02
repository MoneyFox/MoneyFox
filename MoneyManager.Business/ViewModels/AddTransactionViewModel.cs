using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class AddTransactionViewModel
    {
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

        public bool IsExchangeModeActive { get; set; }

        public double ExchangeRate { get; set; }

        public double AmountWithoutExchange
        {
            get { return SelectedTransaction.AmountWithoutExchange; }
            set
            {
                SelectedTransaction.AmountWithoutExchange = value;
                CalculateNewAmount(value);
            }
        }

        private void CalculateNewAmount(double value)
        {
            if (ExchangeRate == 0)
            {
                ExchangeRate = 1;
            }

            if (IsExchangeModeActive)
            {
                SelectedTransaction.Amount = ExchangeRate * value;
            }
        }

        #endregion Properties

        public string Title
        {
            get
            {
                string text = IsEdit
                    ? Translation.GetTranslation("EditTitle")
                    : Translation.GetTranslation("AddTitle");

                string type = TransactionTypeLogic.GetViewTitleForType(SelectedTransaction.Type);

                return String.Format(text, type);
            }
        }

        public async Task LoadCurrencyRatio()
        {
            ExchangeRate = await CurrencyLogic.GetCurrencyRatio(Settings.DefaultCurrency, SelectedTransaction.Currency);
        }

        public void Save()
        {
            if (IsEdit)
            {
                TransactionLogic.UpdateTransaction(SelectedTransaction);
            }
            else
            {
                TransactionLogic.SaveTransaction(SelectedTransaction, RefreshRealtedList);
            }

            AccountLogic.AddTransactionAmount(SelectedTransaction);
            ((Frame) Window.Current.Content).GoBack();
        }

        public void Cancel()
        {
            if (IsEdit)
            {
                AccountLogic.AddTransactionAmount(SelectedTransaction);
            }

            ((Frame) Window.Current.Content).GoBack();
        }

        public async void SetCurrency(string currency)
        {
            SelectedTransaction.Currency = currency;
            await LoadCurrencyRatio();
            IsExchangeModeActive = true;
            CalculateNewAmount(AmountWithoutExchange);
        }
    }
}