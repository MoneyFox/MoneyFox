using MoneyFox.Business.ViewModels;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Views
{
    public sealed partial class AccountListView
    {
        public AccountListView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<MainViewModel>();
        }
    }
}