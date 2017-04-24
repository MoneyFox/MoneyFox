using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using MoneyFox.Business.Manager;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.Service.DataServices;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace MoneyFox.Business.ViewModels
{
    public class PaymentListViewModel : BaseViewModel, IPaymentListViewModel
    {
        private readonly IAccountService accountService;
        private readonly IPaymentService paymentService;
        private readonly IDialogService dialogService;
        private readonly IPaymentRepository paymentRepository;
        private readonly ISettingsManager settingsManager;
        private readonly IBalanceCalculationManager balanceCalculationManager;
        private readonly IBackupManager backupManager;
        private readonly IModifyDialogService modifyDialogService;

        private ObservableCollection<PaymentViewModel> relatedPayments;
        private ObservableCollection<DateListGroup<DateListGroup<PaymentViewModel>>> source;
        private IBalanceViewModel balanceViewModel;
        private IPaymentListViewActionViewModel viewActionViewModel;
        private int accountId;

        public PaymentListViewModel(IAccountService accountService,
                                    IPaymentService paymentService,
            IPaymentRepository paymentRepository,
            IDialogService dialogService,
            ISettingsManager settingsManager,
                                    IBalanceCalculationManager balanceCalculationManager, 
            IBackupManager backupManager, 
            IModifyDialogService modifyDialogService)
        {
            this.accountService = accountService;
            this.paymentService = paymentService;
            this.paymentRepository = paymentRepository;
            this.dialogService = dialogService;
            this.settingsManager = settingsManager;
            this.balanceCalculationManager = balanceCalculationManager;
            this.backupManager = backupManager;
            this.modifyDialogService = modifyDialogService;
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
        public IPaymentListViewActionViewModel ViewActionViewModel => viewActionViewModel;

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
            BalanceViewModel = new PaymentListBalanceViewModel(accountService, balanceCalculationManager, AccountId);
            viewActionViewModel = new PaymentListViewActionViewModel(accountService, settingsManager, dialogService, BalanceViewModel, AccountId);
        }

        private void LoadPayments()
        {
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

            var dailyList = DateListGroup<PaymentViewModel>.CreateGroups(RelatedPayments,
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

            await paymentService.DeletePayment(payment.Payment);
           
            settingsManager.LastDatabaseUpdate = DateTime.Now;
#pragma warning disable 4014
            backupManager.EnqueueBackupTask();
#pragma warning restore 4014
            LoadCommand.Execute();
        }
    }
}