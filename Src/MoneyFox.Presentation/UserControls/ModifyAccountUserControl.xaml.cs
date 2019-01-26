using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.UserControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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