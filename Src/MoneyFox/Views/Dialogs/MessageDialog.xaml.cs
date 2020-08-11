using Rg.Plugins.Popup.Extensions;
using System.Threading.Tasks;

namespace MoneyFox.Views.Dialogs
{
    public partial class MessageDialog
    {
        public MessageDialog(string title, string message)
        {
            InitializeComponent();

            Title = title;
            Message = message;
        }

        public new string Title { get; set; }

        public string Message { get; set; }

        public async Task ShowAsync()
        {
            await App.Current.MainPage.Navigation.PushPopupAsync(this);
        }

        public async Task DismissAsync()
        {
            await App.Current.MainPage.Navigation.PopPopupAsync();
        }

        private async void OnOkClick(object sender, System.EventArgs e)
        {
            await DismissAsync();
        }
    }
}