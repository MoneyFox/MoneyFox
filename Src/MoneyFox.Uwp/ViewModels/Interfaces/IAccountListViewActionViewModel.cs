using CommunityToolkit.Mvvm.Input;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Interfaces
{
    /// <inheritdoc />
    public interface IAccountListViewActionViewModel
    {
        RelayCommand GoToAddAccountCommand { get; }
    }
}