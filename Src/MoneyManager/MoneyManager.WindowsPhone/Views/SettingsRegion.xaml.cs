#region

using Windows.UI.Xaml.Navigation;
using MoneyManager.Common;

#endregion

namespace MoneyManager.Views
{
    public sealed partial class SettingsRegion
    {
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();

        public SettingsRegion()
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