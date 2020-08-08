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

        public string Title { get; set; }
        public string Message { get; set; }

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