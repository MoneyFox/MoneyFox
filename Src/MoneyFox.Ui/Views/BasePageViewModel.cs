namespace MoneyFox.Ui.Views;

using CommunityToolkit.Mvvm.ComponentModel;

public abstract class BasePageViewModel : ObservableRecipient
{
    public virtual Task OnNavigatedAsync(object? parameter)
    {
        return Task.CompletedTask;
    }

    public virtual Task OnNavigatedBackAsync(object? parameter)
    {
        return Task.CompletedTask;
    }
}
