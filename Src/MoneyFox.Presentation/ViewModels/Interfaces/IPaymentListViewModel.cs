using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Groups;
using System.Collections.ObjectModel;

namespace MoneyFox.Presentation.ViewModels.Interfaces
{
    public interface IPaymentListViewModel
    {
        IBalanceViewModel BalanceViewModel { get; }

        IPaymentListViewActionViewModel ViewActionViewModel { get; }

        AsyncCommand InitializeCommand { get; }

        AsyncCommand LoadDataCommand { get; }

        ObservableCollection<DateListGroupCollection<PaymentViewModel>> PaymentList { get; }

        string Title { get; }

        int AccountId { get; }

        bool IsPaymentsEmpty { get; }
    }
}
