namespace MoneyFox.Ui.Controls;

using System.Windows.Input;

public partial class Search : ContentView
{
    public static readonly BindableProperty SearchPlaceholderProperty = BindableProperty.Create(
        propertyName: nameof(SearchPlaceholder),
        returnType: typeof(string),
        declaringType: typeof(Search),
        defaultValue: string.Empty);

    public static readonly BindableProperty SearchCommandProperty = BindableProperty.Create(
        propertyName: nameof(SearchCommand),
        returnType: typeof(ICommand),
        declaringType: typeof(Search));

    public Search()
    {
        InitializeComponent();
    }

    public string SearchPlaceholder
    {
        get => (string)GetValue(SearchPlaceholderProperty);
        set => SetValue(property: SearchPlaceholderProperty, value: value);
    }

    public ICommand SearchCommand
    {
        get => (ICommand)GetValue(SearchCommandProperty);
        set => SetValue(property: SearchCommandProperty, value: value);
    }

    private void SearchEntry_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        SearchCommand.Execute(e.NewTextValue);
    }
}
