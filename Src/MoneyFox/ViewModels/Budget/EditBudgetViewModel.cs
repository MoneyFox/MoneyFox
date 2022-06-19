namespace MoneyFox.ViewModels.Budget
{

    using System.Linq;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.Input;
    using Core.ApplicationCore.UseCases.BudgetUpdate;
    using Core.Interfaces;
    using MediatR;

    internal sealed class EditBudgetViewModel : ModifyBudgetViewModel
    {
        private readonly ISender sender;
        private readonly INavigationService navigationService;

        public EditBudgetViewModel(ISender sender, INavigationService navigationService) : base(navigationService)
        {
            this.sender = sender;
            this.navigationService = navigationService;
        }

        public AsyncRelayCommand DeleteBudgetCommand => new AsyncRelayCommand(DeleteBudgetAsync);

        private Task DeleteBudgetAsync()
        {
            throw new System.NotImplementedException();
        }

        protected override async Task SaveBudgetAsync()
        {
            var query = new UpdateBudget.Command(
                budgetId: SelectedBudget.Id,
                name: SelectedBudget.Name,
                spendingLimit: SelectedBudget.SpendingLimit,
                categories: SelectedCategories.Select(sc => sc.CategoryId).ToList());

            await sender.Send(query);
            await navigationService.GoBackFromModal();
        }
    }

}
