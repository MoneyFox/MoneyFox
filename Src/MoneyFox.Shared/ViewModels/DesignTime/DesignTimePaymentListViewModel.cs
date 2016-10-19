using System.Collections.ObjectModel;
using MoneyFox.Business.Groups;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Shared.Interfaces.ViewModels;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Shared.ViewModels.DesignTime
{
    public class DesignTimePaymentListViewModel : IPaymentListViewModel
    {
        public IBalanceViewModel BalanceViewModel { get; }
        public MvxCommand LoadCommand { get; }
        public MvxCommand<string> GoToAddPaymentCommand => new MvxCommand<string>(s => { });
        public MvxCommand DeleteAccountCommand => new MvxCommand(() => { });
        public MvxCommand<PaymentViewModel> EditCommand => new MvxCommand<PaymentViewModel>(s => { });
        public MvxCommand<PaymentViewModel> DeletePaymentCommand => new MvxCommand<PaymentViewModel>(s => { });
        public ObservableCollection<PaymentViewModel> RelatedPayments => new ObservableCollection<PaymentViewModel>();

        public ObservableCollection<DateListGroup<PaymentViewModel>> Source => new ObservableCollection<DateListGroup<PaymentViewModel>>
        {
            new DateListGroup<PaymentViewModel>("31.1.1992")
            {
                new PaymentViewModel {Amount = 123, Category = new CategoryViewModel {Name = "Beer"}}
            }
        };

        public string Title => "Sparkonto";
    }
}