using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.Common;

namespace MoneyManager.Views
{
    public sealed partial class AddTransaction
    {
        public AddTransaction()
        {
            InitializeComponent();
            NavigationHelper = new NavigationHelper(this);
        }

        private AddTransactionViewModel AddTransactionView
        {
            get { return ServiceLocator.Current.GetInstance<AddTransactionViewModel>(); }
        }

        public NavigationHelper NavigationHelper { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back && AddTransactionView.IsEdit)
            {
                //TODO:Refactor
                //await AccountLogic.RemoveTransactionAmount(AddTransactionView.SelectedTransaction);
            }

            base.OnNavigatedTo(e);
        }

        private void DoneClick(object sender, RoutedEventArgs e)
        {
            AddTransactionView.Save();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            AddTransactionView.Cancel();
        }
    }
}