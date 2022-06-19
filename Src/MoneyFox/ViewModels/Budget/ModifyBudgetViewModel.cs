namespace MoneyFox.ViewModels.Budget
{

    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Core.Common.Messages;
    using Core.Interfaces;
    using Views.Categories;

    internal abstract class ModifyBudgetViewModel : BaseViewModel, IRecipient<CategorySelectedMessage>
    {
        private BudgetViewModel selectedBudget = new BudgetViewModel();

        private readonly INavigationService navigationService;

        protected ModifyBudgetViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            WeakReferenceMessenger.Default.Register(this);
        }

        public BudgetViewModel SelectedBudget
        {
            get => selectedBudget;
            private set => SetProperty(field: ref selectedBudget, newValue: value);
        }

        // Todo: use ReadOnly Collection?
        public ObservableCollection<BudgetCategoryViewModel> SelectedCategories { get; set; } = new ObservableCollection<BudgetCategoryViewModel>();

        public AsyncRelayCommand OpenCategorySelectionCommand => new AsyncRelayCommand(OpenCategorySelection);

        public RelayCommand<BudgetCategoryViewModel> RemoveCategoryCommand => new RelayCommand<BudgetCategoryViewModel>(RemoveCategory);

        public AsyncRelayCommand SaveBudgetCommand => new AsyncRelayCommand(SaveBudgetAsync);

        private async Task OpenCategorySelection()
        {
            await navigationService.OpenModal<SelectCategoryPage>();
        }

        public void Receive(CategorySelectedMessage message)
        {
            var categorySelectedDataSet = message.Value;
            if (SelectedCategories.Any(c => c.CategoryId == message.Value.CategoryId) is false)
            {
                SelectedCategories.Add(new BudgetCategoryViewModel(categoryId: categorySelectedDataSet.CategoryId, name: categorySelectedDataSet.Name));
            }
        }

        private void RemoveCategory(BudgetCategoryViewModel? budgetCategory)
        {
            if (budgetCategory == null)
            {
                return;
            }
            
            SelectedCategories.Remove(budgetCategory);
        }

        protected abstract Task SaveBudgetAsync();
    }

}
