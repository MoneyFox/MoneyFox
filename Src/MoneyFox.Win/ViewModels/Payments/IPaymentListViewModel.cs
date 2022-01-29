using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Data;
using MoneyFox.Win.ViewModels.Interfaces;

namespace MoneyFox.Win.ViewModels.Payments
{
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
}