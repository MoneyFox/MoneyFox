using System;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.ServiceLocation;
using MoneyFox.Foundation.Resources;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Windows.Views
{
    public sealed partial class LoginView
    {
        public LoginView()
        {
            InitializeComponent();
        }

        //TODO: Refactor this to View Model. But before that we have to create an own Navigationservice to work with the appshell.
        private async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            await Login();
        }

        private async void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                await Login();
            }
        }

        private async Task Login()
        {
            if (!ServiceLocator.Current.GetInstance<IPasswordStorage>().ValidatePassword(PasswordBox.Password))
            {
                await new MessageDialog(Strings.PasswordWrongMessage, Strings.PasswordWrongTitle).ShowAsync();
                return;
            }

            (Window.Current.Content as AppShell)?.SetLoggedInView();

            Frame.Navigate(typeof (MainView));
            Frame.BackStack.Clear();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                e.Cancel = true;
            }

            base.OnNavigatingFrom(e);
        }
    }
}