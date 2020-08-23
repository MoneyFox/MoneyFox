using Xamarin.Forms;

namespace MoneyFox.Views.Accounts
{
    public partial class ModifyAccountContentView : ContentView
    {
        public ModifyAccountContentView()
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