using System.Collections.ObjectModel;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Groups;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Foundation.Interfaces.ViewModels
{
    public interface IPaymentListViewModel
    {
        IBalanceViewModel BalanceViewModel { get; }

        MvxCommand LoadCommand { get; }

        MvxCommand DeleteAccountCommand { get; }

        MvxCommand<PaymentViewModel> EditCommand { get; }

        MvxCommand<PaymentViewModel> DeletePaymentCommand { get; }

        ObservableCollection<PaymentViewModel> RelatedPayments { get; }

        ObservableCollection<DateListGroup<PaymentViewModel>> Source { get; }

        string Title { get; }
    }
}