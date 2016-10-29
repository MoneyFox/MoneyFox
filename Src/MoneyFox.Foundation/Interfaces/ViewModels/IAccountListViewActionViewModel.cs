using MvvmCross.Core.ViewModels;

namespace MoneyFox.Foundation.Interfaces.ViewModels
{
    public interface IAccountListViewActionViewModel : IViewActionViewModel
    {
        MvxCommand GoToAddAccountCommand { get; }
    }
}