using MvvmCross.Commands;

namespace MoneyFox.Business.ViewModels.Interfaces
{
    /// <inheritdoc />
    public interface IAccountListViewActionViewModel : IViewActionViewModel
    {
        MvxAsyncCommand GoToAddAccountCommand { get; }
    }
}