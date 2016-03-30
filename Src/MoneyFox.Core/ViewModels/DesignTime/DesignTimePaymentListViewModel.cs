using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Core.Groups;
using MoneyFox.Core.Interfaces.ViewModels;
using MoneyFox.Core.Model;
using MoneyFox.Foundation.Model;

namespace MoneyManager.Core.ViewModels.DesignTime
{
    public class DesignTimePaymentListViewModel : IPaymentListViewModel
    {
        public IBalanceViewModel BalanceViewModel { get; }
        public RelayCommand LoadCommand { get; }
        public RelayCommand<string> GoToAddPaymentCommand => new RelayCommand<string>(s => { });
        public RelayCommand DeleteAccountCommand => new RelayCommand(() => { });
        public RelayCommand<Payment> EditCommand => new RelayCommand<Payment>(s => { });
        public RelayCommand<Payment> DeletePaymentCommand => new RelayCommand<Payment>(s => { });
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