namespace MoneyFox.ViewModels.Budget
{

    using System.Linq;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.Messaging;
    using Core.ApplicationCore.UseCases.BudgetCreation;
    using Core.Common.Messages;
    using Core.Interfaces;
    using MediatR;

    internal sealed class AddBudgetViewModel : ModifyBudgetViewModel
    {
        private readonly ISender sender;
        private readonly INavigationService navigationService;

        public AddBudgetViewModel(ISender sender, INavigationService navigationService) : base(navigationService: navigationService)
        {
            this.sender = sender;
            this.navigationService = navigationService;
        }

        protected override async Task SaveBudgetAsync()
        {
            var query = new CreateBudget.Command(
                name: SelectedBudget.Name,
                spendingLimit: SelectedBudget.SpendingLimit,
                categories: SelectedCategories.Select(sc => sc.CategoryId).ToList());

            await sender.Send(query);
            Messenger.Send(new ReloadMessage());
            await navigationService.GoBackFromModal();
        }
    }

}
