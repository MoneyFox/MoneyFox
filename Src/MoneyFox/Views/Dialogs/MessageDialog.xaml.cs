using Rg.Plugins.Popup.Extensions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.Views.Dialogs
{
    public partial class MessageDialog
    {
        public static readonly BindableProperty PopupTitleProperty = BindableProperty.Create(
            nameof(PopupTitle),
            typeof(string),
            typeof(MessageDialog));

        public static readonly BindableProperty PopupMessageProperty = BindableProperty.Create(
            nameof(PopupMessage),
            typeof(string),
            typeof(MessageDialog));

        public MessageDialog(string title, string message)
        {
            InitializeComponent();

            PopupTitle = title;
            PopupMessage = message;
        }

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

        public async Task ShowAsync() =>
            await Application.Current.MainPage.Navigation.PushPopupAsync(this);

        public static async Task DismissAsync() =>
            await Application.Current.MainPage.Navigation.PopPopupAsync();

        private async void OnOkClick(object sender, EventArgs e) => await DismissAsync();
    }
}