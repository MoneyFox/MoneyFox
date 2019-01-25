using MvvmCross.Commands;

namespace MoneyFox.ServiceLayer.ViewModels.Interfaces
{
    /// <inheritdoc />
    public interface IAccountListViewActionViewModel : IViewActionViewModel
    {
        MvxAsyncCommand GoToAddAccountCommand { get; }
    }
}