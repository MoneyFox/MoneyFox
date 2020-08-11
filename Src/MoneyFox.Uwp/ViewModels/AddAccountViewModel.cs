using AutoMapper;
using MediatR;
using MoneyFox.Application.Accounts.Commands.CreateAccount;
using MoneyFox.Application.Accounts.Queries.GetIfAccountWithNameExists;
using MoneyFox.Uwp.Src;
using MoneyFox.Application.Resources;
using MoneyFox.Domain.Entities;
using MoneyFox.Ui.Shared.Utilities;
using MoneyFox.Uwp.Services;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.ViewModels
{
    public class AddAccountViewModel : ModifyAccountViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public AddAccountViewModel(IMediator mediator,
                                   IMapper mapper,
                                   IDialogService dialogService,
                                   INavigationService navigationService) : base(dialogService, navigationService)
        {
            this.mediator = mediator;
            this.mapper = mapper;

            Title = Strings.AddAccountTitle;
        }

        protected override Task Initialize()
        {
            SelectedAccount = new AccountViewModel();
            AmountString = HelperFunctions.FormatLargeNumbers(SelectedAccount.CurrentBalance);

            return Task.CompletedTask;
        }

        protected override async Task SaveAccount()
        {
            if(await mediator.Send(new GetIfAccountWithNameExistsQuery(SelectedAccount.Name)))
            {
                await DialogService.ShowMessageAsync(Strings.DuplicatedNameTitle, Strings.DuplicateAccountMessage);
                return;
            }

            await mediator.Send(new CreateAccountCommand { AccountToSave = mapper.Map<Account>(SelectedAccount) });
            NavigationService.GoBack();
        }
    }
}
