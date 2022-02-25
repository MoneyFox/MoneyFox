using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Aggregates;
using MoneyFox.Core.Commands.Accounts.DeleteAccountById;
using MoneyFox.Core.Commands.Accounts.UpdateAccount;
using MoneyFox.Core.Queries.Accounts.GetAccountById;
using MoneyFox.Core.Resources;
using MoneyFox.Win.Services;
using MoneyFox.Win.Utilities;
using System.Globalization;
using System.Threading.Tasks;

namespace MoneyFox.Win.ViewModels.Accounts
{
    public class EditAccountViewModel : ModifyAccountViewModel
    {
        private readonly IMapper mapper;

        public EditAccountViewModel(
            IMediator mediator,
            IMapper mapper,
            IDialogService dialogService,
            INavigationService navigationService) : base(dialogService, navigationService, mediator)
        {
            this.mapper = mapper;
        }

        public override bool IsEdit => true;

        public AsyncRelayCommand DeleteCommand => new AsyncRelayCommand(DeleteAccountAsync);

        protected override async Task InitializeAsync()
        {
            SelectedAccount = mapper.Map<AccountViewModel>(await Mediator.Send(new GetAccountByIdQuery(AccountId)));
            AmountString = HelperFunctions.FormatLargeNumbers(SelectedAccount.CurrentBalance);
            Title = string.Format(CultureInfo.InvariantCulture, Strings.EditAccountTitle, SelectedAccount.Name);
        }

        protected override async Task SaveAccountAsync() =>
            await Mediator.Send(new UpdateAccountCommand(mapper.Map<Account>(SelectedAccount)));

        protected async Task DeleteAccountAsync()
        {
            if(await DialogService.ShowConfirmMessageAsync(
                   Strings.DeleteTitle,
                   Strings.DeleteAccountConfirmationMessage))
            {
                await Mediator.Send(new DeactivateAccountByIdCommand(SelectedAccount.Id));
                NavigationService.GoBack();
            }
        }
    }
}