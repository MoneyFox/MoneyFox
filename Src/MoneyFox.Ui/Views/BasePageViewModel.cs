namespace MoneyFox.Ui.Views;

using CommunityToolkit.Mvvm.ComponentModel;

public abstract class BasePageViewModel : ObservableRecipient
{
    private bool isBusy;
    public bool IsBusy
    {
        get => isBusy;
        set => SetProperty(field: ref isBusy, newValue: value);
    }
}
