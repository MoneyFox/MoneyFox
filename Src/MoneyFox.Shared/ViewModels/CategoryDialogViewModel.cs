using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Shared.ViewModels
{
    public class CategoryDialogViewModel : BaseViewModel
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

        public IMvxCommand LoadedCommand => new MvxCommand(Loaded);

        public IMvxCommand DoneCommand => new MvxCommand(Done);

        public string Title => IsEdit ? Strings.EditCategoryTitle : Strings.AddCategoryTitle;

        private void Loaded()
        {
            if (IsEdit)
            {
                return;
            }

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