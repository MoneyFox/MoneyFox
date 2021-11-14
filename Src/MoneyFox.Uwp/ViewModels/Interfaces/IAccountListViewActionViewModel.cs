using CommunityToolkit.Mvvm.Input;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Interfaces
{
    public interface IAccountListViewActionViewModel
    {
        RelayCommand GoToAddAccountCommand { get; }
    }
}