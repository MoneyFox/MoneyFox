namespace MoneyFox.Views.Dialogs
{

    using System.Threading.Tasks;
    using Rg.Plugins.Popup.Extensions;
    using Xamarin.Forms;

    public partial class LoadingDialog
    {
        public LoadingDialog()
        {
            InitializeComponent();
        }

        internal static async Task<LoadingDialog> LoadingAsync()
        {
            var dialog = new LoadingDialog();
            await dialog.ShowAsync();

            return dialog;
        }

        public async Task ShowAsync()
        {
            await Application.Current.MainPage.Navigation.PushPopupAsync(this);
        }

        public static async Task DismissAsync()
        {
            await Application.Current.MainPage.Navigation.PopPopupAsync();
        }
    }

}
