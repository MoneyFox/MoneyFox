namespace MoneyFox.Views.Dialogs
{

    using System;
    using System.Threading.Tasks;

    public partial class MessageDialog
    {
        public static readonly BindableProperty PopupTitleProperty = BindableProperty.Create(
            propertyName: nameof(PopupTitle),
            returnType: typeof(string),
            declaringType: typeof(MessageDialog));

        public static readonly BindableProperty PopupMessageProperty = BindableProperty.Create(
            propertyName: nameof(PopupMessage),
            returnType: typeof(string),
            declaringType: typeof(MessageDialog));

        public MessageDialog(string title, string message)
        {
            InitializeComponent();
            PopupTitle = title;
            PopupMessage = message;
        }

        public string PopupTitle
        {
            get => (string)GetValue(PopupTitleProperty);
            set => SetValue(property: PopupTitleProperty, value: value);
        }

        public string PopupMessage
        {
            get => (string)GetValue(PopupMessageProperty);
            set => SetValue(property: PopupMessageProperty, value: value);
        }

        public async Task ShowAsync()
        {
            await Application.Current.MainPage.Navigation.PushPopupAsync(this);
        }

        public static async Task DismissAsync()
        {
            await Application.Current.MainPage.Navigation.PopPopupAsync();
        }

        private async void OnOkClick(object sender, EventArgs e)
        {
            await DismissAsync();
        }
    }

}
