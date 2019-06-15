using GalaSoft.MvvmLight.Command;
using MoneyFox.ServiceLayer.ViewModels.Interfaces;

namespace MoneyFox.Presentation.ViewModels.Interfaces
{
    /// <inheritdoc />
    public interface IAccountListViewActionViewModel : IViewActionViewModel
    {
        RelayCommand GoToAddAccountCommand { get; }
    }
}