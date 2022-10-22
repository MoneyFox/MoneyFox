namespace MoneyFox.Ui.Controls;

using System.Collections;
using MoneyFox.Ui.ViewModels.Accounts;

public partial class AccountPicker : ContentView
{
    public AccountPicker()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty PickerTitleProperty = BindableProperty.Create(nameof(PickerTitle), typeof(string), typeof(AccountPicker), string.Empty, BindingMode.OneWay);
    public static readonly BindableProperty AccountsSourceProperty = BindableProperty.Create(nameof(AccountsSource), typeof(IList), typeof(AccountPicker), default, BindingMode.OneWay);
    public static readonly BindableProperty SelectedAccountProperty = BindableProperty.Create(nameof(SelectedAccount), typeof(AccountViewModel), typeof(AccountPicker), default, BindingMode.TwoWay);

    public string PickerTitle
    {
        get => (string)GetValue(PickerTitleProperty);
        set => SetValue(PickerTitleProperty, value);
    }

    public IList AccountsSource
    {
        get => (IList)GetValue(AccountsSourceProperty);
        set => SetValue(AccountsSourceProperty, value);
    }

    public AccountViewModel SelectedAccount
    {
        get => (AccountViewModel)GetValue(SelectedAccountProperty);
        set => SetValue(SelectedAccountProperty, value);
    }
}
