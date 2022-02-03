using CommunityToolkit.Mvvm.Input;

namespace MoneyFox.Win.ViewModels.Interfaces
{
    public interface IAccountListViewActionViewModel
    {
        RelayCommand GoToAddAccountCommand { get; }
    }
}