namespace MoneyFox.Ui.Controls;

public partial class MyEntry : ContentView
{
    public MyEntry()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty EntryTextProperty = BindableProperty.Create(nameof(EntryText), typeof(string), typeof(MyEntry), string.Empty);
    public static readonly BindableProperty EntryPlaceholderProperty = BindableProperty.Create(nameof(EntryPlaceholder), typeof(string), typeof(MyEntry), string.Empty);
    public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(nameof(Keyboard), typeof(string), typeof(MyEntry), string.Empty);
    public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(string), typeof(MyEntry), string.Empty);

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

    public string Keyboard
    {
        get => (string)GetValue(MyEntry.KeyboardProperty);
        set => SetValue(MyEntry.KeyboardProperty, value);
    }

    public string HorizontalTextAlignment
    {
        get => (string)GetValue(MyEntry.HorizontalTextAlignmentProperty);
        set => SetValue(MyEntry.HorizontalTextAlignmentProperty, value);
    }
}
