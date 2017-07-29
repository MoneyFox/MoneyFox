using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.Windows.Services;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Views
{
    /// <summary>
    ///     Login View.
    /// </summary>
    public sealed partial class LoginView
    {
        private readonly ShellPage shell;

        /// <summary>
        ///     Constructor
        /// </summary>
        public LoginView()
        {
            InitializeComponent();
            shell = Window.Current.Content as ShellPage;
            shell?.SetLoginView();
            Loaded += HideUnusedButtons;
        }

        /// <summary>
        ///     Hides the unused buttons
        /// </summary>
        public void HideUnusedButtons(object sender, RoutedEventArgs e)
        {
            var loginViewModel = ViewModel as LoginViewModel;
            if (loginViewModel != null && !loginViewModel.PassportEnabled)
            {
                PassportLogin.Visibility = Visibility.Collapsed;
            }

            var viewModel = ViewModel as LoginViewModel;
            if (viewModel != null && !viewModel.PasswordEnabled)
            {
                PasswordBox.Visibility = Visibility.Collapsed;
                LoginButton.Visibility = Visibility.Collapsed;
            }
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

            shell?.SetLoggedInView();
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
                    shell?.SetLoggedInView();
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