namespace MoneyFox.Ui.Controls;

public partial class MyEntry : ContentView
{
    public MyEntry()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty EntryTextProperty = BindableProperty.Create(nameof(EntryText), typeof(string), typeof(MyEntry), string.Empty, BindingMode.TwoWay);
    public static readonly BindableProperty EntryPlaceholderProperty = BindableProperty.Create(nameof(EntryPlaceholder), typeof(string), typeof(MyEntry), string.Empty);
    public static readonly BindableProperty EntryKeyboardProperty = BindableProperty.Create(nameof(EntryKeyboard), typeof(Keyboard), typeof(MyEntry), Keyboard.Default);
    public static readonly BindableProperty EntryHorizontalTextAlignmentProperty = BindableProperty.Create(nameof(EntryHorizontalTextAlignment), typeof(TextAlignment), typeof(MyEntry), TextAlignment.Start);

    public string EntryText
    {
        get => (string)GetValue(MyEntry.EntryTextProperty);
        set => SetValue(MyEntry.EntryTextProperty, value);
    }

    public string EntryPlaceholder
    {
        get => (string)GetValue(MyEntry.EntryPlaceholderProperty);
        set => SetValue(MyEntry.EntryPlaceholderProperty, value);
    }

    public Keyboard EntryKeyboard
    {
        get => (Keyboard)GetValue(MyEntry.EntryKeyboardProperty);
        set => SetValue(MyEntry.EntryKeyboardProperty, value);
    }

    public TextAlignment EntryHorizontalTextAlignment
    {
        get => (TextAlignment)GetValue(MyEntry.EntryHorizontalTextAlignmentProperty);
        set => SetValue(MyEntry.EntryHorizontalTextAlignmentProperty, value);
    }
}
