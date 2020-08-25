using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;

namespace MoneyFox.Uwp.ViewModels.Interfaces
{
    public interface IPaymentListViewModel
    {
        IBalanceViewModel BalanceViewModel { get; }

        IPaymentListViewActionViewModel ViewActionViewModel { get; }

        RelayCommand InitializeCommand { get; }

        RelayCommand LoadDataCommand { get; }

        List<PaymentViewModel> Payments { get; }

        string Title { get; }

        int AccountId { get; }
    }
}
