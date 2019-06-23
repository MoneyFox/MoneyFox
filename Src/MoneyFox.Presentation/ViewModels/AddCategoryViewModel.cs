using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.QueryObject;
using MoneyFox.Presentation.Services;
using MoneyFox.ServiceLayer.Facades;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
{
    public class AddCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly ICrudServicesAsync crudServices;
        private readonly IDialogService dialogService;

        public AddCategoryViewModel(ICrudServicesAsync crudServices,
            IDialogService dialogService,
            ISettingsFacade settingsFacade,
            IBackupService backupService,
            INavigationService navigationService) : base(crudServices, settingsFacade, backupService, navigationService)
        {
            this.crudServices = crudServices;
            this.dialogService = dialogService;

            SelectedCategory = new CategoryViewModel();
            Title = Strings.AddCategoryTitle;
        }

        protected override async Task SaveCategory()
        {
            if (string.IsNullOrEmpty(SelectedCategory.Name))
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            if (await crudServices.ReadManyNoTracked<CategoryViewModel>().AnyWithNameAsync(SelectedCategory.Name))
            {
                await dialogService.ShowMessage(Strings.DuplicatedNameTitle, Strings.DuplicateCategoryMessage);
                return;
            }

            await crudServices.CreateAndSaveAsync(SelectedCategory, "ctor(2)");

            if (!crudServices.IsValid)
            {
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudServices.GetAllErrors());
            }

            NavigationService.GoBack();
        }
    }
}