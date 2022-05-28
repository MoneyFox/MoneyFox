namespace MoneyFox.ViewModels.Budget
{

    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using Extensions;
    using MediatR;
    using Views.Accounts;
    using Xamarin.Forms;

    public sealed class BudgetListViewModel : ObservableRecipient
    {
        private readonly IMediator mediator;

        private ObservableCollection<BudgetViewModel> budgets = new ObservableCollection<BudgetViewModel>();

        public BudgetListViewModel(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public ObservableCollection<BudgetViewModel> Budgets
        {
            get => budgets;

            private set
            {
                budgets = value;
                OnPropertyChanged();
            }
        }

        public AsyncRelayCommand GoToAddBudgetCommand => new AsyncRelayCommand(GoToAddBudget);
        public AsyncRelayCommand<BudgetViewModel> EditBudgetCommand => new AsyncRelayCommand<BudgetViewModel>(EditBudgetAsync);

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
