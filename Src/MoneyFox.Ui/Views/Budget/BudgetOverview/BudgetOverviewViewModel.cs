namespace MoneyFox.Ui.Views.Budget.BudgetOverview;

using System.Collections.ObjectModel;
using BudgetModification;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Settings;
using Core.Queries;
using MediatR;
using Payments.PaymentModification;

public sealed class BudgetOverviewViewModel(ISender sender, ISettingsFacade settingsFacade, INavigationService navigationService) : NavigableViewModel
{
    private int budgetId;
    private string budgetName = string.Empty;
    private ObservableCollection<PaymentDayGroup> paymentsGroups = [];

    public AsyncRelayCommand GoToEditCommand => new(() => navigationService.GoTo<EditBudgetViewModel>(budgetId));
    public AsyncRelayCommand<BudgetPaymentViewModel> GoToEditPaymentCommand => new((vm) => navigationService.GoTo<EditPaymentViewModel>(vm!.Id));

    public ObservableCollection<PaymentDayGroup> PaymentsGroups
    {
        get => paymentsGroups;

        private set => SetProperty(field: ref paymentsGroups, newValue: value);
    }

    public string BudgetName
    {
        get => budgetName;
        private set => SetProperty(field: ref budgetName, newValue: value);
    }

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
        BudgetName = await sender.Send(new GetBudgetNameById.Query(new(budgetId)));
        var currency = settingsFacade.DefaultCurrency;
        var paymentData = await sender.Send(new GetPaymentsInBudget.Query(new(budgetId)));
        var viewModels = paymentData.OrderByDescending(d => d.Date)
            .Select(
                p => new BudgetPaymentViewModel
                {
                    Id = p.PaymentId,
                    Account = p.Account,
                    Date = DateOnly.FromDateTime(p.Date),
                    Amount = new(amount: p.Amount, currencyAlphaIsoCode: currency),
                    Category = p.Category,
                    IsCleared = p.IsCleared,
                    IsRecurring = p.IsRecurring,
                    Note = p.Note
                });

        PaymentsGroups = new(viewModels.GroupBy(pd => pd.Date).Select(g => new PaymentDayGroup(date: g.Key, payments: g.ToList())));
    }
}
