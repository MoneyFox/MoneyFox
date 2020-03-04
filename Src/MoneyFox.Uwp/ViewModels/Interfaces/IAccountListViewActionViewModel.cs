using GalaSoft.MvvmLight.Command;

namespace MoneyFox.Uwp.ViewModels.Interfaces
{
    /// <inheritdoc />
    public interface IAccountListViewActionViewModel : IViewActionViewModel
    {
        RelayCommand GoToAddAccountCommand { get; }
    }
}
