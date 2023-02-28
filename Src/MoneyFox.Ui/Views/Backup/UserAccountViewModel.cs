namespace MoneyFox.Ui.Views.Backup;

using CommunityToolkit.Mvvm.ComponentModel;

internal class UserAccountViewModel : ObservableObject
{
    private string email = string.Empty;
    private string name = string.Empty;

    public string Name
    {
        get => name;
        set
        {
            if (name == value)
            {
                return;
            }

            name = value;
            OnPropertyChanged();
        }
    }

    public string Email
    {
        get => email;
        set
        {
            if (email == value)
            {
                return;
            }

            email = value;
            OnPropertyChanged();
        }
    }
}
