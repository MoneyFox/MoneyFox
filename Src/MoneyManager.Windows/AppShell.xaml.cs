using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using MoneyManager.Localization;
using MoneyManager.Windows.Controls;
using MoneyManager.Windows.Views;

namespace MoneyManager.Windows
{
    /// <summary>
    ///     The "chrome" layer of the app that provides top-level navigation with
    ///     proper keyboarding navigation.
    /// </summary>
    public sealed partial class AppShell
    {
        public static AppShell Current;
        // Declare the top level nav items
        private readonly List<NavMenuItem> navlist = new List<NavMenuItem>(
            new[]
            {
                new NavMenuItem
                {
                    Symbol = Symbol.Library,
                    Label = Strings.AccountsLabel,
                    DestPage = typeof (MainView)
                },
                new NavMenuItem
                {
                    Symbol = Symbol.View,
                    Label = Strings.StatisticsLabel,
                    DestPage = typeof (StatisticsView)
                },
                new NavMenuItem
                {
                    Symbol = Symbol.SyncFolder,
                    Label = Strings.BackupLabel,
                    DestPage = typeof (BackupView)
                },
                new NavMenuItem
                {
                    Symbol = Symbol.Setting,
                    Label = Strings.SettingsLabel,
                    DestPage = typeof (SettingsView)
                },
                new NavMenuItem
                {
                    Symbol = Symbol.Account,
                    Label = Strings.AboutLabel,
                    DestPage = typeof (AboutView)
                }
            });

        /// <summary>
        ///     Initializes a new instance of the AppShell, sets the static 'Current' reference,
        ///     adds callbacks for Back requests and changes in the SplitView's DisplayMode, and
        ///     provide the nav menu list with the data to display.
        /// </summary>
        public AppShell()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                Current = this;

                TogglePaneButton.Focus(FocusState.Programmatic);
            };

            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.BackRequested += SystemNavigationManager_BackRequested;

            // If on a phone device that has hardware buttons then we hide the app's back button.
            currentView.AppViewBackButtonVisibility =
                ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons")
                    ? AppViewBackButtonVisibility.Collapsed
                    : AppViewBackButtonVisibility.Visible;

            NavMenuList.ItemsSource = navlist;
        }

        public Frame AppFrame => Frame;

        public Rect TogglePaneButtonRect { get; private set; }

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
                case VirtualKey.Left:
                case VirtualKey.GamepadDPadLeft:
                case VirtualKey.GamepadLeftThumbstickLeft:
                case VirtualKey.NavigationLeft:
                    direction = FocusNavigationDirection.Left;
                    break;
                case VirtualKey.Right:
                case VirtualKey.GamepadDPadRight:
                case VirtualKey.GamepadLeftThumbstickRight:
                case VirtualKey.NavigationRight:
                    direction = FocusNavigationDirection.Right;
                    break;

                case VirtualKey.Up:
                case VirtualKey.GamepadDPadUp:
                case VirtualKey.GamepadLeftThumbstickUp:
                case VirtualKey.NavigationUp:
                    direction = FocusNavigationDirection.Up;
                    break;

                case VirtualKey.Down:
                case VirtualKey.GamepadDPadDown:
                case VirtualKey.GamepadLeftThumbstickDown:
                case VirtualKey.NavigationDown:
                    direction = FocusNavigationDirection.Down;
                    break;

                case VirtualKey.Escape:
                case VirtualKey.B:
                    var temp = false;
                    BackRequested(ref temp);
                    break;
            }

            if (direction != FocusNavigationDirection.None)
            {
                var control = FocusManager.FindNextFocusableElement(direction) as Control;
                if (control != null)
                {
                    control.Focus(FocusState.Programmatic);
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        ///     An event to notify listeners when the hamburger button may occlude other content in the app.
        ///     The custom "PageHeader" user control is using this.
        /// </summary>
        public event TypedEventHandler<AppShell, Rect> TogglePaneButtonRectChanged;

        /// <summary>
        ///     Callback when the SplitView's Pane is toggled open or close.  When the Pane is not visible
        ///     then the floating hamburger may be occluding other content in the app unless it is aware.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TogglePaneButton_Checked(object sender, RoutedEventArgs e)
        {
            CheckTogglePaneButtonSizeChanged();
        }

        /// <summary>
        ///     Check for the conditions where the navigation pane does not occupy the space under the floating
        ///     hamburger button and trigger the event.
        /// </summary>
        private void CheckTogglePaneButtonSizeChanged()
        {
            if (RootSplitView.DisplayMode == SplitViewDisplayMode.Inline ||
                RootSplitView.DisplayMode == SplitViewDisplayMode.Overlay)
            {
                var transform = TogglePaneButton.TransformToVisual(this);
                var rect =
                    transform.TransformBounds(new Rect(0, 0, TogglePaneButton.ActualWidth, TogglePaneButton.ActualHeight));
                TogglePaneButtonRect = rect;
            }
            else
            {
                TogglePaneButtonRect = new Rect();
            }

            var handler = TogglePaneButtonRectChanged;
            // handler(this, this.TogglePaneButtonRect);
            handler?.DynamicInvoke(this, TogglePaneButtonRect);
        }

        /// <summary>
        ///     Enable accessibility on each nav menu item by setting the AutomationProperties.Name on each container
        ///     using the associated Label of each item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NavMenuItemContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (!args.InRecycleQueue && args.Item is NavMenuItem)
            {
                args.ItemContainer.SetValue(AutomationProperties.NameProperty, ((NavMenuItem) args.Item).Label);
            }
            else
            {
                args.ItemContainer.ClearValue(AutomationProperties.NameProperty);
            }
        }

        #region BackRequested Handlers

        private void SystemNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            var handled = e.Handled;
            BackRequested(ref handled);
            e.Handled = handled;
        }

        private void BackRequested(ref bool handled)
        {
            // Get a hold of the current Frame so that we can inspect the app back stack.

            if (AppFrame == null)
                return;

            // Check to see if this is the top-most page on the app back stack.
            if (AppFrame.CanGoBack && !handled)
            {
                // If not, set the event to handled and go back to the previous page in the app.
                handled = true;
                AppFrame.GoBack();
            }
        }

        #endregion

        #region Navigation

        /// <summary>
        ///     Navigate to the Page for the selected <paramref name="listViewItem" />.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="listViewItem"></param>
        private void NavMenuList_ItemInvoked(object sender, ListViewItem listViewItem)
        {
            var item = (NavMenuItem) ((NavMenuListView) sender).ItemFromContainer(listViewItem);

            if (item?.DestPage != null &&
                item.DestPage != AppFrame.CurrentSourcePageType)
            {
                AppFrame.Navigate(item.DestPage, item.Arguments);
            }
        }

        /// <summary>
        ///     Ensures the nav menu reflects reality when navigation is triggered outside of
        ///     the nav menu buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                var item = (from p in navlist where p.DestPage == e.SourcePageType select p).SingleOrDefault();
                if (item == null && AppFrame.BackStackDepth > 0)
                {
                    // In cases where a page drills into sub-pages then we'll highlight the most recent
                    // navigation menu item that appears in the BackStack
                    foreach (var entry in AppFrame.BackStack.Reverse())
                    {
                        item = (from p in navlist where p.DestPage == entry.SourcePageType select p).SingleOrDefault();
                        if (item != null)
                            break;
                    }
                }

                var container = (ListViewItem) NavMenuList.ContainerFromItem(item);

                // While updating the selection state of the item prevent it from taking keyboard focus.  If a
                // user is invoking the back button via the keyboard causing the selected nav menu item to change
                // then focus will remain on the back button.
                if (container != null) container.IsTabStop = false;
                NavMenuList.SetSelectedItem(container);
                if (container != null) container.IsTabStop = true;
            }
        }

        private void OnNavigatedToPage(object sender, NavigationEventArgs e)
        {
            // After a successful navigation set keyboard focus to the loaded page
            var page = e.Content as Page;
            if (page != null)
            {
                var control = page;
                control.Loaded += Page_Loaded;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ((Page) sender).Focus(FocusState.Programmatic);
            ((Page) sender).Loaded -= Page_Loaded;
            CheckTogglePaneButtonSizeChanged();

            if (SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility ==
                AppViewBackButtonVisibility.Collapsed)
            {
                RootSplitView.IsSwipeablePaneOpen = false;
            }
        }

        #endregion
    }
}