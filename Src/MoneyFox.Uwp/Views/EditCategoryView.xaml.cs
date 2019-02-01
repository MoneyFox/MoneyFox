using Windows.UI.Xaml;
using MoneyFox.Presentation.Views;
using Xamarin.Forms.Platform.UWP;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class EditCategoryView
    {
        public EditCategoryView()
        {
            InitializeComponent();
        }

        private void EditCategoryView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new EditCategoryPage { DataContext = ViewModel }.CreateFrameworkElement());
        }
    }
}
