using System.Collections.ObjectModel;
using MoneyManager.Foundation.Groups;
using MoneyManager.Foundation.Interfaces.ViewModels;
using MoneyManager.Foundation.Model;
using MvvmCross.Core.ViewModels;

namespace MoneyManager.Core.ViewModels
{
    public interface IPaymentListViewModel
    {
        IBalanceViewModel BalanceViewModel { get; }

        MvxCommand<string> GoToAddPaymentCommand { get; }

        MvxCommand DeleteAccountCommand { get; }

        MvxCommand<Payment> EditCommand { get; }

        MvxCommand<Payment> DeletePaymentCommand { get; }

        ObservableCollection<Payment> RelatedPayments { get; }

        ObservableCollection<DateListGroup<Payment>> Source { get; }

        string Title { get; }

        Payment SelectedPayment { get; }
    }
}