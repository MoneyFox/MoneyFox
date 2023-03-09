namespace MoneyFox.Ui.Views.Backup;

using CommunityToolkit.Mvvm.ComponentModel;

internal class UserAccountViewModel : ObservableObject
{
    private string email = string.Empty;
    private string name = string.Empty;

    public string Name
    {
        get => name;
        set => SetProperty(field: ref name, newValue: value);
    }

    public string Email
    {
        get => email;
        set => SetProperty(field: ref email, newValue: value);
    }
}
