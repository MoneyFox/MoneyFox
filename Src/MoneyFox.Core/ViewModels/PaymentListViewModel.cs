using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Foundation.Resources;
using MoneyManager.Foundation.Groups;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Interfaces.ViewModels;
using MoneyManager.Foundation.Model;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class PaymentListViewModel : BaseViewModel, IPaymentListViewModel
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

            BalanceViewModel = new PaymentListBalanceViewModel(accountRepository, paymentRepository);
        }

        public IBalanceViewModel BalanceViewModel { get; }

        /// <summary>
        ///     Loads the data for this view.
        /// </summary>
        public virtual RelayCommand LoadCommand => new RelayCommand(LoadPayments);

        /// <summary>
        ///     Navigate to the add payment view.
        /// </summary>
        public RelayCommand<string> GoToAddPaymentCommand => new RelayCommand<string>(GoToAddPayment);

        /// <summary>
        ///     Deletes the current account and updates the balance.
        /// </summary>
        public RelayCommand DeleteAccountCommand => new RelayCommand(DeleteAccount);

        /// <summary>
        ///     Edits the passed payment.
        /// </summary>
        public RelayCommand<Payment> EditCommand { get; private set; }

        /// <summary>
        ///     Deletes the passed payment.
        /// </summary>
        public RelayCommand<Payment> DeletePaymentCommand => new RelayCommand<Payment>(DeletePayment);

        /// <summary>
        ///     Returns all Payment who are assigned to this repository
        ///     This has to stay until the android list with headers is implemented.
        ///     Currently only used for Android
        /// </summary>
        public ObservableCollection<Payment> RelatedPayments { get; set; }

        /// <summary>
        ///     Returns groupped related payments
        /// </summary>
        public ObservableCollection<DateListGroup<Payment>> Source { get; set; }

        /// <summary>
        ///     Returns the name of the account title for the current page
        /// </summary>
        public string Title => accountRepository.Selected.Name;

        private void LoadPayments()
        {
            EditCommand = null;
            //Refresh balance control with the current account
            BalanceViewModel.UpdateBalanceCommand.Execute();

            RelatedPayments = new ObservableCollection<Payment>(paymentRepository
                .GetRelatedPayments(accountRepository.Selected)
                .OrderByDescending(x => x.Date)
                .ToList());

            Source = new ObservableCollection<DateListGroup<Payment>>(
                DateListGroup<Payment>.CreateGroups(RelatedPayments,
                    CultureInfo.CurrentUICulture,
                    s => s.Date.ToString("d", CultureInfo.InvariantCulture),
                    s => s.Date, true));

            //We have to set the command here to ensure that the selection changed event is triggered earlier
            EditCommand = new RelayCommand<Payment>(Edit);
        }

        private void GoToAddPayment(string type)
        {
            ShowViewModel<ModifyPaymentViewModel>(new {isEdit = false, typeString = type});
        }

        private async void DeleteAccount()
        {
            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                accountRepository.Delete(accountRepository.Selected);
                balanceViewModel.UpdateBalanceCommand.Execute();
                Close(this);
            }
        }

        private void Edit(Payment payment)
        {
            paymentRepository.Selected = payment;

            ShowViewModel<ModifyPaymentViewModel>(new {isEdit = true, typeString = payment.Type.ToString()});
        }

        private async void DeletePayment(Payment payment)
        {
            if (!await
                dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeletePaymentConfirmationMessage))
                return;

            accountRepository.RemovePaymentAmount(payment);
            paymentRepository.Delete(payment);
            LoadCommand.Execute();
        }
    }
}