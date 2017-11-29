using System.Collections.ObjectModel;
using MoneyFox.Business.ViewModels;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation.Groups;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Windows.DesignTime
{
    public class DesignTimePaymentListViewModel : IPaymentListViewModel
    {
        public IBalanceViewModel BalanceViewModel { get; }
        public IPaymentListViewActionViewModel ViewActionViewModel { get; }
        public MvxAsyncCommand LoadCommand { get; }
        public MvxAsyncCommand<PaymentViewModel> EditPaymentCommand { get; }
        public MvxCommand<string> GoToAddPaymentCommand => new MvxCommand<string>(s => { });
        public MvxCommand DeleteAccountCommand => new MvxCommand(() => { });
        public MvxAsyncCommand<PaymentViewModel> DeletePaymentCommand => new MvxAsyncCommand<PaymentViewModel>(async s => { });
        public ObservableCollection<PaymentViewModel> RelatedPayments => new ObservableCollection<PaymentViewModel>();

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

        public string Title => "Sparkonto";
    }
}