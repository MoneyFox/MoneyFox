using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;

namespace MoneyManager.Core.ViewModels
{
    public class CategoryDialogViewModel : BaseViewModel
    {
        private readonly CategoryListViewModel categoryListView;
        private readonly IRepository<Category> categoryRepository;
        private readonly IDialogService dialogService;

        public CategoryDialogViewModel(IRepository<Category> categoryRepository, IDialogService dialogService,
            CategoryListViewModel categoryListView)
        {
            this.categoryRepository = categoryRepository;
            this.dialogService = dialogService;
            this.categoryListView = categoryListView;
        }

        public Category Selected { get; set; }

        public bool IsEdit { get; set; }

        public IMvxCommand LoadedCommand => new MvxCommand(Loaded);

        public IMvxCommand DoneCommand => new MvxCommand(Done);

        private void Loaded()
        {
            if (IsEdit) return;

            Selected = new Category();

            if (!string.IsNullOrEmpty(categoryListView.SearchText))
            {
                Selected.Name = categoryListView.SearchText;
            }
        }

        private async void Done()
        {
            if (string.IsNullOrEmpty(Selected.Name))
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptryTitle, Strings.NameRequiredMessage);
                return;
            }


            categoryRepository.Save(Selected);
            categoryListView.SearchText = string.Empty;
            categoryListView.Search();
        }
    }
}