namespace MoneyFox.Ui.Controls;

using System.Collections;

public partial class PaymentTypePicker
{
    public static readonly BindableProperty PickerTitleProperty = BindableProperty.Create(
        propertyName: nameof(PickerTitle),
        returnType: typeof(string),
        declaringType: typeof(PaymentTypePicker),
        defaultValue: string.Empty);

    public static readonly BindableProperty PaymentTypeSourceProperty = BindableProperty.Create(
        propertyName: nameof(PaymentTypeSource),
        returnType: typeof(IList),
        declaringType: typeof(PaymentTypePicker));

    public static readonly BindableProperty SelectedTypeProperty = BindableProperty.Create(
        propertyName: nameof(SelectedType),
        returnType: typeof(object),
        declaringType: typeof(PaymentTypePicker),
        defaultValue: default,
        defaultBindingMode: BindingMode.TwoWay);

    public PaymentTypePicker()
    {
        InitializeComponent();
    }

    public string PickerTitle
    {
        get => (string)GetValue(PickerTitleProperty);
        set => SetValue(PickerTitleProperty, value);
    }

    public IList PaymentTypeSource
    {
        get => (IList)GetValue(PaymentTypeSourceProperty);
        set => SetValue(PaymentTypeSourceProperty, value);
    }

    public object SelectedType
    {
        get => GetValue(SelectedTypeProperty);
        set => SetValue(SelectedTypeProperty, value);
    }
}
