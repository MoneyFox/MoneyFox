using Rg.Plugins.Popup.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.Views.Dialogs
{
    public partial class MessageDialog
    {
        public MessageDialog(string title, string message)
        {
            InitializeComponent();

            PopupTitle = title;
            PopupMessage = message;
        }

        public static readonly BindableProperty PopupTitleProperty = BindableProperty.Create(
            nameof(PopupTitle),
            typeof(string),
            typeof(MessageDialog),
            default(string));

        public static readonly BindableProperty PopupMessageProperty = BindableProperty.Create(
            nameof(PopupMessage),
            typeof(string),
            typeof(MessageDialog),
            default(string));

        public string PopupTitle
        {
            get => (string)GetValue(PopupTitleProperty);
            set => SetValue(PopupTitleProperty, value);
        }

        public string PopupMessage
        {
            get => (string)GetValue(PopupMessageProperty);
            set => SetValue(PopupMessageProperty, value);
        }

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