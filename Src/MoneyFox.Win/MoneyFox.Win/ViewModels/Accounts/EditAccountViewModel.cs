namespace MoneyFox.Win.ViewModels.Accounts;

using CommunityToolkit.Mvvm.Input;
using Core._Pending_.Common.Interfaces;
using Core.Aggregates;
using Core.Commands.Accounts.DeleteAccountById;
using Core.Commands.Accounts.UpdateAccount;
using Core.Queries.Accounts.GetAccountById;
using Core.Resources;
using global::AutoMapper;
using MediatR;
using Services;
using System.Globalization;
using System.Threading.Tasks;
using Utilities;

public class EditAccountViewModel : ModifyAccountViewModel
{
    private readonly IMapper mapper;
    private readonly IMediator mediator;

    public EditAccountViewModel(
        IMediator mediator,
        IMapper mapper,
        IDialogService dialogService,
        INavigationService navigationService) : base(dialogService, navigationService)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    public override bool IsEdit => true;

    public AsyncRelayCommand DeleteCommand => new(DeleteAccountAsync);

    protected override async Task InitializeAsync()
    {
        SelectedAccount = mapper.Map<AccountViewModel>(await mediator.Send(new GetAccountByIdQuery(AccountId)));
        AmountString = HelperFunctions.FormatLargeNumbers(SelectedAccount.CurrentBalance);
        Title = string.Format(CultureInfo.InvariantCulture, Strings.EditAccountTitle, SelectedAccount.Name);
    }

    protected override async Task SaveAccountAsync() =>
        await mediator.Send(new UpdateAccountCommand(mapper.Map<Account>(SelectedAccount)));

    protected async Task DeleteAccountAsync()
    {
        if(await DialogService.ShowConfirmMessageAsync(
               Strings.DeleteTitle,
               Strings.DeleteAccountConfirmationMessage))
        {
            await mediator.Send(new DeactivateAccountByIdCommand(SelectedAccount.Id));
            NavigationService.GoBack();
        }
    }
}