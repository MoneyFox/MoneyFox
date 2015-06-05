using Windows.UI.Xaml.Navigation;
using MoneyManager.Common;

namespace MoneyManager.Views
{
    public sealed partial class SettingsBackup
    {
        public SettingsBackup()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper { get; }

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