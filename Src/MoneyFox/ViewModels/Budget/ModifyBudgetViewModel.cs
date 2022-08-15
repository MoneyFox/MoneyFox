namespace MoneyFox.ViewModels.Budget
{

    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
    using Core.Common.Messages;
    using Core.Interfaces;
    using Views.Categories;

    internal abstract class ModifyBudgetViewModel : BaseViewModel, IRecipient<CategorySelectedMessage>
    {
        private readonly INavigationService navigationService;
        private BudgetViewModel selectedBudget = new BudgetViewModel();

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

        public ICollection TimeRangeCollection => new List<BudgetTimeRange>
        {
            BudgetTimeRange.YearToDate,
            BudgetTimeRange.Last1Year,
            BudgetTimeRange.Last2Years,
            BudgetTimeRange.Last3Years,
            BudgetTimeRange.Last5Years,
        };

        // Todo: use ReadOnly Collection?
        public ObservableCollection<BudgetCategoryViewModel> SelectedCategories { get; set; } = new ObservableCollection<BudgetCategoryViewModel>();

        public AsyncRelayCommand OpenCategorySelectionCommand => new AsyncRelayCommand(OpenCategorySelection);

        public RelayCommand<BudgetCategoryViewModel> RemoveCategoryCommand => new RelayCommand<BudgetCategoryViewModel>(RemoveCategory);

        public AsyncRelayCommand SaveBudgetCommand => new AsyncRelayCommand(SaveBudgetAsync);

        public void Receive(CategorySelectedMessage message)
        {
            var categorySelectedDataSet = message.Value;
            if (SelectedCategories.Any(c => c.CategoryId == message.Value.CategoryId) is false)
            {
                SelectedCategories.Add(new BudgetCategoryViewModel(categoryId: categorySelectedDataSet.CategoryId, name: categorySelectedDataSet.Name));
            }
        }

        private async Task OpenCategorySelection()
        {
            await navigationService.OpenModal<SelectCategoryPage>();
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
