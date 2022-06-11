namespace MoneyFox.ViewModels.Budget
{

    using System.Linq;
    using System.Threading.Tasks;
    using Core.ApplicationCore.UseCases.BudgetUpdate;
    using Core.Interfaces;
    using MediatR;

    public sealed class EditBudgetViewModel : ModifyBudgetViewModel
    {
        private readonly ISender sender;
        private readonly INavigationService navigationService;

        public EditBudgetViewModel(ISender sender, INavigationService navigationService) : base(navigationService)
        {
            this.sender = sender;
            this.navigationService = navigationService;
        }

        protected override async Task SaveBudgetAsync()
        {
            var query = new UpdateBudget.Command(
                name: SelectedBudget.Name,
                spendingLimit: SelectedBudget.SpendingLimit,
                categories: SelectedCategories.Select(sc => sc.CategoryId).ToList());

            await sender.Send(query);
            await navigationService.GoBackFromModal();
        }
    }

}
