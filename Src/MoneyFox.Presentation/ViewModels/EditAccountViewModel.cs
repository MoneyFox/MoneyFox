using System;
using System.Globalization;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Services;
using MoneyFox.Presentation.Utilities;
using MoneyFox.ServiceLayer.Facades;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
{
    public class EditAccountViewModel : ModifyAccountViewModel
    {
        private readonly ICrudServicesAsync crudServices;
        private readonly IBackupService backupService;
        private readonly IDialogService dialogService;
        private readonly ISettingsFacade settingsFacade;

        public EditAccountViewModel(ICrudServicesAsync crudServices,
            ISettingsFacade settingsFacade,
            IBackupService backupService,
            IDialogService dialogService,
            INavigationService navigationService) : base(settingsFacade, backupService, navigationService)
        {
            this.crudServices = crudServices;
            this.settingsFacade = settingsFacade;
            this.backupService = backupService;
            this.dialogService = dialogService;
        }

        public override string Title =>
            string.Format(CultureInfo.InvariantCulture, Strings.EditAccountTitle, SelectedAccount.Name);

        public AsyncCommand DeleteCommand => new AsyncCommand(DeleteAccount);

        public AsyncCommand InitializeCommand => new AsyncCommand(Initialize);

        public async Task Initialize()
        {
            SelectedAccount = await crudServices.ReadSingleAsync<AccountViewModel>(AccountId);
        }

        protected override async Task SaveAccount()
        {
            await crudServices.UpdateAndSaveAsync(SelectedAccount);

            if (!crudServices.IsValid)
            {
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudServices.GetAllErrors());
            }

            CancelCommand.Execute(null);
        }

        protected async Task DeleteAccount()
        {
            await crudServices.DeleteAndSaveAsync<AccountViewModel>(SelectedAccount.Id);

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            CancelCommand.Execute(null);
            backupService.EnqueueBackupTask().FireAndForgetSafeAsync();
        }
    }
}