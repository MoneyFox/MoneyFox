using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Groups;
using System.Collections.ObjectModel;

namespace MoneyFox.Uwp.ViewModels.Interfaces
{
    public interface IPaymentListViewModel
    {
        IBalanceViewModel BalanceViewModel { get; }

        IPaymentListViewActionViewModel ViewActionViewModel { get; }

        AsyncCommand InitializeCommand { get; }

        AsyncCommand LoadDataCommand { get; }

        ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>> Source { get; }

        string Title { get; }

        int AccountId { get; }

        bool IsPaymentsEmpty { get; }
    }
}
