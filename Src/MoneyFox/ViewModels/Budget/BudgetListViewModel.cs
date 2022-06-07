namespace MoneyFox.ViewModels.Budget
{

    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using Core.ApplicationCore.Queries.BudgetListLoading;
    using Core.Common.Extensions;
    using Extensions;
    using MediatR;
    using Views.Budget;
    using Xamarin.Forms;

    public sealed class BudgetListViewModel : ObservableRecipient
    {
        private readonly ISender sender;

        public BudgetListViewModel(ISender sender)
        {
            this.sender = sender;
        }

        public ObservableCollection<BudgetViewModel> Budgets { get; } = new ObservableCollection<BudgetViewModel>();

        public AsyncRelayCommand InitializeCommand => new AsyncRelayCommand(Initialize);

        public AsyncRelayCommand GoToAddBudgetCommand => new AsyncRelayCommand(GoToAddBudget);

        public AsyncRelayCommand<BudgetViewModel> EditBudgetCommand => new AsyncRelayCommand<BudgetViewModel>(EditBudgetAsync);

        private async Task Initialize()
        {
            var budgetsListData = await sender.Send(new LoadBudgets.Query());
            Budgets.Clear();
            Budgets.AddRange(
                budgetsListData.Select(
                    bld => new BudgetViewModel
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

        private async Task EditBudgetAsync(BudgetViewModel selectedBudget)
        {
            if (selectedBudget == null)
            {
                return;
            }

            await Shell.Current.Navigation.PushModalAsync(new NavigationPage(new EditBudgetPage(selectedBudget.Id)) { BarBackgroundColor = Color.Transparent });
        }
    }

}
