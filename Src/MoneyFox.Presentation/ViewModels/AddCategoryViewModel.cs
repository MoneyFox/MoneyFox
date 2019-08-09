using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using MediatR;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Services;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
{
    public class AddCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly IMediator mediator;
        private readonly IDialogService dialogService;

        public AddCategoryViewModel(IMediator mediator,
            IDialogService dialogService,
            ISettingsFacade settingsFacade,
            IBackupService backupService,
            INavigationService navigationService) : base(mediator, settingsFacade, backupService, navigationService)
        {
            this.mediator = mediator;
            this.dialogService = dialogService;

            Title = Strings.AddCategoryTitle;
        }

        protected override Task Initialize()
        {
            SelectedCategory = new CategoryViewModel();
            return Task.CompletedTask;
        }

        protected override async Task SaveCategory()
        {
            if (string.IsNullOrEmpty(SelectedCategory.Name))
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            // TODO: Reimplement
            //if (await crudServices.ReadManyNoTracked<CategoryViewModel>().AnyWithNameAsync(SelectedCategory.Name))
            //{
            //    await dialogService.ShowMessage(Strings.DuplicatedNameTitle, Strings.DuplicateCategoryMessage);
            //    return;
            //}

            //await crudServices.CreateAndSaveAsync(SelectedCategory, "ctor(2)");

            //if (!crudServices.IsValid)
            //{
            //    await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudServices.GetAllErrors());
            //}

            NavigationService.GoBack();
        }
    }
}