using Windows.UI.Xaml.Navigation;
using MoneyManager.Common;

namespace MoneyManager.Views
{
    public sealed partial class SettingsRegion
    {
        public SettingsRegion()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
        }

        private NavigationHelper NavigationHelper { get; }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion NavigationHelper registration
    }
}