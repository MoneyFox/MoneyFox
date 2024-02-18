namespace MoneyFox.Ui.Controls;

using CommunityToolkit.Mvvm.Input;

public partial class CustomStepperWithEntry
{

    public static readonly BindableProperty MinValueProperty = BindableProperty.Create(
        propertyName: nameof(MinValue),
        returnType: typeof(int),
        declaringType: typeof(TextEntry),
        defaultValue: 0);

    public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(
        propertyName: nameof(MaxValue),
        returnType: typeof(int),
        declaringType: typeof(TextEntry),
        defaultValue: 100);

    public static readonly BindableProperty StepSizeProperty = BindableProperty.Create(
        propertyName: nameof(StepSize),
        returnType: typeof(int),
        declaringType: typeof(TextEntry),
        defaultValue: 1);

    public static readonly BindableProperty StepperValueProperty = BindableProperty.Create(
        propertyName: nameof(StepperValue),
        returnType: typeof(int),
        declaringType: typeof(TextEntry),
        defaultValue: 10,
        defaultBindingMode: BindingMode.TwoWay);

    public CustomStepperWithEntry()
    {
        InitializeComponent();
    }

    public int MaxValue
    {
        get => (int)GetValue(MaxValueProperty);
        set => SetValue(property: MaxValueProperty, value: value);
    }

    public int MinValue
    {
        get => (int)GetValue(MinValueProperty);
        set => SetValue(property: MinValueProperty, value: value);
    }

    public int StepSize
    {
        get => (int)GetValue(StepSizeProperty);
        set => SetValue(property: StepSizeProperty, value: value);
    }

    public int StepperValue
    {
        get => (int)GetValue(StepperValueProperty);
        set {

            if (value < MinValue) { value = MinValue; }
            if (value > MaxValue) { value = MaxValue; }

            SetValue(property: StepperValueProperty, value: value);
        }
    }

    private void StepperValueEntry_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (!int.TryParse(e.NewTextValue, out var num))
        {
            // clean out invalid characters
            stepperValueEntry.Text = new string(e.NewTextValue.Where(char.IsDigit).ToArray());
        }
    }

    public RelayCommand IncreaseStepperValue => new(
        () => UpdateStepperValue(StepSize, 1));

    public RelayCommand DecreaseStepperValue => new(
        () => UpdateStepperValue(StepSize, -1));

    private void UpdateStepperValue(int step, int index)
    {
        StepperValue += step * index;
    }
}
