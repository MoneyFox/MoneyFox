using MvvmCross.Core.ViewModels;

namespace MoneyFox.Foundation.Interfaces.ViewModels
{
    public interface IPaymentListViewActionViewModel : IViewActionViewModel
    {
        MvxCommand DeleteAccountCommand { get; }
    }
}