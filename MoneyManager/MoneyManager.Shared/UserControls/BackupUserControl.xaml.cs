using Windows.UI.Xaml;
using Microsoft.Live;

namespace MoneyManager.UserControls
{
    public sealed partial class BackupUserControl
    {
        public BackupUserControl()
        {
            InitializeComponent();

        }

        private LiveConnectClient liveClient;

        private async void connectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var authClient = new LiveAuthClient();
                LiveLoginResult result = await authClient.LoginAsync(new string[] { "wl.signin", "wl.skydrive" });

                if (result.Status == LiveConnectSessionStatus.Connected)
                {
                    liveClient = new LiveConnectClient(result.Session);
                }
            }
            catch (LiveAuthException ex)
            {
                // Display an error message.
            }
            catch (LiveConnectException ex)
            {
                // Display an error message.
            }
        }

        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
