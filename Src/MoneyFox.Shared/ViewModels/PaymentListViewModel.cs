using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using MoneyFox.Shared.Groups;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.ViewModels;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;
using PropertyChanged;
using MoneyFox.Shared.Helpers;
using System;

namespace MoneyFox.Shared.ViewModels {
    [ImplementPropertyChanged]
    public class PaymentListViewModel : BaseViewModel, IPaymentListViewModel {
        private readonly IAccountRepository accountRepository;
        private readonly IDialogService dialogService;
        private readonly IPaymentManager paymentManager;
        private readonly IPaymentRepository paymentRepository;
        private IBalanceViewModel balanceViewModel;
        private int accountId;

        public PaymentListViewModel(IPaymentRepository paymentRepository,
            IAccountRepository accountRepository,
            IDialogService dialogService, IPaymentManager paymentManager) {
            this.paymentRepository = paymentRepository;
            this.accountRepository = accountRepository;
            this.dialogService = dialogService;
            this.paymentManager = paymentManager;

            
        }

        public void Init(int id)
        {
            accountId = id;
            balanceViewModel = new PaymentListBalanceViewModel(accountRepository, paymentRepository, AccountId);
        }

        public bool IsPaymentsEmtpy => RelatedPayments != null && !RelatedPayments.Any();

        public IBalanceViewModel BalanceViewModel => balanceViewModel;

        public int AccountId => accountId;
        /// <summary>
        ///     Loads the data for this view.
        /// </summary>
        public virtual MvxCommand LoadCommand => new MvxCommand(LoadPayments);

        /// <summary>
        ///     Navigate to the add payment view.
        /// </summary>
        public MvxCommand<string> GoToAddPaymentCommand => new MvxCommand<string>(GoToAddPayment);

        /// <summary>
        ///     Deletes the current account and updates the balance.
        /// </summary>
        public MvxCommand DeleteAccountCommand => new MvxCommand(DeleteAccount);

        /// <summary>
        ///     Edits the passed payment.
        /// </summary>
        public MvxCommand<Payment> EditCommand { get; private set; }

        /// <summary>
        ///     Deletes the passed payment.
        /// </summary>
        public MvxCommand<Payment> DeletePaymentCommand => new MvxCommand<Payment>(DeletePayment);

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
        public string Title => accountRepository.FindById(AccountId).Name;

        private void LoadPayments() {
            EditCommand = null;
            //Refresh balance control with the current account
            BalanceViewModel.UpdateBalanceCommand.Execute();

            RelatedPayments = new ObservableCollection<Payment>(paymentRepository
                .GetRelatedPayments(AccountId)
                .OrderByDescending(x => x.Date)
                .ToList());

            foreach (var payment in RelatedPayments)
            {
                payment.CurrentAccountId = accountId;
            }

            Source = new ObservableCollection<DateListGroup<Payment>>(
                DateListGroup<Payment>.CreateGroups(RelatedPayments,
                    CultureInfo.CurrentUICulture,
                    s => s.Date.ToString("MMMM", CultureInfo.InvariantCulture) + " " + s.Date.Year,
                    s => s.Date, true));

            //We have to set the command here to ensure that the selection changed event is triggered earlier
            EditCommand = new MvxCommand<Payment>(Edit);
        }

        // TODO: Use the actual enum rather than magic strings - Seth Bartlett 7/1/2016 12:07PM
        private void GoToAddPayment(string type) {
            ShowViewModel<ModifyPaymentViewModel>(new {isEdit = false, typeString = type});
        }

        // TODO: I'm pretty sure this shouldn't exist in this ViewModel - Seth Bartlett 7/1/2016 12:06PM
        private async void DeleteAccount() {
            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage)) {
                if(accountRepository.Delete(accountRepository.FindById(AccountId)))
                    SettingsHelper.LastDatabaseUpdate = DateTime.Now;
                BalanceViewModel.UpdateBalanceCommand.Execute();
                Close(this);
            }
        }

        private void Edit(Payment payment) {
            paymentRepository.Selected = payment;

            ShowViewModel<ModifyPaymentViewModel>(new {isEdit = true, typeString = payment.Type.ToString()});
        }

        private async void DeletePayment(Payment payment) {
            if (!await
                dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeletePaymentConfirmationMessage)) {
                return;
            }

            if (await paymentManager.CheckForRecurringPayment(payment)) {
                paymentRepository.DeleteRecurring(payment);
            }

            bool accountSucceded = accountRepository.RemovePaymentAmount(payment);
            bool paymentSucceded = paymentRepository.Delete(payment);
            if(accountSucceded && paymentSucceded)
                SettingsHelper.LastDatabaseUpdate = DateTime.Now;
            LoadCommand.Execute();
        }
    }
}