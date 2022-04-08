namespace MoneyFox.Win.ViewModels.Accounts;

using DialogServiceClass = DialogService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core._Pending_.Common.Messages;
using Core.Common.Interfaces;
using Core.Resources;
using MediatR;
using Microsoft.UI.Xaml.Controls;
using NLog;
using Services;
using System.Globalization;
using System.Threading.Tasks;
using Core.Queries;

public abstract class ModifyAccountViewModel : ObservableRecipient
{
    public int AccountId { get; set; }

    private string title = "";
    private AccountViewModel selectedAccount = new();

    protected ModifyAccountViewModel(
        IDialogService dialogService,
        INavigationService navigationService,
        IMediator mediator)
    {
        DialogService = dialogService;
        NavigationService = navigationService;
        Mediator = mediator;
    }

    protected abstract Task SaveAccountAsync();

    protected abstract Task InitializeAsync();

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
            if(title == value)
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

    private string amountString = "";

    public string AmountString
    {
        get => amountString;
        set
        {
            if(amountString == value)
            {
                return;
            }

            amountString = value;
            OnPropertyChanged();
        }
    }

    private async Task SaveAccountBaseAsync()
    {
        ContentDialog? openContentDialog = DialogServiceClass.GetOpenContentDialog();

        if(string.IsNullOrWhiteSpace(SelectedAccount.Name))
        {
            DialogServiceClass.HideContentDialog(openContentDialog);
            await DialogService.ShowMessageAsync(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
            await DialogServiceClass.ShowContentDialog(openContentDialog);

            return;
        }

        bool nameChanged = SelectedAccount.Id == 0
                           || !SelectedAccount.Name.Equals(
                               await Mediator.Send(new GetAccountNameByIdQuery(SelectedAccount.Id)));

        if(nameChanged
           && await Mediator.Send(new GetIfAccountWithNameExistsQuery(SelectedAccount.Name, SelectedAccount.Id)))
        {
            DialogServiceClass.HideContentDialog(openContentDialog);
            if(!await DialogService.ShowConfirmMessageAsync(Strings.DuplicatedNameTitle,
                   Strings.DuplicateAccountMessage))
            {
                await DialogServiceClass.ShowContentDialog(openContentDialog);
                return;
            }
        }

        if(decimal.TryParse(AmountString, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal convertedValue))
        {
            SelectedAccount.CurrentBalance = convertedValue;
        }
        else
        {
            DialogServiceClass.HideContentDialog(openContentDialog);
            await DialogService.ShowMessageAsync(
                Strings.InvalidNumberTitle,
                Strings.InvalidNumberCurrentBalanceMessage);
            await DialogServiceClass.ShowContentDialog(openContentDialog);
            return;
        }

        await SaveAccountAsync();
        Messenger.Send(new ReloadMessage());
    }
}
