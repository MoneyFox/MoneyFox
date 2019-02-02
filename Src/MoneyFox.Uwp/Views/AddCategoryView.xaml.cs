using Windows.UI.Xaml;
using MoneyFox.Presentation.Views;
using Xamarin.Forms.Platform.UWP;

namespace MoneyFox.Uwp.Views
{ 
    public sealed partial class AddCategoryView
    {
        public AddCategoryView()
        {
            InitializeComponent();
        }

        private void AddCategoryView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new AddCategoryPage { DataContext = ViewModel }.CreateFrameworkElement());
        }
    }
}
