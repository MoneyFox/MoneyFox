namespace MoneyFox.Ui.Views;

using CommunityToolkit.Mvvm.ComponentModel;

public abstract class BasePageViewModel : ObservableRecipient
{
    protected BasePageViewModel()
    {
        IsActive = true;
    }
}
