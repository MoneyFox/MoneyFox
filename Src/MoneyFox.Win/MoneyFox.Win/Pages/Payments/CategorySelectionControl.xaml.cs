namespace MoneyFox.Win.Pages.Payments;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ViewModels.Payments;

public sealed partial class CategorySelectionControl : UserControl
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        name: "ViewModel",
        propertyType: typeof(ModifyPaymentViewModel),
        ownerType: typeof(CategorySelectionControl),
        typeMetadata: new(null));

    public CategorySelectionControl()
    {
        InitializeComponent();
    }

    public ModifyPaymentViewModel ViewModel
    {
        get => (ModifyPaymentViewModel)GetValue(ViewModelProperty);
        set => SetValue(dp: ViewModelProperty, value: value);
    }
}
