namespace MoneyFox.Views.Payments
{

    using Xamarin.Forms;

    public partial class ModifyPaymentContentView
    {
        public ModifyPaymentContentView()
        {
            InitializeComponent();
        }

        private void AmountFieldGotFocus(object sender, FocusEventArgs e)
        {
            Dispatcher.BeginInvokeOnMainThread(
                () =>
                {
                    AmountEntry.CursorPosition = 0;
                    AmountEntry.SelectionLength = AmountEntry.Text != null ? AmountEntry.Text.Length : 0;
                });
        }
    }

}
