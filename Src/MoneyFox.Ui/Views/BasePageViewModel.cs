namespace MoneyFox.Ui.Views;

using CommunityToolkit.Mvvm.ComponentModel;

public abstract class BasePageViewModel : ObservableRecipient
{
    public virtual void OnNavigated(object? parameter)
    {
    }
}
