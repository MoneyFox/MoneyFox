using Windows.UI.Xaml;
using MoneyFox.Business.Views;
using Xamarin.Forms.Platform.UWP;

namespace MoneyFox.Windows.Views
{
    public sealed partial class CategoryListView
    {
        public CategoryListView()
        {
            InitializeComponent();
        }

        private void CategoryListView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new CategoryListPage() { BindingContext = ViewModel }.CreateFrameworkElement());
        }
    }
}