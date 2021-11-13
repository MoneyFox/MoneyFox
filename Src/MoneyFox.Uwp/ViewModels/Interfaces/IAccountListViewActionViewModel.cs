using GalaSoft.MvvmLight.Command;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Interfaces
{
    /// <inheritdoc/>
    public interface IAccountListViewActionViewModel
    {
        RelayCommand GoToAddAccountCommand { get; }
    }
}
