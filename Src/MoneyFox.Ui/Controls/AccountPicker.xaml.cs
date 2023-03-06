namespace MoneyFox.Ui.Controls;

using System.Collections;

public partial class AccountPicker
{
    public static readonly BindableProperty PickerTitleProperty = BindableProperty.Create(
        propertyName: nameof(PickerTitle),
        returnType: typeof(string),
        declaringType: typeof(AccountPicker),
        defaultValue: string.Empty);

    public static readonly BindableProperty AccountsSourceProperty = BindableProperty.Create(
        propertyName: nameof(AccountsSource),
        returnType: typeof(IList),
        declaringType: typeof(AccountPicker));

    public static readonly BindableProperty SelectedAccountProperty = BindableProperty.Create(
        propertyName: nameof(SelectedAccount),
        returnType: typeof(object),
        declaringType: typeof(AccountPicker),
        defaultValue: default,
        defaultBindingMode: BindingMode.TwoWay);

    public AccountPicker()
    {
        InitializeComponent();
    }

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

    public object SelectedAccount
    {
        get => GetValue(SelectedAccountProperty);
        set => SetValue(SelectedAccountProperty, value);
    }
}
