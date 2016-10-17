using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using MoneyFox.Shared.Groups;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Interfaces.ViewModels;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace MoneyFox.Shared.ViewModels
{
    public class PaymentListViewModel : BaseViewModel, IPaymentListViewModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly IDialogService dialogService;
        private readonly IPaymentManager paymentManager;
        private readonly IPaymentRepository paymentRepository;
        private readonly IRepository<RecurringPayment> recurringPaymentRepository;
        private readonly ISettingsManager settingsManager;
        private readonly IEndOfMonthManager endOfMonthManager;

        private ObservableCollection<Payment> relatedPayments;
        private ObservableCollection<DateListGroup<Payment>> source;

        public PaymentListViewModel(IAccountRepository accountRepository,
            IPaymentRepository paymentRepository,
            IRepository<RecurringPayment> recurringPaymentRepository,
            IPaymentManager paymentManager,
            IDialogService dialogService,
            ISettingsManager settingsManager,
            IEndOfMonthManager endOfMonthManager)
        {
            this.paymentManager = paymentManager;
            this.accountRepository = accountRepository;
            this.paymentRepository = paymentRepository;
            this.recurringPaymentRepository = recurringPaymentRepository;
            this.dialogService = dialogService;
            this.settingsManager = settingsManager;
            this.endOfMonthManager = endOfMonthManager;
        }

        public bool IsPaymentsEmtpy => (RelatedPayments != null) && !RelatedPayments.Any();

        public int AccountId { get; private set; }

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        public IBalanceViewModel BalanceViewModel { get; private set; }

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
        ///     Currently only used for Android to get the selected payment.
        /// </summary>
        public ObservableCollection<Payment> RelatedPayments
        {
            get { return relatedPayments; }
            set
            {
                if (relatedPayments == value) return;
                relatedPayments = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Returns groupped related payments
        /// </summary>
        public ObservableCollection<DateListGroup<Payment>> Source
        {
            get { return source; }
            set
            {
                source = value;
                RaisePropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(IsPaymentsEmtpy));
            }
        }

        /// <summary>
        ///     Returns the name of the account title for the current page
        /// </summary>
        public string Title => accountRepository.FindById(AccountId).Name;

        public void Init(int id)
        {
            AccountId = id;
            BalanceViewModel = new PaymentListBalanceViewModel(accountRepository, endOfMonthManager, AccountId);
        }

        private void LoadPayments()
        {
            EditCommand = null;
            //Refresh balance control with the current account
            BalanceViewModel.UpdateBalanceCommand.Execute();

            RelatedPayments = new ObservableCollection<Payment>(paymentRepository
                .GetList(x => (x.ChargedAccountId == AccountId) || (x.TargetAccountId == AccountId))
                .OrderByDescending(x => x.Date)
                .ToList());

            foreach (var payment in RelatedPayments)
            {
                payment.CurrentAccountId = AccountId;
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
        private void GoToAddPayment(string paymentType)
        {
            ShowViewModel<ModifyPaymentViewModel>(
                new {type = (PaymentType) Enum.Parse(typeof(PaymentType), paymentType)});
        }

        // TODO: I'm pretty sure this shouldn't exist in this ViewModel - Seth Bartlett 7/1/2016 12:06PM
        // This may actually exist from the buttons at the bottom right of the view, if so, this view should be separated out. - Seth Bartlett 7/1/2016 2:31AM
        private async void DeleteAccount()
        {
            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                if (accountRepository.Delete(accountRepository.FindById(AccountId)))
                {
                    settingsManager.LastDatabaseUpdate = DateTime.Now;
                }
                BalanceViewModel.UpdateBalanceCommand.Execute();
                Close(this);
            }
        }

        private void Edit(Payment payment)
        {
            ShowViewModel<ModifyPaymentViewModel>(new {paymentId = payment.Id});
        }

        private async void DeletePayment(Payment payment)
        {
            if (!await
                dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeletePaymentConfirmationMessage))
            {
                return;
            }

            if (await paymentManager.CheckRecurrenceOfPayment(payment))
            {
                paymentManager.RemoveRecurringForPayment(payment);
                recurringPaymentRepository.Delete(payment.RecurringPayment);
            }

            var accountSucceded = paymentManager.RemovePaymentAmount(payment);
            var paymentSucceded = paymentRepository.Delete(payment);
            if (accountSucceded && paymentSucceded)
            {
                settingsManager.LastDatabaseUpdate = DateTime.Now;
            }
            LoadCommand.Execute();
        }
    }
}