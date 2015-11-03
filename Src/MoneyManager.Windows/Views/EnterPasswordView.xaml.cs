using System;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Cirrious.CrossCore;
using MoneyManager.Core.Authentication;
using MoneyManager.Core.ViewModels;
using MoneyManager.Localization;

namespace MoneyManager.Windows.Views
{
    public sealed partial class EnterPasswordView
    {
        public EnterPasswordView()
        {
            InitializeComponent();

            DataContext = Mvx.Resolve<EnterPasswordViewModel>();
        }

        //TODO: Refactor this to View Model. But before that we have to create an own Navigationservice to work with the appshell.
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Login();
        }

        private async Task Login()
        {
            if (!Mvx.Resolve<PasswordStorage>().ValidatePassword(PasswordBox.Password))
            {
                await new MessageDialog(Strings.PasswordWrongMessage, Strings.PasswordWrongTitle).ShowAsync();
                return;
            }

            Frame.Navigate(typeof (MainView));
            Frame.BackStack.Clear();
        }

        private async void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                await Login();
            }
        }
    }
}
