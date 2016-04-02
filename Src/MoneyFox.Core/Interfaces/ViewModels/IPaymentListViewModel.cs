using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Core.Groups;
using MoneyFox.Core.ViewModels.Models;

namespace MoneyFox.Core.Interfaces.ViewModels
{
    public interface IPaymentViewModelListViewModel
    {
        IBalanceViewModel BalanceViewModel { get; }

        RelayCommand LoadCommand { get; }

        RelayCommand<string> GoToAddPaymentViewModelCommand { get; }

        RelayCommand DeleteAccountCommand { get; }

        RelayCommand<PaymentViewModel> EditCommand { get; }

        RelayCommand<PaymentViewModel> DeletePaymentViewModelCommand { get; }

        ObservableCollection<PaymentViewModel> RelatedPaymentViewModels { get; }

        ObservableCollection<DateListGroup<PaymentViewModel>> Source { get; }

        string Title { get; }
    }
}