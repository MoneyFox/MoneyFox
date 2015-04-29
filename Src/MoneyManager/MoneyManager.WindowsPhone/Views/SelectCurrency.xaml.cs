#region

using Windows.UI.Xaml.Navigation;
using MoneyManager.Common;

#endregion

namespace MoneyManager.Views {
    public sealed partial class SelectCurrency {
        private readonly NavigationHelper navigationHelper;

        public SelectCurrency() {
            InitializeComponent();

            navigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper {
            get { return navigationHelper; }
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}