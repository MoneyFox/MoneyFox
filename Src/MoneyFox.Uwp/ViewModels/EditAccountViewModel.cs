using System;
using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MoneyFox.Application.Accounts.Commands.DeleteAccountById;
using MoneyFox.Application.Accounts.Commands.UpdateAccount;
using MoneyFox.Application.Accounts.Queries.GetAccountById;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Domain.Entities;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Utilities;
using MoneyFox.Uwp.Services;

namespace MoneyFox.Uwp.ViewModels
{
    public class EditAccountViewModel : ModifyAccountViewModel
    {
        private readonly IBackupService backupService;
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly ISettingsFacade settingsFacade;

        public EditAccountViewModel(IMediator mediator,
                                    IMapper mapper,
                                    ISettingsFacade settingsFacade,
                                    IBackupService backupService,
                                    IDialogService dialogService,
                                    INavigationService navigationService)
            : base(settingsFacade, backupService, dialogService, navigationService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.settingsFacade = settingsFacade;
            this.backupService = backupService;
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
            await mediator.Send(new UpdateAccountCommand {Account = mapper.Map<Account>(SelectedAccount)});
            CancelCommand.Execute(null);
        }

        protected async Task DeleteAccount()
        {
            if (await DialogService.ShowConfirmMessageAsync(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                await mediator.Send(new DeleteAccountByIdCommand(SelectedAccount.Id));

                settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
                NavigationService.GoBack();
                backupService.UploadBackupAsync().FireAndForgetSafeAsync();
            }
        }
    }
}
