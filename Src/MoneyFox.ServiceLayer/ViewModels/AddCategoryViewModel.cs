using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Services;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class AddCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly ICrudServicesAsync crudServices;
        private readonly IDialogService dialogService;

        public AddCategoryViewModel(ICrudServicesAsync crudServices,
            IDialogService dialogService,
            ISettingsFacade settingsFacade,
            IBackupService backupService,
            IMvxLogProvider logProvider,
            IMvxNavigationService navigationService) 
                : base(crudServices, dialogService, settingsFacade, backupService, logProvider, navigationService)
        {
            this.crudServices = crudServices;
            this.dialogService = dialogService;
        }

        public override void Prepare()
        {
            SelectedCategory = new CategoryViewModel();
        }

        protected override async Task SaveCategory()
        {
            await crudServices.CreateAndSaveAsync(SelectedCategory, "ctor(2)");
            if (!crudServices.IsValid)
            {
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudServices.GetAllErrors());
            }

            await NavigationService.Close(this);
        }
    }
}