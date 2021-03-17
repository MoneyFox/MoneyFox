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

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Accounts
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

        public override bool IsEdit => true;

        public AsyncCommand DeleteCommand => new AsyncCommand(DeleteAccountAsync);

        protected override async Task InitializeAsync()
        {
            SelectedAccount = mapper.Map<AccountViewModel>(await mediator.Send(new GetAccountByIdQuery(AccountId)));
            AmountString = HelperFunctions.FormatLargeNumbers(SelectedAccount.CurrentBalance);
            Title = string.Format(CultureInfo.InvariantCulture, Strings.EditAccountTitle, SelectedAccount.Name);
        }

        protected override async Task SaveAccountAsync() => await mediator.Send(new UpdateAccountCommand(mapper.Map<Account>(SelectedAccount)));

        protected async Task DeleteAccountAsync()
        {
            if(await DialogService.ShowConfirmMessageAsync(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                await mediator.Send(new DeactivateAccountByIdCommand(SelectedAccount.Id));
                NavigationService.GoBack();
            }
        }
    }
}
