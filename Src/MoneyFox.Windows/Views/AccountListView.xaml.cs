using Windows.UI.Xaml.Controls;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;

namespace MoneyFox.Windows.Views
{
    public sealed partial class AccountListView : IViewFor<AccountListViewModel>
    {
        public AccountListView() {
            this.InitializeComponent();
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (AccountListViewModel)value;
        }

        public AccountListViewModel ViewModel { get; set; }
    }
}
