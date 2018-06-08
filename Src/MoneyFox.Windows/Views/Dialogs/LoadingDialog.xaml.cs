using Windows.UI.Xaml.Controls;

namespace MoneyFox.Windows.Views.Dialogs
{
    public sealed partial class LoadingDialog : ContentDialog
    {
        public LoadingDialog()
        {
            InitializeComponent();
        }

        public string Text
        {
            get => LoadingText.Text;
            set => LoadingText.Text = value;
        }
    }
}