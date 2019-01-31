using System.Collections.ObjectModel;
using System.Globalization;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;
using MoneyFox.ServiceLayer.ViewModels.Interfaces;
using MvvmCross.Commands;

namespace MoneyFox.ServiceLayer.ViewModels.DesignTime
{
    public class DesignTimePaymentListViewModel : IPaymentListViewModel
    {
        public LocalizedResources Resources { get; } = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);

        public IBalanceViewModel BalanceViewModel { get; }
        public IPaymentListViewActionViewModel ViewActionViewModel { get; }
        public MvxAsyncCommand<PaymentViewModel> EditPaymentCommand { get; }
        public MvxAsyncCommand<PaymentViewModel> DeletePaymentCommand { get; }
        public ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>> Source => new ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>>
        {
            new DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>("Januar 1992")
            {
                new DateListGroupCollection<PaymentViewModel>("31.1.1992") {
                    new PaymentViewModel{Amount = 123, Category = new CategoryViewModel {Name = "Beer"}},
                    new PaymentViewModel{Amount = 123, Category = new CategoryViewModel{Name = "Beer"}}
                }
            }
        };
        public ObservableCollection<DateListGroupCollection<PaymentViewModel>> DailyList { get; }

        public string Title => "Sparkonto";
        public int AccountId { get; } = 13;
        public bool IsPaymentsEmtpy { get; }
    }
}
