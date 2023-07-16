namespace MoneyFox.Ui.Views;
using CommunityToolkit.Mvvm.ComponentModel;

public sealed class Observable<T> : ObservableObject
{
    private T? content;
    public T? Content
    {
        get => content;
        set => SetProperty(field: ref content, newValue: value);
    }
}
