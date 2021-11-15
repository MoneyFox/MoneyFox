﻿using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccounts;
using MoneyFox.Application.Categories.Queries.GetCategoryById;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Extensions;
using MoneyFox.ViewModels.Accounts;
using MoneyFox.ViewModels.Categories;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Payments
{
    public abstract class ModifyPaymentViewModel : ObservableRecipient
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private PaymentViewModel selectedPayment = new PaymentViewModel();
        private ObservableCollection<AccountViewModel> chargedAccounts = new ObservableCollection<AccountViewModel>();
        private ObservableCollection<AccountViewModel> targetAccounts = new ObservableCollection<AccountViewModel>();

        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IDialogService dialogService;

        protected ModifyPaymentViewModel(IMediator mediator,
            IMapper mapper,
            IDialogService dialogService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.dialogService = dialogService;
        }

        protected override void OnActivated()
            => Messenger.Register<ModifyPaymentViewModel, CategorySelectedMessage>(
                this,
                (r, m) => r.ReceiveMessageAsync(m));

        /// <summary>
        ///     The currently selected PaymentViewModel
        /// </summary>
        public PaymentViewModel SelectedPayment
        {
            get => selectedPayment;
            set
            {
                selectedPayment = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gives access to all accounts for Charged Dropdown list
        /// </summary>
        public ObservableCollection<AccountViewModel> ChargedAccounts
        {
            get => chargedAccounts;
            private set
            {
                chargedAccounts = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gives access to all accounts for Target Dropdown list
        /// </summary>
        public ObservableCollection<AccountViewModel> TargetAccounts
        {
            get => targetAccounts;
            private set
            {
                targetAccounts = value;
                OnPropertyChanged();
            }
        }

        protected virtual async Task InitializeAsync()
        {
            var accounts = mapper.Map<List<AccountViewModel>>(await mediator.Send(new GetAccountsQuery()));

            ChargedAccounts = new ObservableCollection<AccountViewModel>(accounts);
            TargetAccounts = new ObservableCollection<AccountViewModel>(accounts);
        }

        /// <summary>
        ///     Indicates if the PaymentViewModel is a transfer.
        /// </summary>
        public bool IsTransfer => SelectedPayment.IsTransfer;

        public List<PaymentType> PaymentTypeList => new List<PaymentType>
        {
            PaymentType.Expense, PaymentType.Income, PaymentType.Transfer
        };

        /// <summary>
        ///     List with the different recurrence types.     This has to have the same order as the enum
        /// </summary>
        public List<PaymentRecurrence> RecurrenceList => new List<PaymentRecurrence>
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

        public string AccountHeader
            => SelectedPayment?.Type == PaymentType.Income
                ? Strings.TargetAccountLabel
                : Strings.ChargedAccountLabel;

        public RelayCommand GoToSelectCategoryDialogCommand => new RelayCommand(
            async () =>
                await Shell.Current.GoToModalAsync(ViewModelLocator.SelectCategoryRoute));

        public RelayCommand ResetCategoryCommand => new RelayCommand(() => SelectedPayment.Category = null);

        public RelayCommand SaveCommand => new RelayCommand(async () => await SavePaymentBaseAsync());

        protected abstract Task SavePaymentAsync();

        private async Task SavePaymentBaseAsync()
        {
            if(SelectedPayment.ChargedAccount == null)
            {
                await dialogService.ShowMessageAsync(Strings.MandatoryFieldEmptyTitle, Strings.AccountRequiredMessage);
                return;
            }

            if(SelectedPayment.Amount < 0)
            {
                await dialogService.ShowMessageAsync(
                    Strings.AmountMayNotBeNegativeTitle,
                    Strings.AmountMayNotBeNegativeMessage);
                return;
            }

            if(SelectedPayment.Category?.RequireNote == true && string.IsNullOrEmpty(SelectedPayment.Note))
            {
                await dialogService.ShowMessageAsync(
                    Strings.MandatoryFieldEmptyTitle,
                    Strings.ANoteForPaymentIsRequired);
                return;
            }

            if(SelectedPayment.IsRecurring
               && !SelectedPayment.RecurringPayment!.IsEndless
               && SelectedPayment.RecurringPayment.EndDate.HasValue
               && SelectedPayment.RecurringPayment.EndDate.Value.Date < DateTime.Today)
            {
                await dialogService.ShowMessageAsync(Strings.InvalidEnddateTitle, Strings.InvalidEnddateMessage);
                return;
            }

            await dialogService.ShowLoadingDialogAsync(Strings.SavingPaymentMessage);

            try
            {
                await SavePaymentAsync();
                Messenger.Send(new ReloadMessage());
                await Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync();
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            finally
            {
                await dialogService.HideLoadingDialogAsync();
            }
        }

        public async Task ReceiveMessageAsync(CategorySelectedMessage message)
        {
            if(SelectedPayment == null || message == null)
            {
                return;
            }

            SelectedPayment.Category =
                mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery(message.CategoryId)));
        }
    }
}