namespace MoneyFox.Ui.Views.Payments.PaymentModification;

using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Aptabase.Maui;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Controls.AccountPicker;
using Controls.CategorySelection;
using Core.Common.Interfaces;
using Core.Common.Settings;
using Core.Queries;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Resources.Strings;
using Serilog;

public abstract class ModifyPaymentViewModel(
    IMediator mediator,
    IDialogService service,
    IToastService toastService,
    CategorySelectionViewModel categorySelectionViewModel,
    ISettingsFacade facade,
    INavigationService navigationService,
    IAptabaseClient client) : NavigableViewModel
{
    private ObservableCollection<AccountPickerViewModel> chargedAccounts = [];

    private PaymentViewModel selectedPayment = new();
    private ObservableCollection<AccountPickerViewModel> targetAccounts = [];

    public PaymentViewModel SelectedPayment
    {
        get => selectedPayment;

        set
        {
            selectedPayment = value;
            OnPropertyChanged();
        }
    }

    public CategorySelectionViewModel CategorySelectionViewModel { get; } = categorySelectionViewModel;

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

    public override async Task OnNavigatedAsync(object? parameter)
    {
        var accounts = await mediator.Send(new GetAccountsQuery());
        var pickerVms = accounts.Select(
                a => new AccountPickerViewModel(
                    Id: a.Id,
                    Name: a.Name,
                    CurrentBalance: new(amount: a.CurrentBalance, currencyAlphaIsoCode: facade.DefaultCurrency)))
            .ToImmutableList();

        ChargedAccounts = new(pickerVms);
        TargetAccounts = new(pickerVms);
        IsFirstLoad = false;
    }

    public override async Task OnNavigatedBackAsync(object? parameter)
    {
        if (parameter is not null)
        {
            var selectedCategoryId = Convert.ToInt32(parameter);
            var category = await mediator.Send(new GetCategoryByIdQuery(selectedCategoryId));
            CategorySelectionViewModel.SelectedCategory = new() { Id = category.Id, Name = category.Name, RequireNote = category.RequireNote };
        }
    }

    protected abstract Task SavePaymentAsync();

    private async Task SaveAsync()
    {
        if (SelectedPayment.ChargedAccount == null)
        {
            await service.ShowMessageAsync(title: Translations.MandatoryFieldEmptyTitle, message: Translations.AccountRequiredMessage);

            return;
        }

        if (SelectedPayment.Amount < 0)
        {
            await service.ShowMessageAsync(title: Translations.AmountMayNotBeNegativeTitle, message: Translations.AmountMayNotBeNegativeMessage);

            return;
        }

        if (CategorySelectionViewModel.SelectedCategory?.RequireNote is true && string.IsNullOrEmpty(SelectedPayment.Note))
        {
            await service.ShowMessageAsync(title: Translations.MandatoryFieldEmptyTitle, message: Translations.ANoteForPaymentIsRequired);

            return;
        }

        if (SelectedPayment.IsRecurring
            && RecurrenceViewModel.IsEndless is false
            && RecurrenceViewModel.EndDate.HasValue
            && RecurrenceViewModel.EndDate.Value.Date < DateTime.Today)
        {
            await service.ShowMessageAsync(title: Translations.InvalidEnddateTitle, message: Translations.InvalidEnddateMessage);

            return;
        }

        try
        {
            await SavePaymentAsync();
            await navigationService.GoBack();
        }
        catch (Exception ex)
        {
            client.TrackEvent(eventName: "failed_to_modify_payment", props: new() { { "excpetion", ex } });
            Log.Error(exception: ex, messageTemplate: "Failed to modify payment");
            await toastService.ShowToastAsync(string.Format(format: Translations.UnknownErrorMessage, arg0: ex.Message));
        }
        finally
        {
            await service.HideLoadingDialogAsync();
        }
    }
}
