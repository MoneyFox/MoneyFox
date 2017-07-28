using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using FontAwesome.UWP;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.Navigation.EventArguments;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Windows
{
    /// <summary>
    ///     ViewModel for the ShellPage.
    /// </summary>
    public class ShellViewModel : MvxViewModel
    {
        private const string PANORAMIC_STATE_NAME = "PanoramicState";
        private const string WIDE_STATE_NAME = "WideState";
        private const string NARROW_STATE_NAME = "NarrowState";
        private const double WIDE_STATE_MIN_WINDOW_WIDTH = 640;
        private const double PANORAMIC_STATE_MIN_WINDOW_WIDTH = 1024;

        public readonly IMvxNavigationService NavigationService;
        private readonly ISettingsManager settings;

        private SplitViewDisplayMode displayMode = SplitViewDisplayMode.CompactInline;

        private ObservableCollection<ShellNavigationItem> primaryItems =
            new ObservableCollection<ShellNavigationItem>();

        private ObservableCollection<ShellNavigationItem> secondaryItems =
            new ObservableCollection<ShellNavigationItem>();

        private IMvxCommand stateChangedCommand;

        private bool isPaneOpen;

        private IMvxCommand itemSelected;

        private object lastSelectedItem;

        private Brush menuButtonColor;

        private IMvxCommand openPaneCommand;

        public ShellViewModel(IMvxNavigationService navigationService, ISettingsManager settings)
        {
            this.NavigationService = navigationService;
            this.settings = settings;

            PopulateNavItems();
            InitializeState(Window.Current.Bounds.Width);
            navigationService.AfterNavigate += NavigationServiceOnAfterNavigate;
        }

        private void PopulateNavItems()
        {
            primaryItems.Clear();
            secondaryItems.Clear();

            primaryItems.Add(ShellNavigationItem.FromType<AccountListViewModel>(Strings.AccountsLabel, FontAwesomeIcon.University));
            primaryItems.Add(ShellNavigationItem.FromType<StatisticSelectorViewModel>(Strings.StatisticsLabel, FontAwesomeIcon.BarChart));

            secondaryItems.Add(ShellNavigationItem.FromType<CategoryListViewModel>(Strings.CategoriesLabel, FontAwesomeIcon.Tags));
            secondaryItems.Add(ShellNavigationItem.FromType<BackupViewModel>(Strings.BackupLabel, FontAwesomeIcon.CloudUpload));
            secondaryItems.Add(ShellNavigationItem.FromType<SettingsViewModel>(Strings.SettingsLabel, FontAwesomeIcon.Cog));
            secondaryItems.Add(ShellNavigationItem.FromType<AboutViewModel>(Strings.AboutLabel, FontAwesomeIcon.InfoCircle));
        }

        public bool IsPaneOpen
        {
            get => isPaneOpen;
            set
            {
                if (isPaneOpen == value) return;

                isPaneOpen = value;
                RaisePropertyChanged();
            }
        }

        public Brush MenuButtonColor
        {
            get => menuButtonColor;
            set
            {
                if (menuButtonColor == value) return;

                menuButtonColor = value;
                RaisePropertyChanged();
            }
        }

        public SplitViewDisplayMode DisplayMode
        {
            get => displayMode;
            set
            {
                if (displayMode == value) return;

                displayMode = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ShellNavigationItem> PrimaryItems
        {
            get => primaryItems;
            set
            {
                if (primaryItems == value) return;

                primaryItems = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ShellNavigationItem> SecondaryItems
        {
            get => secondaryItems;
            set
            {
                if (secondaryItems == value) return;

                secondaryItems = value;
                RaisePropertyChanged();
            }
        }

        private Visibility isBackButtonVisible;
        public Visibility IsBackButtonVisible
        {
            get => isBackButtonVisible;
            set
            {
                if (isBackButtonVisible == value) return;

                isBackButtonVisible = value;
                RaisePropertyChanged();
            }
        } 
        public IMvxCommand OpenPaneCommand
        {
            get
            {
                if (openPaneCommand == null)
                {
                    openPaneCommand = new MvxCommand(() => IsPaneOpen = !isPaneOpen);
                }

                return openPaneCommand;
            }
        }

        public IMvxCommand ItemSelectedCommand
        {
            get
            {
                if (itemSelected == null)
                {
                    itemSelected = new MvxCommand<ItemClickEventArgs>(ItemSelected);
                }

                return itemSelected;
            }
        }

        public IMvxCommand StateChangedCommand
        {
            get
            {
                if (stateChangedCommand == null)
                {
                    stateChangedCommand =
                        new MvxCommand<VisualStateChangedEventArgs>(args => GoToState(args.NewState.Name));
                }

                return stateChangedCommand;
            }
        }

        private void InitializeState(double windowWith)
        {
            if (windowWith < WIDE_STATE_MIN_WINDOW_WIDTH)
            {
                GoToState(NARROW_STATE_NAME);
            }
            else if (windowWith < PANORAMIC_STATE_MIN_WINDOW_WIDTH)
            {
                GoToState(WIDE_STATE_NAME);
            }
            else
            {
                GoToState(PANORAMIC_STATE_NAME);
            }

            primaryItems.First().IsSelected = true;
            lastSelectedItem = primaryItems.First();
        }

        private void GoToState(string stateName)
        {
            switch (stateName)
            {
                case PANORAMIC_STATE_NAME:
                    DisplayMode = SplitViewDisplayMode.CompactInline;
                    MenuButtonColor = new SolidColorBrush(Colors.White);
                    break;
                case WIDE_STATE_NAME:
                    DisplayMode = SplitViewDisplayMode.CompactInline;
                    IsPaneOpen = false;
                    MenuButtonColor = new SolidColorBrush(Colors.White);
                    break;
                case NARROW_STATE_NAME:
                    DisplayMode = SplitViewDisplayMode.Overlay;
                    IsPaneOpen = false;
                    MenuButtonColor = settings.Theme == AppTheme.Light
                        ? new SolidColorBrush(Colors.Black)
                        : new SolidColorBrush(Colors.White);
                    break;
                default:
                    break;
            }
        }

        private void ItemSelected(ItemClickEventArgs args)
        {
            if (DisplayMode == SplitViewDisplayMode.CompactOverlay || DisplayMode == SplitViewDisplayMode.Overlay)
            {
                IsPaneOpen = false;
            }
            Navigate(args.ClickedItem);
        }

        private void ChangeSelected(object oldValue, object newValue)
        {
            if (oldValue != null)
            {
                (oldValue as ShellNavigationItem).IsSelected = false;
            }
            if (newValue != null)
            {
                (newValue as ShellNavigationItem).IsSelected = true;
            }
        }

        private async void Navigate(object item)
        {
            var navigationItem = item as ShellNavigationItem;
            if (navigationItem == null) return;
            if (navigationItem.ViewModel == typeof(AccountListViewModel))
            {
                await NavigationService.Navigate<AccountListViewModel>();
            }
            else if (navigationItem.ViewModel == typeof(StatisticSelectorViewModel))
            {
                await NavigationService.Navigate<StatisticSelectorViewModel>();
            }
            else if (navigationItem.ViewModel == typeof(CategoryListViewModel))
            {
                await NavigationService.Navigate<CategoryListViewModel>();
            }
            else if (navigationItem.ViewModel == typeof(BackupViewModel))
            {
                await NavigationService.Navigate<BackupViewModel>();
            }
            else if (navigationItem.ViewModel == typeof(SettingsViewModel))
            {
                await NavigationService.Navigate<SettingsViewModel>();
            }
            else if (navigationItem.ViewModel == typeof(AboutViewModel))
            {
                await NavigationService.Navigate<AboutViewModel>();
            }
        }

        private void NavigationServiceOnAfterNavigate(object sender, NavigateEventArgs navigateEventArgs)
        {
            var navigationItem = PrimaryItems?.FirstOrDefault(i => i.ViewModel == navigateEventArgs?.ViewModel.GetType());
            if (navigationItem == null)
            {
                navigationItem = SecondaryItems?.FirstOrDefault(i => i.ViewModel == navigateEventArgs?.ViewModel.GetType());
            }

            if (navigationItem != null)
            {
                ChangeSelected(lastSelectedItem, navigationItem);
                lastSelectedItem = navigationItem;
            }
        }
    }
}