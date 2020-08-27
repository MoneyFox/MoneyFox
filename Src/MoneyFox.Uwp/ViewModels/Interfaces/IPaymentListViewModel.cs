using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml.Data;

namespace MoneyFox.Uwp.ViewModels.Interfaces
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
