using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using MoneyFox.Foundation;
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
        private readonly IBackupManager backupManager;
        private readonly IModifyDialogService modifyDialogService;

        private ObservableCollection<DateListGroup<DateListGroup<PaymentViewModel>>> source;
        private IBalanceViewModel balanceViewModel;
        private IPaymentListViewActionViewModel viewActionViewModel;
        private int accountId;

        public PaymentListViewModel(IAccountRepository accountRepository,
            IPaymentRepository paymentRepository,
            IPaymentManager paymentManager,
            IDialogService dialogService,
            ISettingsManager settingsManager,
            IEndOfMonthManager endOfMonthManager, 
            IBackupManager backupManager, 
            IModifyDialogService modifyDialogService)
        {
            this.paymentManager = paymentManager;
            this.accountRepository = accountRepository;
            this.paymentRepository = paymentRepository;
            this.dialogService = dialogService;
            this.settingsManager = settingsManager;
            this.endOfMonthManager = endOfMonthManager;
            this.backupManager = backupManager;
            this.modifyDialogService = modifyDialogService;
        }

        #region Properties

        public bool IsPaymentsEmtpy => (Source != null) && !Source.Any();

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
        ///     Returns groupped related payments
        /// </summary>
        public ObservableCollection<DateListGroup<DateListGroup<PaymentViewModel>>> Source
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
        ///     Opens the Edit Dialog for the passed Payment
        /// </summary>
        public MvxCommand<PaymentViewModel> EditPaymentCommand => new MvxCommand<PaymentViewModel>(EditPayment);

        /// <summary>
        ///     Opens a option dialog to select the modify operation
        /// </summary>
        public MvxCommand<PaymentViewModel> OpenContextMenuCommand => new MvxCommand<PaymentViewModel>(OpenContextMenu);

        /// <summary>
        ///     Deletes the passed PaymentViewModel.
        /// </summary>
        public MvxCommand<PaymentViewModel> DeletePaymentCommand => new MvxCommand<PaymentViewModel>(DeletePayment);
        
        #endregion

        public void Init(int id)
        {
            AccountId = id;
            BalanceViewModel = new PaymentListBalanceViewModel(accountRepository, endOfMonthManager, AccountId);
            viewActionViewModel = new PaymentListViewActionViewModel(accountRepository, paymentManager, settingsManager, dialogService, BalanceViewModel, AccountId);
        }

        private void LoadPayments()
        {
            //Refresh balance control with the current account
            BalanceViewModel.UpdateBalanceCommand.Execute();

            var relatedPayments = paymentRepository
                .GetList(x => (x.ChargedAccountId == AccountId) || (x.TargetAccountId == AccountId))
                .OrderByDescending(x => x.Date)
                .ToList();

            foreach (var payment in relatedPayments)
            {
                payment.CurrentAccountId = AccountId;
            }

            var dailyList = DateListGroup<PaymentViewModel>.CreateGroups(relatedPayments,
                CultureInfo.CurrentUICulture,
                s => s.Date.ToString("D", CultureInfo.InvariantCulture),
                s => s.Date,
                itemClickCommand: EditPaymentCommand, itemLongClickCommand:OpenContextMenuCommand);

            Source = new ObservableCollection<DateListGroup<DateListGroup<PaymentViewModel>>>(
                DateListGroup<DateListGroup<PaymentViewModel>>.CreateGroups(dailyList, CultureInfo.CurrentUICulture,
                    s =>
                    {
                        var date = Convert.ToDateTime(s.Key);
                        return date.ToString("MMMM", CultureInfo.InvariantCulture) + " " + date.Year;
                    },
                    s => Convert.ToDateTime(s.Key)));
        }

        private void EditPayment(PaymentViewModel payment)
        {
            ShowViewModel<ModifyPaymentViewModel>(new {paymentId = payment.Id});
        }
        
        private async void OpenContextMenu(PaymentViewModel payment)
        {
            var result = await modifyDialogService.ShowEditSelectionDialog();

            switch (result)
            {
                case ModifyOperation.Edit:
                    EditPaymentCommand.Execute(payment);
                    break;

                case ModifyOperation.Delete:
                    DeletePaymentCommand.Execute(payment);
                    break;
            }
        }

        private async void DeletePayment(PaymentViewModel payment)
        {
            if (!await dialogService
                .ShowConfirmMessage(Strings.DeleteTitle, Strings.DeletePaymentConfirmationMessage)) return;

            var deletePaymentSucceded = await paymentManager.DeletePayment(payment);
            var removePaymentAmountSuceed = paymentManager.RemovePaymentAmount(payment);
            if (deletePaymentSucceded && removePaymentAmountSuceed)
            {
                settingsManager.LastDatabaseUpdate = DateTime.Now;
#pragma warning disable 4014
                backupManager.EnqueueBackupTask();
#pragma warning restore 4014
            }
            LoadCommand.Execute();
        }
    }
}