using Windows.UI.Xaml;
using MoneyFox.Presentation.Views;
using Xamarin.Forms.Platform.UWP;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class EditCategoryView
    {
        private readonly int categoryId;

        public EditCategoryView(int categoryId)
        {
            InitializeComponent();
            this.categoryId = categoryId;
        }

        private void EditCategoryView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new EditCategoryPage(categoryId) { BindingContext = ViewModel }.CreateFrameworkElement());
        }
    }
}
