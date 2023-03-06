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
        returnType: typeof(DateTime),
        declaringType: typeof(CustomDatePicker),
        defaultValue: DateTime.Today,
        defaultBindingMode: BindingMode.TwoWay);

    public CustomDatePicker()
    {
        InitializeComponent();
    }

    public string DatePickerTitle
    {
        get => (string)GetValue(DatePickerTitleProperty);
        set => SetValue(DatePickerTitleProperty, value);
    }

    public DateTime DateField
    {
        get => (DateTime)GetValue(DateFieldProperty);
        set => SetValue(DateFieldProperty, value);
    }
}
