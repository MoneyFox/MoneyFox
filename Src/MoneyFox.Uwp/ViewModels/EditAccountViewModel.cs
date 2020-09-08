using AutoMapper;
using MediatR;
using MoneyFox.Application.Accounts.Commands.DeleteAccountById;
using MoneyFox.Application.Accounts.Commands.UpdateAccount;
using MoneyFox.Application.Accounts.Queries.GetAccountById;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Domain.Entities;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Utilities;
using MoneyFox.Ui.Shared.ViewModels.Accounts;
using MoneyFox.Uwp.Services;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.ViewModels
{
    public class EditAccountViewModel : ModifyAccountViewModel
    {
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public EditAccountViewModel(IMediator mediator,
                                    IMapper mapper,
                                    IDialogService dialogService,
                                    INavigationService navigationService) : base(dialogService, navigationService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public AsyncCommand DeleteCommand => new AsyncCommand(DeleteAccount);

        protected override async Task Initialize()
        {
            SelectedAccount = mapper.Map<AccountViewModel>(await mediator.Send(new GetAccountByIdQuery(AccountId)));
            AmountString = HelperFunctions.FormatLargeNumbers(SelectedAccount.CurrentBalance);
            Title = string.Format(CultureInfo.InvariantCulture, Strings.EditAccountTitle, SelectedAccount.Name);
        }

        protected override async Task SaveAccount()
        {
            await mediator.Send(new UpdateAccountCommand(mapper.Map<Account>(SelectedAccount)));
            await Window.CloseAsync();
        }

        protected async Task DeleteAccount()
        {
            if(await DialogService.ShowConfirmMessageAsync(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                await mediator.Send(new DeleteAccountByIdCommand(SelectedAccount.Id));
                await Window.CloseAsync();
            }
        }
    }
}
