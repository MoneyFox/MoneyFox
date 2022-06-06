namespace MoneyFox.ViewModels.Budget
{

    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using Core.ApplicationCore.Queries.BudgetListLoading;
    using Core.Common.Extensions;
    using Extensions;
    using MediatR;
    using Views.Accounts;
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
            Budgets.AddRange(budgetsListData.Select(bld => new BudgetViewModel { Name = bld.Name, SpendingLimit = bld.SpendingLimit, CurrentSpending = 0 }));
        }

        private static async Task GoToAddBudget()
        {
            await Shell.Current.GoToModalAsync(ViewModelLocator.AddBudgetRoute);
        }

        private static async Task EditBudgetAsync(BudgetViewModel? budgetViewModel)
        {
            if (budgetViewModel == null)
            {
                throw new ArgumentNullException(nameof(budgetViewModel));
            }

            await Shell.Current.Navigation.PushModalAsync(
                new NavigationPage(new EditAccountPage(budgetViewModel.Id)) { BarBackgroundColor = Color.Transparent });
        }
    }

}
