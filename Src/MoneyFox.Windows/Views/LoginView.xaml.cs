using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Platform;
using MoneyFox.Windows.Services;

namespace MoneyFox.Windows.Views
{
    public sealed partial class LoginView
    {
        private readonly AppShell appShell;


        public void hideButton(object sender, RoutedEventArgs e)
        {
            if (!((ViewModel as LoginViewModel).PassportEnabled))
            {
                PassportLogin.Visibility = Visibility.Collapsed;
            }

            if (!((ViewModel as LoginViewModel).PasswordEnabled))
            {
                PasswordBox.Visibility = Visibility.Collapsed;
                LoginButton.Visibility = Visibility.Collapsed;
            }
        }

        public LoginView()
        {
            InitializeComponent();
            appShell = Window.Current.Content as AppShell;
            appShell?.SetLoginView();
            Loaded+=new RoutedEventHandler(hideButton);
        }
        
        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                Login();
            }
        }

        private void Login()
        {
            if (!Mvx.Resolve<IPasswordStorage>().ValidatePassword(PasswordBox.Password))
            {
                Mvx.Resolve<IDialogService>().ShowMessage(Strings.PasswordWrongTitle, Strings.PasswordWrongMessage);
                return;
            }

            appShell?.SetLoggedInView();
            (ViewModel as LoginViewModel)?.LoginNavigationCommand.Execute();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                e.Cancel = true;
            }

            base.OnNavigatingFrom(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Frame.BackStack.Clear();
        }

        private async void SignInPassport()
        {
            if ((ViewModel as LoginViewModel).PassportEnabled)
            {
                if (await MicrosoftPassportHelper.CreatePassportKeyAsync())
                {
                    appShell?.SetLoggedInView();
                    (ViewModel as LoginViewModel)?.LoginNavigationCommand.Execute();
                }
            }
            else
            {
                PassportLogin.Visibility = Visibility.Collapsed;
            }
            
        }

        private void PassportSignInButton_Click(object sender, RoutedEventArgs e)
        {
            SignInPassport();
        }       
    }
}