namespace MoneyFox.Ui.Views.Payments.PaymentModification;

using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Aptabase.Maui;
using Categories.CategorySelection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Controls.AccountPicker;
using Controls.CategorySelection;
using Core.Common.Interfaces;
using Core.Common.Settings;
using Core.Queries;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Resources.Strings;
using Serilog;

public abstract class ModifyPaymentViewModel : BasePageViewModel, IQueryAttributable
{
    private readonly IAptabaseClient aptabaseClient;
    private readonly IDialogService dialogService;
    private readonly IMediator mediator;
    private readonly ISettingsFacade settingsFacade;
    private readonly IToastService toastService;
    private ObservableCollection<AccountPickerViewModel> chargedAccounts = new();

    private PaymentViewModel selectedPayment = new();
    private ObservableCollection<AccountPickerViewModel> targetAccounts = new();

    protected ModifyPaymentViewModel(
        IMediator mediator,
        IDialogService dialogService,
        IToastService toastService,
        CategorySelectionViewModel categorySelectionViewModel,
        ISettingsFacade settingsFacade,
        IAptabaseClient aptabaseClient)
    {
        this.mediator = mediator;
        this.dialogService = dialogService;
        this.toastService = toastService;
        CategorySelectionViewModel = categorySelectionViewModel;
        this.settingsFacade = settingsFacade;
        this.aptabaseClient = aptabaseClient;
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

    public CategorySelectionViewModel CategorySelectionViewModel { get; }

    public RecurrenceViewModel RecurrenceViewModel { get; protected set; } = new();

    public ObservableCollection<AccountPickerViewModel> ChargedAccounts
    {
        get => chargedAccounts;

        private set
        {
            chargedAccounts = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<AccountPickerViewModel> TargetAccounts
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
            PaymentRecurrence.Weekly,
            PaymentRecurrence.Biweekly,
            PaymentRecurrence.Monthly,
            PaymentRecurrence.Bimonthly,
            PaymentRecurrence.Quarterly,
            PaymentRecurrence.Biannually,
            PaymentRecurrence.Yearly
        };

    public string AccountHeader => SelectedPayment.Type == PaymentType.Income ? Translations.TargetAccountLabel : Translations.ChargedAccountLabel;

    protected bool IsFirstLoad { get; set; } = true;

    public AsyncRelayCommand SaveCommand => new(SaveAsync);

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(key: SelectCategoryViewModel.SELECTED_CATEGORY_ID_PARAM, value: out var selectedCategoryIdParam))
        {
            var selectedCategoryId = Convert.ToInt32(selectedCategoryIdParam);
            var category = mediator.Send(new GetCategoryByIdQuery(selectedCategoryId)).GetAwaiter().GetResult();
            CategorySelectionViewModel.SelectedCategory = new() { Id = category.Id, Name = category.Name, RequireNote = category.RequireNote };
        }
    }

    protected async Task InitializeAsync()
    {
        var accounts = await mediator.Send(new GetAccountsQuery());
        var pickerVms = accounts.Select(
                a => new AccountPickerViewModel(
                    Id: a.Id,
                    Name: a.Name,
                    CurrentBalance: new(amount: a.CurrentBalance, currencyAlphaIsoCode: settingsFacade.DefaultCurrency)))
            .ToImmutableList();

        ChargedAccounts = new(pickerVms);
        TargetAccounts = new(pickerVms);
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

        if (CategorySelectionViewModel.SelectedCategory?.RequireNote is true && string.IsNullOrEmpty(SelectedPayment.Note))
        {
            await dialogService.ShowMessageAsync(title: Translations.MandatoryFieldEmptyTitle, message: Translations.ANoteForPaymentIsRequired);

            return;
        }

        if (SelectedPayment.IsRecurring
            && RecurrenceViewModel.IsEndless is false
            && RecurrenceViewModel.EndDate.HasValue
            && RecurrenceViewModel.EndDate.Value.Date < DateTime.Today)
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
            aptabaseClient.TrackEvent(eventName: "failed_to_modify_payment", props: new() { { "excpetion", ex } });
            Log.Error(exception: ex, messageTemplate: "Failed to modify payment");
            await toastService.ShowToastAsync(string.Format(format: Translations.UnknownErrorMessage, arg0: ex.Message));
        }
        finally
        {
            await dialogService.HideLoadingDialogAsync();
        }
    }
}
