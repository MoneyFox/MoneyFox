using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.QueryObject;
using MoneyFox.ServiceLayer.Services;
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
            IBackupService backupService,
            IDialogService dialogService,
            IMvxLogProvider logProvider, 
            IMvxNavigationService navigationService) : base(settingsFacade, backupService, logProvider, navigationService)
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
            if (await crudService.ReadManyNoTracked<AccountViewModel>().AnyWithName(SelectedAccount.Name))
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            await crudService.CreateAndSaveAsync(SelectedAccount, "ctor(4)");
            if (!crudService.IsValid)
            {
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudService.GetAllErrors());
            }

            await NavigationService.Close(this);
        }
    }
}
