namespace MoneyFox.Views.Dialogs
{

    using System.Threading.Tasks;
    using Xamarin.CommunityToolkit.Extensions;
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
            await Application.Current.MainPage.Navigation.ShowPopupAsync(this);
        }
    }

}
