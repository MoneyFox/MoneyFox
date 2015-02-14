#region

using Windows.UI.Xaml.Navigation;
using MoneyManager.Common;

#endregion

namespace MoneyManager.Views {
    public sealed partial class SettingsTiles {
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly NavigationHelper navigationHelper;

        public SettingsTiles() {
            InitializeComponent();

            navigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper {
            get { return navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel {
            get { return defaultViewModel; }
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion NavigationHelper registration
    }
}