namespace MoneyManager.Windows.Views
{
    public sealed partial class PaymentListView
    {
        public PaymentListView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<PaymentListViewModel>();
        }
    }
}