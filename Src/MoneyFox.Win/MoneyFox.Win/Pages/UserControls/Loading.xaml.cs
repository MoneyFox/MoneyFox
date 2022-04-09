namespace MoneyFox.Win.Pages.UserControls;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

public sealed partial class Loading : UserControl
{
    public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(
        name: "IsLoading",
        propertyType: typeof(bool),
        ownerType: typeof(Loading),
        typeMetadata: new(false));

    public Loading()
    {
        InitializeComponent();
    }

    public bool IsLoading
    {
        get => (bool)GetValue(IsLoadingProperty);
        set => SetValue(dp: IsLoadingProperty, value: value);
    }
}
