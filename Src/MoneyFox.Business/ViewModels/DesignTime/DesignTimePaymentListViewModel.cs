using System.Collections.ObjectModel;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.DataAccess.Pocos;
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
        public ObservableCollection<DateListGroup<DateListGroup<PaymentViewModel>>> Source => new ObservableCollection<DateListGroup<DateListGroup<PaymentViewModel>>>
        {
            new DateListGroup<DateListGroup<PaymentViewModel>>("Januar 1992")
            {
                new DateListGroup<PaymentViewModel>("31.1.1992") {
                    new PaymentViewModel(new Payment()) {Amount = 123, Category = new CategoryViewModel(new Category()) {Name = "Beer"}},
                    new PaymentViewModel(new Payment()) {Amount = 123, Category = new CategoryViewModel(new Category()) {Name = "Beer"}}
                }
            }
        };
        public ObservableCollection<DateListGroup<PaymentViewModel>> DailyList { get; }

        public string Title => "Sparkonto";
        public int AccountId { get; } = 13;
        public bool IsPaymentsEmtpy { get; }
    }
}
