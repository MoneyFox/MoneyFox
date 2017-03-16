using System.Collections.ObjectModel;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Groups;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Foundation.Interfaces.ViewModels
{
    public interface IPaymentListViewModel
    {
        IBalanceViewModel BalanceViewModel { get; }

        IPaymentListViewActionViewModel ViewActionViewModel { get; }

        MvxCommand LoadCommand { get; }

        MvxCommand<PaymentViewModel> EditPaymentCommand { get; }

        MvxCommand<PaymentViewModel> DeletePaymentCommand { get; }

        ObservableCollection<DateListGroup<DateListGroup<PaymentViewModel>>> Source { get; }

        string Title { get; }
    }
}