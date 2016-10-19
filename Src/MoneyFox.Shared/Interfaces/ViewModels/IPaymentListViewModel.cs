using System.Collections.ObjectModel;
using MoneyFox.Shared.Groups;
using MoneyFox.Shared.Model;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Shared.Interfaces.ViewModels
{
    public interface IPaymentListViewModel
    {
        IBalanceViewModel BalanceViewModel { get; }

        MvxCommand LoadCommand { get; }

        MvxCommand<string> GoToAddPaymentCommand { get; }

        MvxCommand DeleteAccountCommand { get; }

        MvxCommand<PaymentViewModel> EditCommand { get; }

        MvxCommand<PaymentViewModel> DeletePaymentCommand { get; }

        ObservableCollection<PaymentViewModel> RelatedPayments { get; }

        ObservableCollection<DateListGroup<PaymentViewModel>> Source { get; }

        string Title { get; }
    }
}