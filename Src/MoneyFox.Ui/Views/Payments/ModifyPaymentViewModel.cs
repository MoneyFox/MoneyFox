namespace MoneyFox.Ui.Views.Payments;

using System.Collections.ObjectModel;
using Accounts;
using AutoMapper;
using Categories;
using Common.Extensions;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Interfaces;
using Core.Common.Messages;
using Core.Queries;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.AppCenter.Crashes;
using Resources.Strings;
using Serilog;

// ReSharper disable once PartialTypeWithSinglePart
internal abstract partial class ModifyPaymentViewModel : BaseViewModel, IRecipient<CategorySelectedMessage>
{
    private readonly IDialogService dialogService;
    private readonly IMapper mapper;
    private readonly IMediator mediator;
    private readonly IToastService toastService;
    private ObservableCollection<AccountViewModel> chargedAccounts = new();

    private PaymentViewModel selectedPayment = new();
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

    public RelayCommand ResetCategoryCommand => new(() => SelectedPayment.Category = null);

    protected bool IsFirstLoad { get; set; } = true;

    public async void Receive(CategorySelectedMessage message)
    {
        var category = await mediator.Send(new GetCategoryByIdQuery(message.Value.CategoryId));

        SelectedPayment.Category = new CategoryListItemViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Note = category.Note,
            RequireNote = category.RequireNote,
            Created = category.Created,
            LastModified = category.LastModified
        };
    }

    protected async Task InitializeAsync()
    {
        var accounts = mapper.Map<List<AccountViewModel>>(await mediator.Send(new GetAccountsQuery()));
        ChargedAccounts = new(accounts);
        TargetAccounts = new(accounts);
        IsActive = true;
        IsFirstLoad = false;
    }

    protected abstract Task SavePaymentAsync();

    [RelayCommand]
    private async Task Save()
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

        if (SelectedPayment.Category?.RequireNote == true && string.IsNullOrEmpty(SelectedPayment.Note))
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
            Messenger.Send(new ReloadMessage());
            await Shell.Current.Navigation.PopModalAsync();
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
