namespace MoneyFox.Win.Pages.Dialogs;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

public sealed partial class LoadingDialog : ContentDialog
{
    // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        name: "Text",
        propertyType: typeof(string),
        ownerType: typeof(LoadingDialog),
        typeMetadata: new(0));

    public LoadingDialog()
    {
        XamlRoot = MainWindow.RootFrame.XamlRoot;
        InitializeComponent();
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(dp: TextProperty, value: value);
    }
}
