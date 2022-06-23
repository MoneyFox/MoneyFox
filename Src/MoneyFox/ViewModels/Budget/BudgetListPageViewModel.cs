namespace MoneyFox.ViewModels.Budget
{

    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using CommunityToolkit.Mvvm.Input;
    using Core.ApplicationCore.Queries.BudgetListLoading;
    using Core.Common.Extensions;
    using MediatR;
    using Views.Budget;
    using Xamarin.Forms;

    internal sealed class BudgetListPageViewModel : BaseViewModel
    {
        private readonly ISender sender;

        public BudgetListPageViewModel(ISender sender)
        {
            this.sender = sender;
        }

        public ObservableCollection<BudgetListViewModel> Budgets { get; } = new ObservableCollection<BudgetListViewModel>();

        public AsyncRelayCommand InitializeCommand => new AsyncRelayCommand(Initialize);

        public AsyncRelayCommand GoToAddBudgetCommand => new AsyncRelayCommand(GoToAddBudget);

        public AsyncRelayCommand<BudgetListViewModel> EditBudgetCommand => new AsyncRelayCommand<BudgetListViewModel>(EditBudgetAsync);

        private async Task Initialize()
        {
            var budgetsListData = await sender.Send(new LoadBudgets.Query());
            Budgets.Clear();
            Budgets.AddRange(
                budgetsListData.Select(
                    bld => new BudgetListViewModel
                    {
                        Id = bld.Id,
                        Name = bld.Name,
                        SpendingLimit = bld.SpendingLimit,
                        CurrentSpending = bld.CurrentSpending
                    }));
        }

        private static async Task GoToAddBudget()
        {
            await Shell.Current.GoToModalAsync(ViewModelLocator.AddBudgetRoute);
        }

        private async Task EditBudgetAsync(BudgetListViewModel? selectedBudget)
        {
            if (selectedBudget == null)
            {
                return;
            }

            await Shell.Current.Navigation.PushModalAsync(new NavigationPage(new EditBudgetPage(selectedBudget.Id)) { BarBackgroundColor = Color.Transparent });
        }
    }

}
