using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
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

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back && AddTransactionView.IsEdit)
            {
                await AccountLogic.RemoveTransactionAmount(AddTransactionView.SelectedTransaction);
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