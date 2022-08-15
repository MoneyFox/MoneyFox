namespace MoneyFox.Win.ViewModels.Accounts;

using System.Globalization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.ApplicationCore.Queries;
using Core.Common.Interfaces;
using Core.Common.Messages;
using Core.Resources;
using MediatR;
using Services;
using DialogServiceClass = DialogService;

internal abstract class ModifyAccountViewModel : BaseViewModel
{
    private string amountString = "";
    private AccountViewModel selectedAccount = new();
    private string title = "";

    protected ModifyAccountViewModel(IDialogService dialogService, INavigationService navigationService, IMediator mediator)
    {
        DialogService = dialogService;
        NavigationService = navigationService;
        Mediator = mediator;
    }

    public int AccountId { get; set; }

    protected IDialogService DialogService { get; }

    protected INavigationService NavigationService { get; }

    protected IMediator Mediator { get; }

    public AsyncRelayCommand InitializeCommand => new(InitializeAsync);

    public AsyncRelayCommand SaveCommand => new(SaveAccountBaseAsync);

    public string Title
    {
        get => title;

        set
        {
            if (title == value)
            {
                return;
            }

            title = value;
            OnPropertyChanged();
        }
    }

    public virtual bool IsEdit => false;

    public AccountViewModel SelectedAccount
    {
        get => selectedAccount;

        set
        {
            selectedAccount = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(AmountString));
        }
    }

    public string AmountString
    {
        get => amountString;

        set
        {
            if (amountString == value)
            {
                return;
            }

            amountString = value;
            OnPropertyChanged();
        }
    }

    protected abstract Task SaveAccountAsync();

    protected abstract Task InitializeAsync();

    private async Task SaveAccountBaseAsync()
    {
        var openContentDialog = DialogServiceClass.GetOpenContentDialog();
        if (string.IsNullOrWhiteSpace(SelectedAccount.Name))
        {
            DialogServiceClass.HideContentDialog(openContentDialog);
            await DialogService.ShowMessageAsync(title: Strings.MandatoryFieldEmptyTitle, message: Strings.NameRequiredMessage);
            await DialogServiceClass.ShowContentDialog(openContentDialog);

            return;
        }

        var nameChanged = SelectedAccount.Id == 0 || !SelectedAccount.Name.Equals(await Mediator.Send(new GetAccountNameByIdQuery(SelectedAccount.Id)));
        if (nameChanged && await Mediator.Send(new GetIfAccountWithNameExistsQuery(accountName: SelectedAccount.Name, accountId: SelectedAccount.Id)))
        {
            DialogServiceClass.HideContentDialog(openContentDialog);
            if (!await DialogService.ShowConfirmMessageAsync(title: Strings.DuplicatedNameTitle, message: Strings.DuplicateAccountMessage))
            {
                await DialogServiceClass.ShowContentDialog(openContentDialog);

                return;
            }
        }

        if (decimal.TryParse(s: AmountString, style: NumberStyles.Any, provider: CultureInfo.CurrentCulture, result: out var convertedValue))
        {
            SelectedAccount.CurrentBalance = convertedValue;
        }
        else
        {
            DialogServiceClass.HideContentDialog(openContentDialog);
            await DialogService.ShowMessageAsync(title: Strings.InvalidNumberTitle, message: Strings.InvalidNumberCurrentBalanceMessage);
            await DialogServiceClass.ShowContentDialog(openContentDialog);

            return;
        }

        await SaveAccountAsync();
        Messenger.Send(new ReloadMessage());
    }
}
