namespace MoneyFox.ViewModels.Payments
{

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Accounts;
    using AutoMapper;
    using Categories;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Core._Pending_.Common.Messages;
    using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using Core.ApplicationCore.Queries;
    using Core.Common.Interfaces;
    using Core.Resources;
    using Extensions;
    using MediatR;
    using Xamarin.Forms;

    internal abstract class ModifyPaymentViewModel : BaseViewModel
    {
        private readonly IDialogService dialogService;
        private readonly IMapper mapper;

        private readonly IMediator mediator;
        private ObservableCollection<AccountViewModel> chargedAccounts = new ObservableCollection<AccountViewModel>();

        private PaymentViewModel selectedPayment = new PaymentViewModel();
        private ObservableCollection<AccountViewModel> targetAccounts = new ObservableCollection<AccountViewModel>();

        protected ModifyPaymentViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.dialogService = dialogService;
        }

        public PaymentViewModel SelectedPayment
        {
            get => selectedPayment;

            set
            {
                selectedPayment = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AccountViewModel> ChargedAccounts
        {
            get => chargedAccounts;

            private set
            {
                chargedAccounts = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AccountViewModel> TargetAccounts
        {
            get => targetAccounts;

            private set
            {
                targetAccounts = value;
                OnPropertyChanged();
            }
        }

        public bool IsTransfer => SelectedPayment.IsTransfer;

        public List<PaymentType> PaymentTypeList => new List<PaymentType> { PaymentType.Expense, PaymentType.Income, PaymentType.Transfer };

        /// <summary>
        ///     List with the different recurrence types. This has to have the same order as the enum
        /// </summary>
        public List<PaymentRecurrence> RecurrenceList
            => new List<PaymentRecurrence>
            {
                PaymentRecurrence.Daily,
                PaymentRecurrence.DailyWithoutWeekend,
                PaymentRecurrence.Weekly,
                PaymentRecurrence.Biweekly,
                PaymentRecurrence.Monthly,
                PaymentRecurrence.Bimonthly,
                PaymentRecurrence.Quarterly,
                PaymentRecurrence.Biannually,
                PaymentRecurrence.Yearly
            };

        public string AccountHeader => SelectedPayment?.Type == PaymentType.Income ? Strings.TargetAccountLabel : Strings.ChargedAccountLabel;

        public RelayCommand GoToSelectCategoryDialogCommand
            => new RelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.SelectCategoryRoute));

        public RelayCommand ResetCategoryCommand => new RelayCommand(() => SelectedPayment.Category = null);

        public RelayCommand SaveCommand => new RelayCommand(async () => await SavePaymentBaseAsync());

        protected virtual async Task InitializeAsync()
        {
            var accounts = mapper.Map<List<AccountViewModel>>(await mediator.Send(new GetAccountsQuery()));
            ChargedAccounts = new ObservableCollection<AccountViewModel>(accounts);
            TargetAccounts = new ObservableCollection<AccountViewModel>(accounts);
            IsActive = true;
        }

        protected override void OnActivated()
        {
            Messenger.Register<ModifyPaymentViewModel, CategorySelectedMessage>(recipient: this, handler: (r, m) => r.ReceiveMessageAsync(m));
        }

        protected abstract Task SavePaymentAsync();

        private async Task SavePaymentBaseAsync()
        {
            if (SelectedPayment.ChargedAccount == null)
            {
                await dialogService.ShowMessageAsync(title: Strings.MandatoryFieldEmptyTitle, message: Strings.AccountRequiredMessage);

                return;
            }

            if (SelectedPayment.Amount < 0)
            {
                await dialogService.ShowMessageAsync(title: Strings.AmountMayNotBeNegativeTitle, message: Strings.AmountMayNotBeNegativeMessage);

                return;
            }

            if (SelectedPayment.Category?.RequireNote == true && string.IsNullOrEmpty(SelectedPayment.Note))
            {
                await dialogService.ShowMessageAsync(title: Strings.MandatoryFieldEmptyTitle, message: Strings.ANoteForPaymentIsRequired);

                return;
            }

            if (SelectedPayment.IsRecurring
                && !SelectedPayment.RecurringPayment!.IsEndless
                && SelectedPayment.RecurringPayment.EndDate.HasValue
                && SelectedPayment.RecurringPayment.EndDate.Value.Date < DateTime.Today)
            {
                await dialogService.ShowMessageAsync(title: Strings.InvalidEnddateTitle, message: Strings.InvalidEnddateMessage);

                return;
            }

            await dialogService.ShowLoadingDialogAsync(Strings.SavingPaymentMessage);
            try
            {
                await SavePaymentAsync();
                Messenger.Send(new ReloadMessage());
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            finally
            {
                await dialogService.HideLoadingDialogAsync();
            }
        }

        private async Task ReceiveMessageAsync(CategorySelectedMessage message)
        {
            if (SelectedPayment == null || message == null)
            {
                return;
            }

            SelectedPayment.Category = mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery(message.CategoryId)));
        }
    }

}
