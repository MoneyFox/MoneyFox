namespace MoneyFox.Ui.ViewModels.Payments;

using System.Collections.ObjectModel;
using Accounts;
using AutoMapper;
using Categories;
using Common.Extensions;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using MoneyFox.Core.ApplicationCore.Queries;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Common.Messages;
using MoneyFox.Core.Resources;

internal abstract partial class ModifyPaymentViewModel : BaseViewModel
{
    private readonly IDialogService dialogService;
    private readonly IMapper mapper;

    private readonly IMediator mediator;
    private ObservableCollection<AccountViewModel> chargedAccounts = new();

    private PaymentViewModel selectedPayment = new();
    private ObservableCollection<AccountViewModel> targetAccounts = new();

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
            OnPropertyChanged(nameof(AccountHeader));
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

    public List<PaymentType> PaymentTypeList => new() { PaymentType.Expense, PaymentType.Income, PaymentType.Transfer };

    public List<PaymentRecurrence> RecurrenceList
        => new()
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

    public AsyncRelayCommand GoToSelectCategoryDialogCommand => new(async () => await Shell.Current.GoToModalAsync(Routes.SelectCategoryRoute));

    public RelayCommand ResetCategoryCommand => new(() => SelectedPayment.Category = null);

    protected virtual async Task InitializeAsync()
    {
        var accounts = mapper.Map<List<AccountViewModel>>(await mediator.Send(new GetAccountsQuery()));
        ChargedAccounts = new(accounts);
        TargetAccounts = new(accounts);
        IsActive = true;
    }

    protected override void OnActivated()
    {
        Messenger.Register<ModifyPaymentViewModel, CategorySelectedMessage>(recipient: this, handler: (r, m) => r.ReceiveMessageAsync(m));
    }

    protected abstract Task SavePaymentAsync();

    [RelayCommand]
    private async Task Save()
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

        SelectedPayment.Category = mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery(message.Value.CategoryId)));
    }
}
