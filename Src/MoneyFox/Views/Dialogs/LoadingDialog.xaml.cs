using System.Threading.Tasks;

namespace MoneyFox.Views.Dialogs
{
    public partial class LoadingDialog
    {
        public LoadingDialog()
        {
            InitializeComponent();
        }

        internal static async Task<LoadingDialog> Loading()
        {
            var dialog = new LoadingDialog();
            await dialog.ShowAsync();

            return dialog;
        }

        public async Task ShowAsync()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(this);
        }

        public async Task DismissAsync()
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}