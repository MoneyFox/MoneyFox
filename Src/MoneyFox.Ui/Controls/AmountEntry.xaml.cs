namespace MoneyFox.Ui.Controls;

public partial class AmountEntry : ContentView
{
    public AmountEntry()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty EntryTextProperty = BindableProperty.Create(nameof(EntryText), typeof(string), typeof(AmountEntry), string.Empty, BindingMode.TwoWay);
    public static readonly BindableProperty EntryPlaceholderProperty = BindableProperty.Create(nameof(EntryPlaceholder), typeof(string), typeof(AmountEntry), string.Empty);
    public static readonly BindableProperty EntryKeyboardProperty = BindableProperty.Create(nameof(EntryKeyboard), typeof(Keyboard), typeof(AmountEntry), Keyboard.Default);
    public static readonly BindableProperty EntryHorizontalTextAlignmentProperty = BindableProperty.Create(nameof(EntryHorizontalTextAlignment), typeof(TextAlignment), typeof(AmountEntry), TextAlignment.Start);

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

    public Keyboard EntryKeyboard
    {
        get => (Keyboard)GetValue(AmountEntry.EntryKeyboardProperty);
        set => SetValue(AmountEntry.EntryKeyboardProperty, value);
    }

    public TextAlignment EntryHorizontalTextAlignment
    {
        get => (TextAlignment)GetValue(AmountEntry.EntryHorizontalTextAlignmentProperty);
        set => SetValue(AmountEntry.EntryHorizontalTextAlignmentProperty, value);
    }
}
