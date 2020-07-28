using Xamarin.Forms;

namespace MoneyFox.Views.Payments
{
    public partial class ModifyPaymentContentView : ContentView
    {
        public ModifyPaymentContentView()
        {
            InitializeComponent();
        }

        private void AmountFieldGotFocus(object sender, FocusEventArgs e)
        {
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                AmountEntry.CursorPosition = 0;
                AmountEntry.SelectionLength = AmountEntry.Text != null ? AmountEntry.Text.Length : 0;
            });
        }
    }
}