using Windows.UI.Xaml.Navigation;
using MoneyManager.Common;

namespace MoneyManager.Views {
    public sealed partial class SettingsBackup {
        private readonly NavigationHelper _navigationHelper;

        public SettingsBackup() {
            InitializeComponent();

            _navigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper {
            get { return _navigationHelper; }
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            _navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            _navigationHelper.OnNavigatedFrom(e);
        }

        #endregion NavigationHelper registration
    }
}