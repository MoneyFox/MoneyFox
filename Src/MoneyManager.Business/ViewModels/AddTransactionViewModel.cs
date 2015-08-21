using System;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using MoneyManager.Business.Logic;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class AddTransactionViewModel : ViewModelBase
    {
        private readonly IRepository<Account> accountRepository;
        private readonly SettingDataAccess settings;
        private readonly ITransactionRepository transactionRepository;

        public AddTransactionViewModel(ITransactionRepository transactionRepository,
            IRepository<Account> accountRepository,
            SettingDataAccess settings)
        {
            this.transactionRepository = transactionRepository;
            this.settings = settings;
            this.accountRepository = accountRepository;

            IsNavigationBlocked = true;
        }

        public bool IsNavigationBlocked { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsEndless { get; set; } = true;
        public bool IsEdit { get; set; } = false;
        public int Recurrence { get; set; }
        public bool IsTransfer { get; set; }
        public bool RefreshRealtedList { get; set; }

        public FinancialTransaction SelectedTransaction
        {
            get { return transactionRepository.Selected; }
            set { transactionRepository.Selected = value; }
        }

        public string DefaultCurrency => settings.DefaultCurrency;
        public ObservableCollection<Account> AllAccounts => accountRepository.Data;

        public string Title
        {
            get
            {
                var text = IsEdit
                    ? Translation.GetTranslation("EditTitle")
                    : Translation.GetTranslation("AddTitle");

                var type = TransactionTypeLogic.GetViewTitleForType(transactionRepository.Selected.Type);

                return string.Format(text, type);
            }
        }

        public DateTime Date
        {
            get
            {
                if (!IsEdit && SelectedTransaction.Date == DateTime.MinValue)
                {
                    SelectedTransaction.Date = DateTime.Now;
                }
                return SelectedTransaction.Date;
            }
            set { SelectedTransaction.Date = value; }
        }

        public async void Save()
        {
            if (transactionRepository.Selected.ChargedAccount == null)
            {
                ShowAccountRequiredMessage();
                return;
            }

            if (IsEdit)
            {
                await TransactionLogic.UpdateTransaction(transactionRepository.Selected);
            }
            else
            {
                await TransactionLogic.SaveTransaction(transactionRepository.Selected, RefreshRealtedList);
            }

            ((Frame) Window.Current.Content).GoBack();
        }

        private async void ShowAccountRequiredMessage()
        {
            var dialog = new MessageDialog
                (
                Translation.GetTranslation("AccountRequiredMessage"),
                Translation.GetTranslation("MandatoryField")
                );
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("OkLabel")));
            dialog.DefaultCommandIndex = 1;
            await dialog.ShowAsync();
        }

        public async void Cancel()
        {
            if (IsEdit)
            {
                await AccountLogic.AddTransactionAmount(transactionRepository.Selected);
            }
            ((Frame) Window.Current.Content).GoBack();
        }
    }
}