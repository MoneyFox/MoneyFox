using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Foundation.Model;
using MoneyManager.Foundation.Groups;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Foundation.Interfaces.ViewModels
{
    public interface IPaymentListViewModel
    {
        IBalanceViewModel BalanceViewModel { get; }

        RelayCommand LoadCommand { get; }

        RelayCommand<string> GoToAddPaymentCommand { get; }

        RelayCommand DeleteAccountCommand { get; }

        RelayCommand<Payment> EditCommand { get; }

        RelayCommand<Payment> DeletePaymentCommand { get; }

        ObservableCollection<Payment> RelatedPayments { get; }

        ObservableCollection<DateListGroup<Payment>> Source { get; }

        string Title { get; }
    }
}