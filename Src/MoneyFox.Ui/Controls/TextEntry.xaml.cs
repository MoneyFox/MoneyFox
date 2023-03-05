namespace MoneyFox.Ui.Controls;

public partial class TextEntry
{
    public static readonly BindableProperty TextFieldTitleProperty = BindableProperty.Create(
        propertyName: nameof(TextFieldTitle),
        returnType: typeof(string),
        declaringType: typeof(TextEntry),
        defaultValue: string.Empty);

    public static readonly BindableProperty EntryTextProperty = BindableProperty.Create(
        propertyName: nameof(EntryText),
        returnType: typeof(string),
        declaringType: typeof(TextEntry),
        defaultValue: string.Empty,
        defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty EntryPlaceholderProperty = BindableProperty.Create(
        propertyName: nameof(EntryPlaceholder),
        returnType: typeof(string),
        declaringType: typeof(TextEntry),
        defaultValue: string.Empty);

    public static readonly BindableProperty IsRequiredProperty = BindableProperty.Create(
        propertyName: nameof(IsRequired),
        returnType: typeof(bool),
        declaringType: typeof(TextEntry),
        defaultValue: false);

    public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(
        propertyName: nameof(IsReadOnly),
        returnType: typeof(bool),
        declaringType: typeof(TextEntry),
        defaultValue: false);

    public TextEntry()
    {
        InitializeComponent();
    }

    public string TextFieldTitle
    {
        get => (string)GetValue(TextFieldTitleProperty);
        set => SetValue(TextFieldTitleProperty, value);
    }

    public string EntryText
    {
        get => (string)GetValue(EntryTextProperty);
        set => SetValue(EntryTextProperty, value);
    }

    public string EntryPlaceholder
    {
        get => (string)GetValue(EntryPlaceholderProperty);
        set => SetValue(EntryPlaceholderProperty, value);
    }

    public bool IsRequired
    {
        get => (bool)GetValue(IsRequiredProperty);
        set => SetValue(IsRequiredProperty, value);
    }

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }
}
