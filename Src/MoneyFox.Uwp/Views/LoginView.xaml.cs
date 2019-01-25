using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.ViewModels;
using MoneyFox.Uwp.Services;
using MvvmCross;

namespace MoneyFox.Uwp.Views
{
    /// <summary>
    ///     Login View.
    /// </summary>
    public sealed partial class LoginView
    {
        private readonly MainView shell;

        /// <summary>
        ///     Constructor
        /// </summary>
        public LoginView()
        {
            InitializeComponent();
            shell = Window.Current.Content as MainView;
            shell?.SetLoginView();
            Loaded += HideUnusedButtons;
        }

        /// <summary>
        ///     Hides the unused buttons
        /// </summary>
        public void HideUnusedButtons(object sender, RoutedEventArgs e)
        {
            if (ViewModel is LoginViewModel loginViewModel && !loginViewModel.PassportEnabled)
            {
                PassportLogin.Visibility = Visibility.Collapsed;
            }

            if (ViewModel is LoginViewModel viewModel && !viewModel.PasswordEnabled)
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
            if (!Mvx.IoCProvider.CanResolve<IPasswordStorage>()) return;
            if (!Mvx.IoCProvider.Resolve<IPasswordStorage>().ValidatePassword(PasswordBox.Password))
            {
                Mvx.IoCProvider.Resolve<IDialogService>().ShowMessage(Strings.PasswordWrongTitle, Strings.PasswordWrongMessage);
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
            if (((LoginViewModel) ViewModel).PassportEnabled)
            {
                if (await MicrosoftPassportHelper.CreatePassportKeyAsync())
                {
                    shell?.SetLoggedInView();
                    ((LoginViewModel) ViewModel)?.LoginNavigationCommand.Execute();
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