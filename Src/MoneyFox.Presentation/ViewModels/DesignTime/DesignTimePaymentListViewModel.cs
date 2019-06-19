using System.Collections.ObjectModel;
using System.Globalization;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.ViewModels.Interfaces;
using MoneyFox.ServiceLayer.Utilities;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimePaymentListViewModel : IPaymentListViewModel
    {
        public LocalizedResources Resources { get; } = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);

        public IBalanceViewModel BalanceViewModel { get; }
        public IPaymentListViewActionViewModel ViewActionViewModel { get; }
        public RelayCommand InitializeCommand { get; }
        public RelayCommand<PaymentViewModel> EditPaymentCommand { get; }
        public RelayCommand<PaymentViewModel> DeletePaymentCommand { get; }
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
        public bool IsPaymentsEmpty { get; }
    }
}
