using System.Collections.ObjectModel;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Foundation.Groups;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimePaymentListViewModel : BaseViewModel, IPaymentListViewModel
    {
        public IBalanceViewModel BalanceViewModel { get; }
        public IPaymentListViewActionViewModel ViewActionViewModel { get; }
        public MvxAsyncCommand<PaymentViewModel> EditPaymentCommand { get; }
        public MvxAsyncCommand<PaymentViewModel> DeletePaymentCommand { get; }
        public ObservableCollection<DateListGroup<DateListGroup<PaymentViewModel>>> Source { get; }
        public ObservableCollection<DateListGroup<PaymentViewModel>> DailyList { get; }
        public string Title { get; }
        public bool IsPaymentsEmtpy { get; }
    }
}
