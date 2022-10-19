namespace MoneyFox.Ui.Controls;

public partial class TextEntry : ContentView
{
    public TextEntry()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty TextFieldTitleProperty = BindableProperty.Create(nameof(TextFieldTitle), typeof(string), typeof(TextEntry), string.Empty);
    public static readonly BindableProperty EntryTextProperty = BindableProperty.Create(nameof(EntryText), typeof(string), typeof(TextEntry), string.Empty, BindingMode.TwoWay);
    public static readonly BindableProperty EntryPlaceholderProperty = BindableProperty.Create(nameof(EntryPlaceholder), typeof(string), typeof(TextEntry), string.Empty);

    public string TextFieldTitle
    {
        get => (string)GetValue(TextEntry.TextFieldTitleProperty);
        set => SetValue(TextEntry.TextFieldTitleProperty, value);
    }
    public string EntryText
    {
        get => (string)GetValue(TextEntry.EntryTextProperty);
        set => SetValue(TextEntry.EntryTextProperty, value);
    }

    public string EntryPlaceholder
    {
        get => (string)GetValue(TextEntry.EntryPlaceholderProperty);
        set => SetValue(TextEntry.EntryPlaceholderProperty, value);
    }
}
