namespace MoneyFox.Ui.Controls;

public partial class AmountEntry : ContentView
{
    public AmountEntry()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty AmountFieldTitleProperty = BindableProperty.Create(nameof(AmountFieldTitle), typeof(string), typeof(AmountEntry), string.Empty);
    public static readonly BindableProperty EntryTextProperty = BindableProperty.Create(nameof(EntryText), typeof(string), typeof(AmountEntry), string.Empty, BindingMode.TwoWay);
    public static readonly BindableProperty EntryPlaceholderProperty = BindableProperty.Create(nameof(EntryPlaceholder), typeof(string), typeof(AmountEntry), string.Empty);

    public string AmountFieldTitle
    {
        get => (string)GetValue(AmountEntry.AmountFieldTitleProperty);
        set => SetValue(AmountEntry.AmountFieldTitleProperty, value);
    }
    public string EntryText
    {
        get => (string)GetValue(AmountEntry.EntryTextProperty);
        set => SetValue(AmountEntry.EntryTextProperty, value);
    }

    public string EntryPlaceholder
    {
        get => (string)GetValue(AmountEntry.EntryPlaceholderProperty);
        set => SetValue(AmountEntry.EntryPlaceholderProperty, value);
    }

    private void AmountFieldGotFocus(object sender, FocusEventArgs e)
    {
        AmountField.CursorPosition = 0;
        AmountField.SelectionLength = AmountField.Text != null ? AmountField.Text.Length : 0;
    }
}
