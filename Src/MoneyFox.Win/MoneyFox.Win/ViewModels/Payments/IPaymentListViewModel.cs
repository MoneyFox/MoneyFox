namespace MoneyFox.Win.ViewModels.Payments;

using CommunityToolkit.Mvvm.Input;
using Interfaces;
using Microsoft.UI.Xaml.Data;

public interface IPaymentListViewModel
{
    IBalanceViewModel BalanceViewModel { get; }

    IPaymentListViewActionViewModel ViewActionViewModel { get; }

    RelayCommand InitializeCommand { get; }

    RelayCommand LoadDataCommand { get; }

    CollectionViewSource GroupedPayments { get; }

    string Title { get; }

    int AccountId { get; }
}