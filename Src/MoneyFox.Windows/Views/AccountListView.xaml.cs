using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;
using Splat;

namespace MoneyFox.Windows.Views
{
    public sealed partial class AccountListView : IViewFor<AccountListViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
            .Register(nameof(ViewModel), typeof(AccountListViewModel), typeof(AccountListView), null);


        public AccountListView() {
            this.InitializeComponent();

            ViewModel = Locator.Current.GetService<AccountListViewModel>();
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (AccountListViewModel)value;
        }

        public AccountListViewModel ViewModel { get; set; }
    }
}
