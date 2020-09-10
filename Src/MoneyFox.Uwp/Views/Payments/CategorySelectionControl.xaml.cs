using MoneyFox.Uwp.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoneyFox.Uwp.Views.Payments
{
    public sealed partial class CategorySelectionControl : UserControl
    {
        public CategorySelectionControl()
        {
            InitializeComponent();
        }

        public ModifyPaymentViewModel ViewModel
        {
            get { return (ModifyPaymentViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel",
                                                                                                  typeof(ModifyPaymentViewModel),
                                                                                                  typeof(CategorySelectionControl),
                                                                                                  new PropertyMetadata(null));
    }
}
