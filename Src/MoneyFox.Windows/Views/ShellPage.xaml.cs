using System;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using MvvmCross.Core.Navigation.EventArguments;

namespace MoneyFox.Windows.Views
{
    /// <summary>
    ///     The basic frame for the windows app
    /// </summary>
    public sealed partial class ShellPage : Page
    {
        public Frame Frame => ShellFrame;

        private ShellViewModel viewModel;

        public ShellViewModel ViewModel
        {
            get => viewModel;
            set => viewModel = value;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public ShellPage()
        {
            InitializeComponent();

            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.BackRequested += SystemNavigationManager_BackRequested;
            //start with a hidden back button. This changes when you navigate to an other page
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }
        
        private void SystemNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            var handled = e.Handled;
            BackRequested(ref handled);
            e.Handled = handled;
        }

        private void BackRequested(ref bool handled)
        {
            // Get a hold of the current Frame so that we can inspect the app back stack.
            if (Frame == null)
            {
                return;
            }

            // Check to see if this is the top-most page on the app back stack.
            if (Frame.CanGoBack && !handled)
            {
                // If not, set the event to handled and go back to the previous page in the app.
                handled = true;
                Frame.GoBack();

                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    Frame.CanGoBack ?
                        AppViewBackButtonVisibility.Visible :
                        AppViewBackButtonVisibility.Collapsed;
            }
        }

        /// <summary>
        ///     Default keyboard focus movement for any unhandled keyboarding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppShell_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            var direction = FocusNavigationDirection.None;
            switch (e.Key)
            {
                case VirtualKey.Escape:
                    var temp = false;
                    BackRequested(ref temp);
                    break;
            }

            if (direction != FocusNavigationDirection.None)
            {
                var control = FocusManager.FindNextFocusableElement(direction) as Control;
                if (control == null) return;

                control.Focus(FocusState.Programmatic);
                e.Handled = true;
            }
        }

        private void ShellPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // we have to update the color here, since it is not a dependency property.
            NavigationMenu.PaneForeground = ViewModel.MenuButtonColor;
        }

        /// <summary>
        ///     Adjusts the view for login.
        /// </summary>
        public void SetLoginView()
        {
            NavigationMenu.OpenPaneLength = 0;
        }

        /// <summary>
        ///     Adjusts the view for the general usage.
        /// </summary>
        public void SetLoggedInView()
        {
            NavigationMenu.OpenPaneLength = 200;
        }
    }
}
