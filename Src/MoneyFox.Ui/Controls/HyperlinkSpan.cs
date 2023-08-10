namespace MoneyFox.Ui.Controls;

public class HyperlinkSpan : Span
{
    public static readonly BindableProperty UrlProperty = BindableProperty.Create(
        propertyName: nameof(Url),
        returnType: typeof(string),
        declaringType: typeof(HyperlinkSpan));

    public HyperlinkSpan()
    {
        TextDecorations = TextDecorations.Underline;
        TextColor = Colors.Blue;
        GestureRecognizers.Add(
            new TapGestureRecognizer
            {
                // Launcher.OpenAsync is provided by Essentials.
                Command = new Command(async () => await Launcher.OpenAsync(Url))
            });
    }

    public string Url
    {
        get => (string)GetValue(UrlProperty);
        set => SetValue(property: UrlProperty, value: value);
    }
}
