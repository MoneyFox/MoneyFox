namespace MoneyFox.Ui.Controls;

using System.Collections;

public partial class PaymentTypePicker : ContentView
{
    public PaymentTypePicker()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty PickerTitleProperty = BindableProperty.Create(nameof(PickerTitle), typeof(string), typeof(PaymentTypePicker), string.Empty, BindingMode.OneWay);
    public static readonly BindableProperty PickerItemsSourceProperty = BindableProperty.Create(nameof(PickerItemsSource), typeof(IList), typeof(PaymentTypePicker));
    public static readonly BindableProperty PickerSelectedItemProperty = BindableProperty.Create(nameof(PickerSelectedItem), typeof(object), typeof(PaymentTypePicker), BindingMode.TwoWay);

    public string PickerTitle
    {
        get => (string)GetValue(PaymentTypePicker.PickerTitleProperty);
        set => SetValue(PaymentTypePicker.PickerTitleProperty, value);
    }
    public IList PickerItemsSource
    {
        get => (IList)GetValue(PaymentTypePicker.PickerItemsSourceProperty);
        set => SetValue(PaymentTypePicker.PickerItemsSourceProperty, value);
    }

    public object PickerSelectedItem
    {
        get => GetValue(PaymentTypePicker.PickerSelectedItemProperty);
        set => SetValue(PaymentTypePicker.PickerSelectedItemProperty, value);
    }
}
