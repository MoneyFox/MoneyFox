using System;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Parameters;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class EditAccountViewModel : ModifyAccountViewModel
    {
        private readonly ICrudServicesAsync crudServices;
        private readonly ISettingsFacade settingsFacade;
        private readonly IBackupManager backupManager;
        private readonly IDialogService dialogService;

        public EditAccountViewModel(ICrudServicesAsync crudServices,
            ISettingsFacade settingsFacade,
            IBackupManager backupManager,
            IDialogService dialogService,
            IMvxLogProvider logProvider, 
            IMvxNavigationService navigationService) : base(crudServices, settingsFacade, backupManager, dialogService, logProvider, navigationService)
        {
            this.crudServices = crudServices;
            this.settingsFacade = settingsFacade;
            this.backupManager = backupManager;
            this.dialogService = dialogService;
        }

        public override string Title => string.Format(Strings.EditAccountTitle, SelectedAccount.Name);

        public override async void Prepare(ModifyAccountParameter parameter)
        {
            base.Prepare(parameter);
            SelectedAccount = await crudServices.ReadSingleAsync<AccountViewModel>(AccountId);
        }

        public MvxAsyncCommand DeleteCommand => new MvxAsyncCommand(DeleteAccount);

        protected override async Task SaveAccount()
        {
            await crudServices.UpdateAndSaveAsync(SelectedAccount, "ctor(4)");
            if (!crudServices.IsValid)
            {
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudServices.GetAllErrors());
            }

            await NavigationService.Close(this);
        }

        protected async Task DeleteAccount()
        {
            await crudServices.DeleteAndSaveAsync<AccountViewModel>(SelectedAccount.Id);
            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            await backupManager.EnqueueBackupTask();
        }
    }
}