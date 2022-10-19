namespace MoneyFox.Ui.Controls;

using System.Collections;

public partial class AccountPicker : ContentView
{
    public AccountPicker()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty PickerTitleProperty = BindableProperty.Create(nameof(PickerTitle), typeof(string), typeof(AccountPicker), string.Empty);
    public static readonly BindableProperty PickerItemsSourceProperty = BindableProperty.Create(nameof(PickerItemsSource), typeof(IList), typeof(AccountPicker));
    public static readonly BindableProperty PickerSelectedItemProperty = BindableProperty.Create(nameof(PickerSelectedItem), typeof(object), typeof(AccountPicker));
    public static readonly BindableProperty PickerIsVisibleProperty = BindableProperty.Create(nameof(PickerIsVisible), typeof(bool), typeof(AccountPicker));

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

    public bool PickerIsVisible
    {
        get => (bool)GetValue(AccountPicker.PickerIsVisibleProperty);
        set => SetValue(AccountPicker.PickerIsVisibleProperty, value);
    }
}
