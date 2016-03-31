using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Core.Groups;
using MoneyFox.Core.Interfaces.ViewModels;
using MoneyFox.Core.Model;

namespace MoneyManager.Core.ViewModels.DesignTime
{
    public class DesignTimePaymentViewModelListViewModel : IPaymentViewModelListViewModel
    {
        public IBalanceViewModel BalanceViewModel { get; }
        public RelayCommand LoadCommand { get; }
        public RelayCommand<string> GoToAddPaymentViewModelCommand => new RelayCommand<string>(s => { });
        public RelayCommand DeleteAccountCommand => new RelayCommand(() => { });
        public RelayCommand<PaymentViewModel> EditCommand => new RelayCommand<PaymentViewModel>(s => { });
        public RelayCommand<PaymentViewModel> DeletePaymentViewModelCommand => new RelayCommand<PaymentViewModel>(s => { });
        public ObservableCollection<PaymentViewModel> RelatedPaymentViewModels => new ObservableCollection<PaymentViewModel>();

        public ObservableCollection<DateListGroup<PaymentViewModel>> Source => new ObservableCollection<DateListGroup<PaymentViewModel>>
        {
            new DateListGroup<PaymentViewModel>("31.1.1992")
            {
                new PaymentViewModel {Amount = 123, Category = new Category {Name = "Beer"}}
            }
        };

        public string Title => "Sparkonto";
    }
}