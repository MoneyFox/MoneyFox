namespace MoneyFox.Ui.Views.Budget.BudgetOverview;

using System.Collections.ObjectModel;
using BudgetModification;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Settings;
using Core.Queries;
using MediatR;

public sealed class BudgetOverviewViewModel(ISender sender, ISettingsFacade settingsFacade, INavigationService navigationService) : NavigableViewModel
{
    private int budgetId;

    public AsyncRelayCommand GoToEditCommand => new(() => navigationService.GoTo<EditBudgetViewModel>(budgetId));

    public ObservableCollection<PaymentDayGroup> PaymentsGroups { get; private set; } = [];

    public override Task OnNavigatedAsync(object? parameter)
    {
        budgetId = Convert.ToInt32(parameter);

        return LoadData();
    }

    public override Task OnNavigatedBackAsync(object? parameter)
    {
        return LoadData();
    }

    private async Task LoadData()
    {
        var currency = settingsFacade.DefaultCurrency;
        var paymentData = await sender.Send(new GetPaymentsInBudget.Query(new(budgetId)));
        var viewModels = paymentData.Select(
            p => new BudgetPaymentViewModel
            {
                AccountName = p.Account,
                Date = DateOnly.FromDateTime(p.Date),
                Amount = new(amount: p.Amount, currencyAlphaIsoCode: currency),
                Category = p.Category,
                IsCleared = p.IsCleared,
                IsRecurring = p.IsRecurring
            });

        PaymentsGroups = new(viewModels.GroupBy(pd => pd.Date).Select(g => new PaymentDayGroup(date: g.Key, payments: g.ToList())));
        OnPropertyChanged(nameof(PaymentsGroups));
    }
}
