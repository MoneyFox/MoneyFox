using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.Interfaces
{
    public interface IPaymentListViewActionViewModel : IViewActionViewModel
    {
        MvxCommand DeleteAccountCommand { get; }
    }
}