namespace MoneyFox.Ui.Views;

using CommunityToolkit.Mvvm.ComponentModel;

public class BasePageViewModel : ObservableRecipient
{
    protected BasePageViewModel()
    {
        IsActive = true;
    }
}
