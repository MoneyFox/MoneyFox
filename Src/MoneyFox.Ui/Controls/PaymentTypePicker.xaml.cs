namespace MoneyFox.Ui.Controls;

using System.Collections;
using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;

public partial class PaymentTypePicker : ContentView
{
    public PaymentTypePicker()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty PickerTitleProperty = BindableProperty.Create(nameof(PickerTitle), typeof(string), typeof(PaymentTypePicker), string.Empty, BindingMode.OneWay);
    public static readonly BindableProperty PaymentTypeSourceProperty = BindableProperty.Create(nameof(PaymentTypeSource), typeof(IList), typeof(PaymentTypePicker));
    public static readonly BindableProperty SelectedTypeProperty = BindableProperty.Create(nameof(SelectedType), typeof(PaymentType), typeof(PaymentTypePicker), default, BindingMode.TwoWay);

    public string PickerTitle
    {
        get => (string)GetValue(PaymentTypePicker.PickerTitleProperty);
        set => SetValue(PaymentTypePicker.PickerTitleProperty, value);
    }
    public IList PaymentTypeSource
    {
        get => (IList)GetValue(PaymentTypePicker.PaymentTypeSourceProperty);
        set => SetValue(PaymentTypePicker.PaymentTypeSourceProperty, value);
    }

    public PaymentType SelectedType
    {
        get => (PaymentType)GetValue(PaymentTypePicker.SelectedTypeProperty);
        set => SetValue(PaymentTypePicker.SelectedTypeProperty, value);
    }
}
