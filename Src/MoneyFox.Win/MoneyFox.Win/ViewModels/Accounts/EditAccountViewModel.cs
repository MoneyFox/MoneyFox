namespace MoneyFox.Win.ViewModels.Accounts;

using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Queries;
using Core.Commands.Accounts.DeleteAccountById;
using Core.Commands.Accounts.UpdateAccount;
using Core.Common.Interfaces;
using Core.Resources;
using MediatR;
using Services;
using Utilities;

public class EditAccountViewModel : ModifyAccountViewModel
{
    private readonly IMapper mapper;

    public EditAccountViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService, INavigationService navigationService) : base(
        dialogService: dialogService,
        navigationService: navigationService,
        mediator: mediator)
    {
        this.mapper = mapper;
    }

    public override bool IsEdit => true;

    public AsyncRelayCommand DeleteCommand => new(DeleteAccountAsync);

    protected override async Task InitializeAsync()
    {
        SelectedAccount = mapper.Map<AccountViewModel>(await Mediator.Send(new GetAccountByIdQuery(AccountId)));
        AmountString = HelperFunctions.FormatLargeNumbers(SelectedAccount.CurrentBalance);
        Title = string.Format(provider: CultureInfo.InvariantCulture, format: Strings.EditAccountTitle, arg0: SelectedAccount.Name);
    }

    protected override async Task SaveAccountAsync()
    {
        await Mediator.Send(new UpdateAccountCommand(mapper.Map<Account>(SelectedAccount)));
    }

    protected async Task DeleteAccountAsync()
    {
        if (await DialogService.ShowConfirmMessageAsync(title: Strings.DeleteTitle, message: Strings.DeleteAccountConfirmationMessage))
        {
            await Mediator.Send(new DeactivateAccountByIdCommand(SelectedAccount.Id));
            NavigationService.GoBack();
        }
    }
}
