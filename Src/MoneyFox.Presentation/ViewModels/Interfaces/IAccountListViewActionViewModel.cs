using GalaSoft.MvvmLight.Command;

namespace MoneyFox.Presentation.ViewModels.Interfaces
{
    /// <inheritdoc />
    public interface IAccountListViewActionViewModel : IViewActionViewModel
    {
        RelayCommand GoToAddAccountCommand { get; }
    }
}