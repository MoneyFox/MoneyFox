using System;
using System.Collections.ObjectModel;
using MoneyManager.Core.DataAccess;
using MoneyManager.Core.Helper;
using MoneyManager.Core.Logic;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;
using IDialogService = MoneyManager.Foundation.OperationContracts.IDialogService;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class AddTransactionViewModel : BaseViewModel
    {
        private readonly IRepository<Account> accountRepository;
        private readonly IDialogService dialogService;
        private readonly SettingDataAccess settings;
        private readonly ITransactionRepository transactionRepository;

        public AddTransactionViewModel(ITransactionRepository transactionRepository,
            IRepository<Account> accountRepository,
            SettingDataAccess settings,
            IDialogService dialogService)
        {
            this.transactionRepository = transactionRepository;
            this.settings = settings;
            this.dialogService = dialogService;
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
                    ? Strings.EditTitle
                    : Strings.AddTitle;

                var type = TransactionTypeHelper.GetViewTitleForType(transactionRepository.Selected.Type);

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
            Close(this);
        }

        private async void ShowAccountRequiredMessage()
        {
            await dialogService.ShowMessage(Strings.MandatoryFieldEmptryTitle,
                Strings.AccountRequiredMessage);
        }

        public async void Cancel()
        {
            if (IsEdit)
            {
                await AccountLogic.AddTransactionAmount(transactionRepository.Selected);
            }
            Close(this);
        }
    }
}