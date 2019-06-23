using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using MoneyFox.Presentation.Views;
using Xamarin.Forms.Platform.UWP;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class EditCategoryView
    {
        private int categoryId;

        public EditCategoryView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null) categoryId = (int)e.Parameter;
        }

        private void EditCategoryView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new EditCategoryPage(categoryId) { BindingContext = DataContext }.CreateFrameworkElement());
        }
    }
}
