using System.Collections.ObjectModel;
using MoneyFox.Foundation.Groups;
using MvvmCross.Commands;

namespace MoneyFox.ServiceLayer.ViewModels.Interfaces
{
    public interface IPaymentListViewModel
    {
        IBalanceViewModel BalanceViewModel { get; }

        PaymentListViewActionViewModel ViewActionViewModel { get; }
        
        MvxAsyncCommand<PaymentViewModel> EditPaymentCommand { get; }

        MvxAsyncCommand<PaymentViewModel> DeletePaymentCommand { get; }

        ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>> Source { get; }

        ObservableCollection<DateListGroupCollection<PaymentViewModel>> DailyList { get; }

        string Title { get; }

        int AccountId { get; }

        bool IsPaymentsEmpty { get; }
    }
}