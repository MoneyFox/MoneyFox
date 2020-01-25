using System.Collections.ObjectModel;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.Groups;

namespace MoneyFox.Presentation.ViewModels.Interfaces
{
    public interface IPaymentListViewModel
    {
        IBalanceViewModel BalanceViewModel { get; }

        IPaymentListViewActionViewModel ViewActionViewModel { get; }

        AsyncCommand InitializeCommand { get; }
        AsyncCommand LoadDataCommand { get; }

        ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>> Source { get; }

        ObservableCollection<DateListGroupCollection<PaymentViewModel>> DailyList { get; }

        string Title { get; }

        int AccountId { get; }

        bool IsPaymentsEmpty { get; }
    }
}
