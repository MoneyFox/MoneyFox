using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Resources;
using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using MoneyFox.Windows.Views.UserControls;
using Windows.UI.Xaml.Media.Animation;

namespace MoneyFox.Windows.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView
    {
        private bool aboutStateEnabled;
        private bool settingStateEnabled;

        public MainView()
        {
            this.InitializeComponent();

            CoreApplicationViewTitleBar titleBar = CoreApplication.GetCurrentView().TitleBar;
            titleBar.LayoutMetricsChanged += TitleBar_LayoutMetricsChanged;

            // Dynamic change
            this.SizeChanged += new SizeChangedEventHandler(ChangeSpacerSize);


        }

        private void ChangeSpacerSize(object sender, SizeChangedEventArgs args)
        {
            var temp = (DoubleAnimation)SeperatorStoryboard.Children.ElementAt(0);
            temp.To = Convert.ToInt32(args.NewSize.Height) - 230 - 88;
            SeperatorStoryboard.Begin();
        }
       

        public Frame MainFrame => ContentFrame;

        private void TitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            AppTitle.Margin = new Thickness(CoreApplication.GetCurrentView().TitleBar.SystemOverlayLeftInset + 12, 8, 0, 0);
        }

        private async void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {

            if (args.IsSettingsInvoked)
            {
                await ((MainViewModel)ViewModel).ShowSettingsCommand.ExecuteAsync();
            }
            else
            {
                if (args.InvokedItem == null)
                {
                    return;
                }

                if (args.InvokedItem.Equals(Strings.AccountsTitle))
                {
                    await ((MainViewModel)ViewModel).ShowAccountListCommand.ExecuteAsync();
                }
                else if (args.InvokedItem.Equals(Strings.StatisticsTitle))
                {
                    await ((MainViewModel)ViewModel).ShowStatisticSelectorCommand.ExecuteAsync();
                }
                else if (args.InvokedItem.Equals(Strings.CategoriesTitle))
                {
                    await ((MainViewModel)ViewModel).ShowCategoryListCommand.ExecuteAsync();
                }
                else if (args.InvokedItem.Equals(Strings.BackupTitle))
                {
                    await ((MainViewModel)ViewModel).ShowBackupViewCommand.ExecuteAsync();
                }
                else if (args.InvokedItem.Equals(Strings.SettingsTitle))
                {
                    if (!settingStateEnabled)
                    {
                        ShowSettings.Begin();
                        settingStateEnabled = true;
                        settings.Focus(FocusState.Programmatic);
                    }
                }
                else if (args.InvokedItem.Equals(Strings.AboutTitle))
                {
                    if(!aboutStateEnabled)
                    {
                        ShowAbout.Begin();
                        aboutStateEnabled = true;
                        about.Focus(FocusState.Programmatic);
                    }
                }
            }
        }

        /// <summary>
        ///     Adjusts the view for login.
        /// </summary>
        public void SetLoginView()
        {
            NavView.OpenPaneLength = 0;
        }

        /// <summary>
        ///     Adjusts the view for the general usage.
        /// </summary>
        public void SetLoggedInView()
        {
            NavView.OpenPaneLength = 200;
        }

        private void AboutUserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            var tempFocus = FocusManager.GetFocusedElement();
            if (aboutStateEnabled && (sender != tempFocus) && (about.MailButton != tempFocus))
            {
                HideAbout.Begin();
                aboutStateEnabled = false;
            }
            
        }

        private void Settings_LostFocus(object sender, RoutedEventArgs e)
        {
            var tempFocus = FocusManager.GetFocusedElement();
            if (settingStateEnabled && (settings != tempFocus)
                && settings.Security.Password != tempFocus && settings.Security.PasswordConfirmation != tempFocus
                 && (settings.Personalization.ToggleSwitch != tempFocus))
            {
                HideSettings.Begin();
                settingStateEnabled = false;
            }
        }
    }
}
