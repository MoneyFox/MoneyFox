namespace MoneyManager.Views
{
    public sealed partial class AddTransaction
    {
        public AddTransaction()
        {
            InitializeComponent();
            navigationHelper = new NavigationHelper(this);
        }

        #region NavigationHelper registration

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion NavigationHelper registration
    }
}