using MoneyManager.Common;
using Windows.UI.Xaml.Navigation;

namespace MoneyManager.Views
{
    public sealed partial class AddAccount
    {
        private readonly NavigationHelper navigationHelper;

        public AddAccount()
        {
            InitializeComponent();
            navigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }
    }
}