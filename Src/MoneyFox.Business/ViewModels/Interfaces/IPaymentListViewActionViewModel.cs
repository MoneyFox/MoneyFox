using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.Interfaces
{
    public interface IPaymentListViewActionViewModel : IViewActionViewModel
    {
        MvxAsyncCommand DeleteAccountCommand { get; }
    }
}