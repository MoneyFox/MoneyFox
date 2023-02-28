namespace MoneyFox.Ui.Views.Payments.PaymentModification;

using System.Collections.ObjectModel;
using Accounts;
using AutoMapper;
using Common.Extensions;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Interfaces;
using Core.Queries;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Messages;
using Microsoft.AppCenter.Crashes;
using MoneyFox.Ui.Views.Payments.PaymentList;
using Resources.Strings;
using Serilog;

public abstract class ModifyPaymentViewModel : BasePageViewModel, IRecipient<CategorySelectedMessage>
{
    private readonly IDialogService dialogService;
    private readonly IMapper mapper;
    private readonly IMediator mediator;
    private readonly IToastService toastService;
    private ObservableCollection<AccountViewModel> chargedAccounts = new();

    private PaymentViewModel selectedPayment = new();
    private SelectedCategoryViewModel? selectedCategory;
    private ObservableCollection<AccountViewModel> targetAccounts = new();

    protected ModifyPaymentViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService, IToastService toastService)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.dialogService = dialogService;
        this.toastService = toastService;
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

    public SelectedCategoryViewModel? SelectedCategory
    {
        get => selectedCategory;

        set
        {
            selectedCategory = value;
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

    public static List<PaymentType> PaymentTypeList => new() { PaymentType.Expense, PaymentType.Income, PaymentType.Transfer };

    public static List<PaymentRecurrence> RecurrenceList
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

    public string AccountHeader => SelectedPayment?.Type == PaymentType.Income ? Translations.TargetAccountLabel : Translations.ChargedAccountLabel;

    public AsyncRelayCommand GoToSelectCategoryDialogCommand => new(async () => await Shell.Current.GoToModalAsync(Routes.SelectCategoryRoute));

    public RelayCommand ResetCategoryCommand => new(() => SelectedCategory = null);

    protected bool IsFirstLoad { get; set; } = true;

    public AsyncRelayCommand SaveCommand => new(SaveAsync);

    public async void Receive(CategorySelectedMessage message)
    {
        IsActive = true;
        var category = await mediator.Send(new GetCategoryByIdQuery(message.Value.CategoryId));
        SelectedCategory = new() { Id = category.Id, Name = category.Name, RequireNote = category.RequireNote };
    }

    protected async Task InitializeAsync()
    {
        var accounts = mapper.Map<List<AccountViewModel>>(await mediator.Send(new GetAccountsQuery()));
        ChargedAccounts = new(accounts);
        TargetAccounts = new(accounts);
        IsFirstLoad = false;
    }

    protected abstract Task SavePaymentAsync();

    private async Task SaveAsync()
    {
        if (SelectedPayment.ChargedAccount == null)
        {
            await dialogService.ShowMessageAsync(title: Translations.MandatoryFieldEmptyTitle, message: Translations.AccountRequiredMessage);

            return;
        }

        if (SelectedPayment.Amount < 0)
        {
            await dialogService.ShowMessageAsync(title: Translations.AmountMayNotBeNegativeTitle, message: Translations.AmountMayNotBeNegativeMessage);

            return;
        }

        if (SelectedCategory?.RequireNote is true && string.IsNullOrEmpty(SelectedPayment.Note))
        {
            await dialogService.ShowMessageAsync(title: Translations.MandatoryFieldEmptyTitle, message: Translations.ANoteForPaymentIsRequired);

            return;
        }

        if (SelectedPayment.IsRecurring
            && !SelectedPayment.RecurringPayment!.IsEndless
            && SelectedPayment.RecurringPayment.EndDate.HasValue
            && SelectedPayment.RecurringPayment.EndDate.Value.Date < DateTime.Today)
        {
            await dialogService.ShowMessageAsync(title: Translations.InvalidEnddateTitle, message: Translations.InvalidEnddateMessage);

            return;
        }

        try
        {
            await SavePaymentAsync();
            await Shell.Current.Navigation.PopModalAsync();
            Messenger.Send(new PaymentsChangedMessage());
        }
        catch (Exception ex)
        {
            Crashes.TrackError(ex);
            Log.Error(exception: ex, messageTemplate: "Failed to modify payment");
            await toastService.ShowToastAsync(string.Format(format: Translations.UnknownErrorMessage, arg0: ex.Message));
        }
        finally
        {
            await dialogService.HideLoadingDialogAsync();
        }
    }
}
