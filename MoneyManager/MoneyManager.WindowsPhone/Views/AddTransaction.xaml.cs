using MoneyManager.Common;
using Windows.UI.Xaml.Navigation;

namespace MoneyManager.Views
{
    public sealed partial class AddTransaction
    {
        private readonly NavigationHelper navigationHelper;

        public AddTransaction()
        {
            InitializeComponent();
            navigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }
    }
}