using System.Collections.ObjectModel;
using MoneyFox.Shared.Groups;
using MoneyFox.Shared.Model;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Shared.Interfaces.ViewModels {
    public interface IPaymentListViewModel {
        IBalanceViewModel BalanceViewModel { get; }

        MvxCommand LoadCommand { get; }

        MvxCommand<string> GoToAddPaymentCommand { get; }

        MvxCommand DeleteAccountCommand { get; }

        MvxCommand<Payment> EditCommand { get; }

        MvxCommand<Payment> DeletePaymentCommand { get; }

        ObservableCollection<Payment> RelatedPayments { get; }

        ObservableCollection<DateListGroup<Payment>> Source { get; }

        string Title { get; }
    }
}