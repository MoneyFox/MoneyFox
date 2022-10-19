namespace MoneyFox.Ui.Controls;

using System.Collections;

public partial class AccountPicker : ContentView
{
    public AccountPicker()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty PickerTitleProperty = BindableProperty.Create(nameof(PickerTitle), typeof(string), typeof(AccountPicker), string.Empty, BindingMode.OneWay);
    public static readonly BindableProperty PickerItemsSourceProperty = BindableProperty.Create(nameof(PickerItemsSource), typeof(IList), typeof(AccountPicker));
    public static readonly BindableProperty PickerSelectedItemProperty = BindableProperty.Create(nameof(PickerSelectedItem), typeof(object), typeof(AccountPicker), BindingMode.TwoWay);

    public string PickerTitle
    {
        get => (string)GetValue(AccountPicker.PickerTitleProperty);
        set => SetValue(AccountPicker.PickerTitleProperty, value);
    }
    public IList PickerItemsSource
    {
        get => (IList)GetValue(AccountPicker.PickerItemsSourceProperty);
        set => SetValue(AccountPicker.PickerItemsSourceProperty, value);
    }

    public object PickerSelectedItem
    {
        get => GetValue(AccountPicker.PickerSelectedItemProperty);
        set => SetValue(AccountPicker.PickerSelectedItemProperty, value);
    }
}
