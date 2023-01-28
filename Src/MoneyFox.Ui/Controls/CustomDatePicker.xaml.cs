namespace MoneyFox.Ui.Controls;

public partial class CustomDatePicker
{
    public static readonly BindableProperty DatePickerTitleProperty = BindableProperty.Create(
        propertyName: nameof(DatePickerTitle),
        returnType: typeof(string),
        declaringType: typeof(CustomDatePicker),
        defaultValue: string.Empty);

    public static readonly BindableProperty DateFieldProperty = BindableProperty.Create(
        propertyName: nameof(DateField),
        returnType: typeof(string),
        declaringType: typeof(CustomDatePicker),
        defaultValue: string.Empty,
        defaultBindingMode: BindingMode.TwoWay);

    public CustomDatePicker()
    {
        InitializeComponent();
    }

    public string DatePickerTitle
    {
        get => (string)GetValue(DatePickerTitleProperty);
        set => SetValue(property: DatePickerTitleProperty, value: value);
    }

    public string DateField
    {
        get => (string)GetValue(DateFieldProperty);
        set => SetValue(property: DateFieldProperty, value: value);
    }
}
