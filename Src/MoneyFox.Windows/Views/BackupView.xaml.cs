using Windows.UI.Xaml;
using MoneyFox.Business.Views;
using Xamarin.Forms.Platform.UWP;

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
            ContentGrid.Children.Add(new BackupPage{BindingContext = ViewModel}.CreateFrameworkElement());
        }
    }
}