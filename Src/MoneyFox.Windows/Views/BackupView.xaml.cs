using Windows.UI.Xaml;
using Xamarin.Forms.Platform.UWP;
using MoneyFox.Views;

namespace MoneyFox.Windows.Views
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
            ContentGrid.Children.Add(new BackupPage { DataContext = ViewModel }.CreateFrameworkElement());
        }
    }
}