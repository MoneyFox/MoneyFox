using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.Groups;

namespace MoneyFox.Presentation.ViewModels.Interfaces
{
    public interface IPaymentListViewModel
    {
        IBalanceViewModel BalanceViewModel { get; }

        IPaymentListViewActionViewModel ViewActionViewModel { get; }
        
        AsyncCommand InitializeCommand { get; }

        RelayCommand<PaymentViewModel> EditPaymentCommand { get; }

        AsyncCommand<PaymentViewModel> DeletePaymentCommand { get; }

        ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>> Source { get; }

        ObservableCollection<DateListGroupCollection<PaymentViewModel>> DailyList { get; }

        string Title { get; }

        int AccountId { get; }

        bool IsPaymentsEmpty { get; }
    }
}