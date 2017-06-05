using System.Collections.ObjectModel;
using MoneyFox.Foundation.Groups;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.Interfaces
{
    public interface IPaymentListViewModel
    {
        IBalanceViewModel BalanceViewModel { get; }

        IPaymentListViewActionViewModel ViewActionViewModel { get; }

        MvxAsyncCommand LoadCommand { get; }

        MvxAsyncCommand<PaymentViewModel> EditPaymentCommand { get; }

        MvxAsyncCommand<PaymentViewModel> DeletePaymentCommand { get; }

        ObservableCollection<PaymentViewModel> RelatedPayments { get; }

        ObservableCollection<DateListGroup<DateListGroup<PaymentViewModel>>> Source { get; }

        string Title { get; }
    }
}