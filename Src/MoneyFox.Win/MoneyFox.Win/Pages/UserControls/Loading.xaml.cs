namespace MoneyFox.Win.Pages.UserControls;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

public sealed partial class Loading : UserControl
{
    public Loading()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(
        "IsLoading",
        typeof(bool),
        typeof(Loading),
        new PropertyMetadata(false)
    );

    public bool IsLoading
    {
        get => (bool)GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }
}