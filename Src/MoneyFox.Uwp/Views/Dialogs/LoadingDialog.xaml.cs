using Windows.UI.Xaml.Controls;

#nullable enable
namespace MoneyFox.Uwp.Views.Dialogs
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
