using System.Threading.Tasks;
using GenericServices;
using MoneyFox.BusinessLogic.Backup;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class AddAccountViewModel : ModifyAccountViewModel
    {
        private readonly ICrudServicesAsync crudService;
        private readonly IDialogService dialogService;

        public AddAccountViewModel(ICrudServicesAsync crudService,
            ISettingsFacade settingsFacade,
            IBackupManager backupManager,
            IDialogService dialogService,
            IMvxLogProvider logProvider, 
            IMvxNavigationService navigationService) : base(crudService, settingsFacade, backupManager, dialogService, logProvider, navigationService)
        {
            this.crudService = crudService;
            this.dialogService = dialogService;
        }

        public override void Prepare()
        {
            SelectedAccount = new AccountViewModel();
            base.Prepare();
        }

        protected override async Task SaveAccount()
        {
            await crudService.CreateAndSaveAsync(SelectedAccount, "ctor(4)");
            if (!crudService.IsValid)
            {
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudService.GetAllErrors());
            }

            await NavigationService.Close(this);
        }
    }
}
