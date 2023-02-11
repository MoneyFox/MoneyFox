namespace MoneyFox.Ui.Controls;

using MoneyFox.Core.Common.Settings;
using MoneyFox.Ui.Infrastructure.Adapters;

public partial class AmountEntry
{
    public static readonly BindableProperty AmountFieldTitleProperty = BindableProperty.Create(
        propertyName: nameof(AmountFieldTitle),
        returnType: typeof(string),
        declaringType: typeof(AmountEntry),
        defaultValue: string.Empty);

    public static readonly BindableProperty AmountProperty = BindableProperty.Create(
        propertyName: nameof(Amount),
        returnType: typeof(decimal),
        declaringType: typeof(AmountEntry),
        defaultValue: 0m,
        defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty EntryPlaceholderProperty = BindableProperty.Create(
        propertyName: nameof(EntryPlaceholder),
        returnType: typeof(string),
        declaringType: typeof(AmountEntry),
        defaultValue: string.Empty);

    public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(
        propertyName: nameof(IsReadOnly),
        returnType: typeof(bool),
        declaringType: typeof(AmountEntry),
        defaultValue: false);

    public AmountEntry()
    {
        // The property has to be set before the InitializeComponent, so that the UI is current
        Currency = new SettingsFacade(new SettingsAdapter()).DefaultCurrency;
        InitializeComponent();
    }

    public string Currency { get; }

    public string AmountFieldTitle
    {
        get => (string)GetValue(AmountFieldTitleProperty);
        set => SetValue(property: AmountFieldTitleProperty, value: value);
    }

    public decimal Amount
    {
        get => (decimal)GetValue(AmountProperty);
        set => SetValue(property: AmountProperty, value: value);
    }

    public string EntryPlaceholder
    {
        get => (string)GetValue(EntryPlaceholderProperty);
        set => SetValue(property: EntryPlaceholderProperty, value: value);
    }

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(property: IsReadOnlyProperty, value: value);
    }

    private void AmountFieldGotFocus(object sender, FocusEventArgs e)
    {
        AmountField.CursorPosition = 0;
        AmountField.SelectionLength = AmountField.Text != null ? AmountField.Text.Length : 0;
    }
}
