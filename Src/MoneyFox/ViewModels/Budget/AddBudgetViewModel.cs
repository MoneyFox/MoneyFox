namespace MoneyFox.ViewModels.Budget
{

    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Core.ApplicationCore.UseCases.BudgetCreation;
    using Core.Common.Messages;
    using Core.Interfaces;
    using MediatR;

    public sealed class AddBudgetViewModel : ObservableRecipient, IRecipient<CategorySelectedMessage>
    {
        private BudgetViewModel selectedBudget = new BudgetViewModel();

        private readonly ISender sender;
        private readonly INavigationService navigationService;

        public AddBudgetViewModel(ISender sender, INavigationService navigationService)
        {
            this.sender = sender;
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

        private void RemoveCategory(BudgetCategoryViewModel budgetCategory)
        {
            SelectedCategories.Remove(budgetCategory);
        }

        private async Task SaveBudgetAsync()
        {
            var query = new CreateBudget.Query(
                name: SelectedBudget.Name,
                spendingLimit: SelectedBudget.SpendingLimit,
                categories: SelectedCategories.Select(sc => sc.CategoryId).ToList());

            await sender.Send(query);
            await navigationService.GoBackFromModal();
        }
    }

}
