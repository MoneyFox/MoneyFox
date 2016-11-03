using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Interfaces.ViewModels;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace MoneyFox.Business.ViewModels
{
    public class PaymentListViewModel : BaseViewModel, IPaymentListViewModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly IDialogService dialogService;
        private readonly IPaymentManager paymentManager;
        private readonly IPaymentRepository paymentRepository;
        private readonly ISettingsManager settingsManager;
        private readonly IEndOfMonthManager endOfMonthManager;

        private ObservableCollection<PaymentViewModel> relatedPayments;
        private ObservableCollection<DateListGroup<PaymentViewModel>> source;
        private MvxCommand<PaymentViewModel> editCommand;
        private IBalanceViewModel balanceViewModel;
        private IPaymentListViewActionViewModel viewActionViewModel;
        private int accountId;

        public PaymentListViewModel(IAccountRepository accountRepository,
            IPaymentRepository paymentRepository,
            IPaymentManager paymentManager,
            IDialogService dialogService,
            ISettingsManager settingsManager,
            IEndOfMonthManager endOfMonthManager)
        {
            this.paymentManager = paymentManager;
            this.accountRepository = accountRepository;
            this.paymentRepository = paymentRepository;
            this.dialogService = dialogService;
            this.settingsManager = settingsManager;
            this.endOfMonthManager = endOfMonthManager;
        }

        #region Properties

        public bool IsPaymentsEmtpy => (RelatedPayments != null) && !RelatedPayments.Any();

        public int AccountId
        {
            get { return accountId; }
            private set
            {
                accountId = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        public IBalanceViewModel BalanceViewModel
        {
            get { return balanceViewModel; }
            private set
            {
                balanceViewModel = value;
                RaisePropertyChanged();
            }
        }
        public IPaymentListViewActionViewModel ViewActionViewModel
        {
            get { return viewActionViewModel; }
            private set
            {
                viewActionViewModel = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        ///     Returns all PaymentViewModel who are assigned to this repository
        ///     Currently only used for Android to get the selected PaymentViewModel.
        /// </summary>
        public ObservableCollection<PaymentViewModel> RelatedPayments
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
        public ObservableCollection<DateListGroup<PaymentViewModel>> Source
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

        #endregion

        #region Commands

        /// <summary>
        ///     Loads the data for this view.
        /// </summary>
        public virtual MvxCommand LoadCommand => new MvxCommand(LoadPayments);

        /// <summary>
        ///     Edits the passed PaymentViewModel.
        /// </summary>
        public MvxCommand<PaymentViewModel> EditCommand
        {
            get { return editCommand; }
            private set
            {
                editCommand = value; 
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Deletes the passed PaymentViewModel.
        /// </summary>
        public MvxCommand<PaymentViewModel> DeletePaymentCommand => new MvxCommand<PaymentViewModel>(DeletePayment);
        
        #endregion

        public void Init(int id)
        {
            AccountId = id;
            BalanceViewModel = new PaymentListBalanceViewModel(accountRepository, endOfMonthManager, AccountId);
            viewActionViewModel = new PaymentListViewActionViewModel(accountRepository, settingsManager, dialogService, BalanceViewModel, AccountId);
        }

        private void LoadPayments()
        {
            EditCommand = null;
            //Refresh balance control with the current account
            BalanceViewModel.UpdateBalanceCommand.Execute();

            RelatedPayments = new ObservableCollection<PaymentViewModel>(paymentRepository
                .GetList(x => (x.ChargedAccountId == AccountId) || (x.TargetAccountId == AccountId))
                .OrderByDescending(x => x.Date)
                .ToList());

            foreach (var payment in RelatedPayments)
            {
                payment.CurrentAccountId = AccountId;
            }

            Source = new ObservableCollection<DateListGroup<PaymentViewModel>>(
                DateListGroup<PaymentViewModel>.CreateGroups(RelatedPayments,
                    CultureInfo.CurrentUICulture,
                    s => s.Date.ToString("MMMM", CultureInfo.InvariantCulture) + " " + s.Date.Year,
                    s => s.Date, true));

            //We have to set the command here to ensure that the selection changed event is triggered earlier
            EditCommand = new MvxCommand<PaymentViewModel>(Edit);
        }

        private void Edit(PaymentViewModel payment)
        {
            ShowViewModel<ModifyPaymentViewModel>(new {paymentId = payment.Id});
        }

        private async void DeletePayment(PaymentViewModel payment)
        {
            if (!await dialogService
                .ShowConfirmMessage(Strings.DeleteTitle, Strings.DeletePaymentConfirmationMessage)) return;

            var deletePaymentSucceded = await paymentManager.DeletePayment(payment);
            var deleteAccountSucceded = paymentManager.RemovePaymentAmount(payment);
            if (deletePaymentSucceded && deleteAccountSucceded)
            {
                settingsManager.LastDatabaseUpdate = DateTime.Now;
            }
            LoadCommand.Execute();
        }
    }
}