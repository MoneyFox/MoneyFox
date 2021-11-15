using CommunityToolkit.Mvvm.Input;
using MoneyFox.Uwp.ViewModels.Interfaces;
using Windows.UI.Xaml.Data;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Payments
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