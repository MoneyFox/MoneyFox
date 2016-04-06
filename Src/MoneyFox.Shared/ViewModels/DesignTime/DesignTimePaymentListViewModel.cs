using System.Collections.ObjectModel;
using MoneyFox.Shared.Groups;
using MoneyFox.Shared.Interfaces.ViewModels;
using MoneyFox.Shared.Model;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Shared.ViewModels.DesignTime
{
    public class DesignTimePaymentListViewModel : IPaymentListViewModel
    {
        public IBalanceViewModel BalanceViewModel { get; }
        public MvxCommand LoadCommand { get; }
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