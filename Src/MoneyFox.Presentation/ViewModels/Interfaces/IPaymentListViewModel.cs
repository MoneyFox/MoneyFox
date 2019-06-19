using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Foundation.Groups;

namespace MoneyFox.Presentation.ViewModels.Interfaces
{
    public interface IPaymentListViewModel
    {
        IBalanceViewModel BalanceViewModel { get; }

        IPaymentListViewActionViewModel ViewActionViewModel { get; }
        
        RelayCommand InitializeCommand { get; }

        RelayCommand<PaymentViewModel> EditPaymentCommand { get; }

        RelayCommand<PaymentViewModel> DeletePaymentCommand { get; }

        ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>> Source { get; }

        ObservableCollection<DateListGroupCollection<PaymentViewModel>> DailyList { get; }

        string Title { get; }

        int AccountId { get; }

        bool IsPaymentsEmpty { get; }
    }
}