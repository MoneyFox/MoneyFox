namespace MoneyFox.Win.Pages.Payments;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ViewModels.Payments;

public sealed partial class CategorySelectionControl : UserControl
{
    public CategorySelectionControl()
    {
        InitializeComponent();
    }

    public ModifyPaymentViewModel ViewModel
    {
        get => (ModifyPaymentViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        "ViewModel",
        typeof(ModifyPaymentViewModel),
        typeof(CategorySelectionControl),
        new PropertyMetadata(null));
}