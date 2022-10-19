namespace MoneyFox.Ui.Controls;

using System.Collections;

public partial class AccountPicker : ContentView
{
    public AccountPicker()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty PickerTitleProperty = BindableProperty.Create(nameof(PickerTitle), typeof(string), typeof(AccountPicker), string.Empty, BindingMode.OneWay);
    public static readonly BindableProperty AccountsSourceProperty = BindableProperty.Create(nameof(AccountsSource), typeof(IList), typeof(AccountPicker));
    public static readonly BindableProperty SelectedAccountProperty = BindableProperty.Create(nameof(SelectedAccount), typeof(object), typeof(AccountPicker), BindingMode.TwoWay);

    public string PickerTitle
    {
        get => (string)GetValue(AccountPicker.PickerTitleProperty);
        set => SetValue(AccountPicker.PickerTitleProperty, value);
    }
    public IList AccountsSource
    {
        get => (IList)GetValue(AccountPicker.AccountsSourceProperty);
        set => SetValue(AccountPicker.AccountsSourceProperty, value);
    }

    public object SelectedAccount
    {
        get => GetValue(AccountPicker.SelectedAccountProperty);
        set => SetValue(AccountPicker.SelectedAccountProperty, value);
    }
}
