using System.Reactive.Disposables;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.QueryObject;
using MoneyFox.ServiceLayer.Services;
using ReactiveUI;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class AddCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly ICrudServicesAsync crudServices;
        private readonly IDialogService dialogService;

        public AddCategoryViewModel(IScreen hostScreen,
                                    ICrudServicesAsync crudServices,
                                    IDialogService dialogService,
                                    ISettingsFacade settingsFacade,
                                    IBackupService backupService)
            : base(0, hostScreen, crudServices, settingsFacade, backupService, dialogService)
        {
            this.crudServices = crudServices;
            this.dialogService = dialogService;

            this.WhenActivated((CompositeDisposable disposable) =>
            {
                SelectedCategory = new CategoryViewModel();
                Title = Strings.AddCategoryTitle;
            });
        }

        public override string UrlPathSegment => "AddCategory";

        protected override async Task SaveCategory()
        {
            if (await crudServices.ReadManyNoTracked<AccountViewModel>().AnyWithNameAsync(SelectedCategory.Name))
            {
                await dialogService.ShowMessage(Strings.DuplicatedNameTitle, Strings.DuplicateCategoryMessage);
                return;
            }

            await crudServices.CreateAndSaveAsync(SelectedCategory, "ctor(2)");

            if (!crudServices.IsValid)
            {
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudServices.GetAllErrors());
            }

            HostScreen.Router.NavigateBack.Execute();
        }
    }
}