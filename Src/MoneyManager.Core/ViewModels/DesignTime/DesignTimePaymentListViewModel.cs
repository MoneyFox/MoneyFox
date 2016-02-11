using System.Collections.ObjectModel;
using MoneyManager.Foundation.Groups;
using MoneyManager.Foundation.Interfaces.ViewModels;
using MoneyManager.Foundation.Model;
using MvvmCross.Core.ViewModels;

namespace MoneyManager.Core.ViewModels.DesignTime
{
    public class DesignTimePaymentListViewModel : IPaymentListViewModel
    {
        public IBalanceViewModel BalanceViewModel { get; }
        public MvxCommand LoadedCommand { get; }
        public MvxCommand<string> GoToAddPaymentCommand => new MvxCommand<string>(s => { });
        public MvxCommand DeleteAccountCommand => new MvxCommand(() => { });
        public MvxCommand<Payment> EditCommand => new MvxCommand<Payment>(s => { });
        public MvxCommand<Payment> DeletePaymentCommand => new MvxCommand<Payment>(s => { });
        public ObservableCollection<Payment> RelatedPayments => new ObservableCollection<Payment>();

        public ObservableCollection<DateListGroup<Payment>> Source => new ObservableCollection<DateListGroup<Payment>>
        {
            new DateListGroup<Payment>("31.1.1992")
            {
                new Payment {Amount = 123, Category = new Category {Name = "Beer"}}
            }
        };

        public string Title => "Sparkonto";
    }
}