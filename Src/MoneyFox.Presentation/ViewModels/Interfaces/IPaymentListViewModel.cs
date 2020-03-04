using System.Collections.ObjectModel;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Groups;

namespace MoneyFox.Presentation.ViewModels.Interfaces
{
    public interface IPaymentListViewModel
    {
        IBalanceViewModel BalanceViewModel { get; }

        IPaymentListViewActionViewModel ViewActionViewModel { get; }

        AsyncCommand InitializeCommand { get; }
        AsyncCommand LoadDataCommand { get; }

        ObservableCollection<DateListGroupCollection<PaymentViewModel>> DailyList { get; }

        string Title { get; }

        int AccountId { get; }

        bool IsPaymentsEmpty { get; }
    }
}
