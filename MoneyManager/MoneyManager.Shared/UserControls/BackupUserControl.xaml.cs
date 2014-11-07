using Windows.UI.Xaml;
using Microsoft.Live.Controls;

namespace MoneyManager.UserControls
{
    public sealed partial class BackupUserControl
    {
        public BackupUserControl()
        {
            InitializeComponent();

        }

        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void SignInButton_SessionChanged(object sender, LiveConnectSessionChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
