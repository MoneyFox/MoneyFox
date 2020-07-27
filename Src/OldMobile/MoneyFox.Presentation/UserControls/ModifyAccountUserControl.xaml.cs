using Xamarin.Forms;

namespace MoneyFox.Presentation.UserControls
{
    public partial class ModifyAccountUserControl
    {
        public ModifyAccountUserControl()
        {
            InitializeComponent();
        }

#pragma warning disable CRR0026 // Unused member; Used by UI
        private void AmountFieldGotFocused(object sender, FocusEventArgs e)
        {
            AmountEntry.Text = string.Empty;
        }
#pragma warning restore CRR0026 // Unused member
    }
}
