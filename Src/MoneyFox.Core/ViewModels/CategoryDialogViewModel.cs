using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Foundation.Model;
using MoneyFox.Foundation.Resources;
using MoneyManager.Foundation.Interfaces;

namespace MoneyFox.Core.ViewModels
{
    public class CategoryDialogViewModel : ViewModelBase
    {
        private readonly IRepository<Category> categoryRepository;
        private readonly IDialogService dialogService;

        public CategoryDialogViewModel(IRepository<Category> categoryRepository, IDialogService dialogService,
            CategoryListViewModel categoryListViewModel)
        {
            this.categoryRepository = categoryRepository;
            this.dialogService = dialogService;
        }

        public Category Selected { get; set; }

        public bool IsEdit { get; set; }

        public RelayCommand LoadedCommand => new RelayCommand(Loaded);

        public RelayCommand DoneCommand => new RelayCommand(Done);

        public string Title => IsEdit ? Strings.EditCategoryTitle : Strings.AddCategoryTitle;

        private void Loaded()
        {
            if (IsEdit) return;

            Selected = new Category();
        }

        private async void Done()
        {
            if (string.IsNullOrEmpty(Selected.Name))
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            categoryRepository.Save(Selected);
        }
    }
}