namespace MoneyFox.Ui.Views.Backup;

internal class UserAccountViewModel : ObservableViewModelBase
{
    private string email = string.Empty;
    private string name = string.Empty;

    public string Name
    {
        get => name;
        set => SetProperty(property: ref name, value: value);
    }

    public string Email
    {
        get => email;
        set => SetProperty(property: ref email, value: value);
    }
}
