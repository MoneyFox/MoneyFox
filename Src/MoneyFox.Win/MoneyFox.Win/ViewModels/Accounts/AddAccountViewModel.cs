using MediatR;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Commands.Accounts.CreateAccount;
using MoneyFox.Core.Queries.Accounts.GetIfAccountWithNameExists;
using MoneyFox.Core.Resources;
using MoneyFox.Win.Services;
using MoneyFox.Win.Utilities;
using System.Threading.Tasks;

namespace MoneyFox.Win.ViewModels.Accounts
{
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
}