using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Interfaces.ViewModels;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using MoneyManager.Windows.Concrete;
using MvvmCross.Core.ViewModels;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class PaymentListViewModel : BaseViewModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly IBalanceViewModel balanceViewModel;
        private readonly IDialogService dialogService;
        private readonly IPaymentRepository paymentRepository;

        public PaymentListViewModel(IPaymentRepository paymentRepository,
            IAccountRepository accountRepository,
            IBalanceViewModel balanceViewModel,
            IDialogService dialogService)
        {
            this.paymentRepository = paymentRepository;
            this.accountRepository = accountRepository;
            this.balanceViewModel = balanceViewModel;
            this.dialogService = dialogService;
        }

        public MvxCommand<string> GoToAddTransactionCommand => new MvxCommand<string>(GoToAddTransaction);
        public MvxCommand DeleteAccountCommand => new MvxCommand(DeleteAccount);
        public virtual MvxCommand LoadedCommand => new MvxCommand(LoadTransactions);
        public MvxCommand EditCommand { get; private set; }

        public MvxCommand<Payment> DeleteTransactionCommand
            => new MvxCommand<Payment>(DeleteTransaction);

        /// <summary>
        ///     Returns all Payment who are assigned to this repository
        ///     This has to stay until the android list with headers is implemented.
        /// </summary>
        public ObservableCollection<Payment> RelatedTransactions { set; get; }

        /// <summary>
        ///     Returns groupped related transactions 
        /// </summary>
        public ObservableCollection<DateListGroup<Payment>> Source { set; get; }

        /// <summary>
        ///     Returns the name of the account title for the current page
        /// </summary>
        public string Title => accountRepository.Selected.Name;

        /// <summary>
        ///     Currently selected Item
        /// </summary>
        public Payment SelectedTransaction { get; set; }

        private void LoadTransactions()
        {
            EditCommand = null;
            //Refresh balance control with the current account
            balanceViewModel.UpdateBalance(true);

            SelectedTransaction = null;
            RelatedTransactions = new ObservableCollection<Payment>(paymentRepository
                .GetRelatedPayments(accountRepository.Selected)
                .OrderByDescending(x => x.Date)
                .ToList());

            Source = new ObservableCollection<DateListGroup<Payment>>(
                DateListGroup<Payment>.CreateGroups(RelatedTransactions,
                    CultureInfo.CurrentUICulture,
                    s => s.Date.ToString("MMMM", CultureInfo.InvariantCulture) + " " + s.Date.Year,
                    s => s.Date, true));

            SelectedTransaction = null;
            //We have to set the command here to ensure that the selection changed event is triggered earlier
            EditCommand = new MvxCommand(Edit);
        }

        private void GoToAddTransaction(string type)
        {
            ShowViewModel<ModifyPaymentViewModel>(new {isEdit = false, typeString = type});
        }

        private async void DeleteAccount()
        {
            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                accountRepository.Delete(accountRepository.Selected);
                accountRepository.RemovePaymentAmount(SelectedTransaction);
                balanceViewModel.UpdateBalance();
                Close(this);
            }
        }

        private void Edit()
        {
            if (SelectedTransaction == null)
            {
                return;
            }

            paymentRepository.Selected = SelectedTransaction;

            ShowViewModel<ModifyPaymentViewModel>(
                new {isEdit = true, typeString = SelectedTransaction.Type.ToString()});
            SelectedTransaction = null;
        }


        private async void DeleteTransaction(Payment transaction)
        {
            if (!await
                dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteTransactionConfirmationMessage))
                return;

            accountRepository.RemovePaymentAmount(transaction);
            paymentRepository.Delete(transaction);
            RelatedTransactions.Remove(transaction);
            balanceViewModel.UpdateBalance(true);
        }
    }
}