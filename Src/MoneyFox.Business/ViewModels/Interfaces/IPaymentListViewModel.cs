using System.Collections.ObjectModel;
using MoneyFox.Foundation.Groups;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.Interfaces
{
    public interface IPaymentListViewModel
    {
        IBalanceViewModel BalanceViewModel { get; }

        IPaymentListViewActionViewModel ViewActionViewModel { get; }
        
        MvxAsyncCommand<PaymentViewModel> EditPaymentCommand { get; }

        MvxAsyncCommand<PaymentViewModel> DeletePaymentCommand { get; }

        ObservableCollection<DateListGroup<DateListGroup<PaymentViewModel>>> Source { get; }

        ObservableCollection<DateListGroup<PaymentViewModel>> DailyList { get; }

        string Title { get; }

        bool IsPaymentsEmtpy { get; }
    }
}