namespace MoneyFox.Win.ViewModels.Accounts;

using Core.Commands.Accounts.CreateAccount;
using Core.Common.Interfaces;
using Core.Resources;
using MediatR;
using Services;
using System.Threading.Tasks;
using Utilities;

public class AddAccountViewModel : ModifyAccountViewModel
{
    public AddAccountViewModel(
        IMediator mediator,
        IDialogService dialogService,
        INavigationService navigationService) : base(dialogService, navigationService, mediator)
    {
        Title = Strings.AddAccountTitle;
    }

    protected override Task InitializeAsync()
    {
        SelectedAccount = new AccountViewModel();
        AmountString = HelperFunctions.FormatLargeNumbers(SelectedAccount.CurrentBalance);

        return Task.CompletedTask;
    }

    protected override async Task SaveAccountAsync() =>
        await Mediator.Send(
            new CreateAccountCommand(
                SelectedAccount.Name,
                SelectedAccount.CurrentBalance,
                SelectedAccount.Note,
                SelectedAccount.IsExcluded));
}