namespace MoneyFox.Win.ViewModels.Accounts;

using System.Threading.Tasks;
using Common.Helpers;
using Core.Commands.Accounts.CreateAccount;
using Core.Common.Interfaces;
using Core.Resources;
using MediatR;
using Services;

internal sealed class AddAccountViewModel : ModifyAccountViewModel
{
    public AddAccountViewModel(IMediator mediator, IDialogService dialogService, INavigationService navigationService) : base(
        dialogService: dialogService,
        navigationService: navigationService,
        mediator: mediator)
    {
        Title = Strings.AddAccountTitle;
    }

    protected override Task InitializeAsync()
    {
        SelectedAccount = new();
        AmountString = HelperFunctions.FormatLargeNumbers(SelectedAccount.CurrentBalance);

        return Task.CompletedTask;
    }

    protected override async Task SaveAccountAsync()
    {
        await Mediator.Send(
            new CreateAccountCommand(
                name: SelectedAccount.Name,
                currentBalance: SelectedAccount.CurrentBalance,
                note: SelectedAccount.Note,
                isExcluded: SelectedAccount.IsExcluded));
    }
}
