using System.Collections.ObjectModel;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Groups;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimePaymentListViewModel : IPaymentListViewModel
    {
        public IBalanceViewModel BalanceViewModel { get; }
        public IPaymentListViewActionViewModel ViewActionViewModel { get; }
        public MvxCommand LoadCommand { get; }
        public MvxCommand<PaymentViewModel> EditPaymentCommand { get; }
        public MvxCommand<string> GoToAddPaymentCommand => new MvxCommand<string>(s => { });
        public MvxCommand DeleteAccountCommand => new MvxCommand(() => { });
        public MvxCommand<PaymentViewModel> DeletePaymentCommand => new MvxCommand<PaymentViewModel>(s => { });
        public ObservableCollection<PaymentViewModel> RelatedPayments => new ObservableCollection<PaymentViewModel>();

        public ObservableCollection<DateListGroup<DateListGroup<PaymentViewModel>>> Source => new ObservableCollection<DateListGroup<DateListGroup<PaymentViewModel>>>
        {
            new DateListGroup<DateListGroup<PaymentViewModel>>("Januar 1992")
            {
                new DateListGroup<PaymentViewModel>("31.1.1992") {
                new PaymentViewModel {Amount = 123, Category = new CategoryViewModel {Name = "Beer"}},
                new PaymentViewModel {Amount = 123, Category = new CategoryViewModel {Name = "Beer"}}
                }
            }
        };

        public string Title => "Sparkonto";
    }
}