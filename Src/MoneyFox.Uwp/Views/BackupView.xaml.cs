using Windows.UI.Xaml;
using MoneyFox.Presentation.Views;
using Xamarin.Forms.Platform.UWP;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class BackupView
    {
        public BackupView()
        {
            InitializeComponent();
        }

        private void BackupView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new BackupPage { BindingContext = DataContext }.CreateFrameworkElement());
        }
    }
}