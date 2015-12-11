using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.ViewModels.CategoryList;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;

namespace MoneyManager.Core.ViewModels.Dialogs
{
    public class CategoryDialogViewModel : BaseViewModel
    {
        private readonly SettingsCategoryListViewModel categoryListViewModel;
        private readonly IRepository<Category> categoryRepository;
        private readonly IDialogService dialogService;

        public CategoryDialogViewModel(IRepository<Category> categoryRepository, IDialogService dialogService,
            SettingsCategoryListViewModel categoryListViewModel)
        {
            this.categoryRepository = categoryRepository;
            this.dialogService = dialogService;
            this.categoryListViewModel = categoryListViewModel;
        }

        public Category Selected { get; set; }

        public bool IsEdit { get; set; }

        public IMvxCommand LoadedCommand => new MvxCommand(Loaded);

        public IMvxCommand DoneCommand => new MvxCommand(Done);

        private void Loaded()
        {
            if (IsEdit) return;

            Selected = new Category();

            if (!string.IsNullOrEmpty(categoryListViewModel.SearchText))
            {
                Selected.Name = categoryListViewModel.SearchText;
            }
        }

        private async void Done()
        {
            if (string.IsNullOrEmpty(Selected.Name))
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            categoryRepository.Save(Selected);
            categoryListViewModel.SearchText = string.Empty;
            categoryListViewModel.Search();
        }
    }
}