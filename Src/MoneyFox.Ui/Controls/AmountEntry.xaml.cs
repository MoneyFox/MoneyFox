namespace MoneyFox.Ui.Controls;

public partial class AmountEntry : ContentView
{
    public AmountEntry()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty AmountFieldTitleProperty = BindableProperty.Create(nameof(AmountFieldTitle), typeof(string), typeof(AmountEntry), string.Empty);
    public static readonly BindableProperty AmountProperty = BindableProperty.Create(nameof(Amount), typeof(string), typeof(AmountEntry), string.Empty, BindingMode.TwoWay);
    public static readonly BindableProperty EntryPlaceholderProperty = BindableProperty.Create(nameof(EntryPlaceholder), typeof(string), typeof(AmountEntry), string.Empty);
    public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(AmountEntry), false);

    public string AmountFieldTitle
    {
        get => (string)GetValue(AmountEntry.AmountFieldTitleProperty);
        set => SetValue(AmountEntry.AmountFieldTitleProperty, value);
    }
    public string Amount
    {
        get => (string)GetValue(AmountEntry.AmountProperty);
        set => SetValue(AmountEntry.AmountProperty, value);
    }

    public string EntryPlaceholder
    {
        get => (string)GetValue(AmountEntry.EntryPlaceholderProperty);
        set => SetValue(AmountEntry.EntryPlaceholderProperty, value);
    }

    public bool IsReadOnly
    {
        get => (bool)GetValue(AmountEntry.IsReadOnlyProperty);
        set => SetValue(AmountEntry.IsReadOnlyProperty, value);
    }

    private void AmountFieldGotFocus(object sender, FocusEventArgs e)
    {
        AmountField.CursorPosition = 0;
        AmountField.SelectionLength = AmountField.Text != null ? AmountField.Text.Length : 0;
    }
}
