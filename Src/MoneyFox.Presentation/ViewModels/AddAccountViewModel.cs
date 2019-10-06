using System.Threading.Tasks;
using AutoMapper;
using GalaSoft.MvvmLight.Views;
using MediatR;
using MoneyFox.Application.Accounts.Commands.CreateAccount;
using MoneyFox.Application.Accounts.Queries.GetIfAccountWithNameExists;
using MoneyFox.Application.Resources;
using MoneyFox.Domain.Entities;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Services;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
{
    public class AddAccountViewModel : ModifyAccountViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IDialogService dialogService;

        public AddAccountViewModel(IMediator mediator,
                                   IMapper mapper,
                                   ISettingsFacade settingsFacade,
                                   IBackupService backupService,
                                   IDialogService dialogService,
                                   INavigationService navigationService)
            : base(settingsFacade, backupService, navigationService)
        {
            this.mediator = mediator;
            this.dialogService = dialogService;
            this.mapper = mapper;

            Title = Strings.AddAccountTitle;
        }

        protected override Task Initialize()
        {
            SelectedAccount = new AccountViewModel();
            return Task.CompletedTask;
        }

        protected override async Task SaveAccount()
        {
            if (await mediator.Send(new GetIfAccountWithNameExistsQuery { AccountName = SelectedAccount.Name}))
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            await mediator.Send(new CreateAccountCommand {AccountToSave = mapper.Map<Account>(SelectedAccount)});
            NavigationService.GoBack();
        }
    }
}
