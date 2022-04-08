namespace MoneyFox.Win.ViewModels.Accounts;

using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using Core.Aggregates;
using Core.Commands.Accounts.DeleteAccountById;
using Core.Commands.Accounts.UpdateAccount;
using Core.Common.Interfaces;
using Core.Resources;
using MediatR;
using Services;
using System.Globalization;
using System.Threading.Tasks;
using Core.Queries;
using Utilities;

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

    public AsyncRelayCommand DeleteCommand => new(DeleteAccountAsync);

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