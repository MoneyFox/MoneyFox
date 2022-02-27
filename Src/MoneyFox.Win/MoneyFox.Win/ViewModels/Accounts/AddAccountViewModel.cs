namespace MoneyFox.Win.ViewModels.Accounts;

using Core._Pending_.Common.Interfaces;
using Core.Commands.Accounts.CreateAccount;
using Core.Queries.Accounts.GetIfAccountWithNameExists;
using Core.Resources;
using MediatR;
using Services;
using System.Threading.Tasks;
using Utilities;

public class AddAccountViewModel : ModifyAccountViewModel
{
    private readonly IMediator mediator;

    public AddAccountViewModel(
        IMediator mediator,
        IDialogService dialogService,
        INavigationService navigationService) : base(dialogService, navigationService)
    {
        this.mediator = mediator;

        Title = Strings.AddAccountTitle;
    }

    protected override Task InitializeAsync()
    {
        SelectedAccount = new AccountViewModel();
        AmountString = HelperFunctions.FormatLargeNumbers(SelectedAccount.CurrentBalance);

        return Task.CompletedTask;
    }

    protected override async Task SaveAccountAsync()
    {
        if(await mediator.Send(new GetIfAccountWithNameExistsQuery(SelectedAccount.Name)))
        {
            await DialogService.ShowMessageAsync(Strings.DuplicatedNameTitle, Strings.DuplicateAccountMessage);
            return;
        }

        await mediator.Send(
            new CreateAccountCommand(
                SelectedAccount.Name,
                SelectedAccount.CurrentBalance,
                SelectedAccount.Note,
                SelectedAccount.IsExcluded));
    }
}