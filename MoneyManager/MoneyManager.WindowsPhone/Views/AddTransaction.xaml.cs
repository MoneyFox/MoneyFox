using MoneyManager.Common;
using Windows.UI.Xaml;

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

        private void Done_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.GoBack();
        }
    }
}