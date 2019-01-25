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
        public ObservableCollection<DateListGroup<DateListGroup<PaymentViewModel>>> Source => new ObservableCollection<DateListGroup<DateListGroup<PaymentViewModel>>>
        {
            new DateListGroup<DateListGroup<PaymentViewModel>>("Januar 1992")
            {
                new DateListGroup<PaymentViewModel>("31.1.1992") {
                    new PaymentViewModel{Amount = 123, Category = new CategoryViewModel {Name = "Beer"}},
                    new PaymentViewModel{Amount = 123, Category = new CategoryViewModel{Name = "Beer"}}
                }
            }
        };
        public ObservableCollection<DateListGroup<PaymentViewModel>> DailyList { get; }

        public string Title => "Sparkonto";
        public int AccountId { get; } = 13;
        public bool IsPaymentsEmtpy { get; }
    }
}
