using System;
using Windows.UI.Xaml;
using Microsoft.Live;

namespace MoneyManager.UserControls
{
    public sealed partial class BackupUserControl
    {
        private LiveConnectClient liveClient;

        public BackupUserControl()
        {
            InitializeComponent();
        }

        private async void LoginToOneDrive(object sender, RoutedEventArgs e)
        {
            try
            {
                var authClient = new LiveAuthClient();
                LiveLoginResult result = await authClient.LoginAsync(new [] { "wl.signin", "wl.skydrive" });

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

        private void CreateBackup(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RestoreBackup(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
