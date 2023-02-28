namespace MoneyFox.Ui.Views.Backup;

using CommunityToolkit.Mvvm.ComponentModel;

internal class UserAccountViewModel : ObservableViewModelBase
{
    private string email = string.Empty;
    private string name = string.Empty;

    public string Name
    {
        get => name;
        set => SetProperty( ref name,   value);
    }

    public string Email
    {
        get => email;
        set => SetProperty( ref email,   value);
    }
}
