using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Windows.Views
{
    public sealed partial class TransactionListView
    {
        public TransactionListView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<TransactionListViewModel>();
        }
    }
}