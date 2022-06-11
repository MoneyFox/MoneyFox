namespace MoneyFox.ViewModels.Budget
{

    using System.Threading.Tasks;
    using Core.Interfaces;
    using MediatR;

    public sealed class EditBudgetViewModel : ModifyBudgetViewModel
    {
        private ISender sender;

        public EditBudgetViewModel(ISender sender, INavigationService navigationService) : base(navigationService)
        {
            this.sender = sender;
        }

        protected override Task SaveBudgetAsync()
        {
            throw new System.NotImplementedException();
        }
    }

}
