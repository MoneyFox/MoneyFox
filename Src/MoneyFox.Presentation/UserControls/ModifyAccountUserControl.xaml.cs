using Xamarin.Forms;

namespace MoneyFox.Presentation.UserControls
{
    public partial class ModifyAccountUserControl
    {
        public ModifyAccountUserControl()
        {
            InitializeComponent();
        }

        private void AmountFieldGotFocused(object sender, FocusEventArgs e)
        {
            AmountEntry.Text = string.Empty;
        }
    }
}