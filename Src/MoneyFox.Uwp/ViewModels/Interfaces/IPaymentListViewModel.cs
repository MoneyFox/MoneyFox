using GalaSoft.MvvmLight.Command;
using MoneyFox.Ui.Shared.Groups;
using System.Collections.ObjectModel;

namespace MoneyFox.Uwp.ViewModels.Interfaces
{
    public interface IPaymentListViewModel
    {
        IBalanceViewModel BalanceViewModel { get; }

        IPaymentListViewActionViewModel ViewActionViewModel { get; }

        RelayCommand InitializeCommand { get; }

        RelayCommand LoadDataCommand { get; }

        ObservableCollection<DateListGroupCollection<PaymentViewModel>> GroupedPayments { get; }

        string Title { get; }

        int AccountId { get; }
    }
}
