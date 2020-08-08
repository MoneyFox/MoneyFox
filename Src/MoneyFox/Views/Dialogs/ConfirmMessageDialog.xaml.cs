using System.Threading.Tasks;

namespace MoneyFox.Views.Dialogs
{
    public partial class ConfirmMessageDialog
    {
        public ConfirmMessageDialog(string title, string message, string positiveText = "", string negativeText = "")
        {
            InitializeComponent();

            Title = title;
            Message = message;
            PositiveText = positiveText;
            NegativeText = negativeText;
        }

        public new string Title { get; set; }

        public string Message { get; set; }
        public string PositiveText { get; set; }

        public string NegativeText { get; set; }

        private TaskCompletionSource<bool>? confirmTaskCompletionSource;

        public async Task<bool> ShowAsync()
        {
            confirmTaskCompletionSource = new TaskCompletionSource<bool>();
            await App.Current.MainPage.Navigation.PushModalAsync(this);
            return await confirmTaskCompletionSource.Task;
        }

        private async void PositiveHandlerClicked(object sender, System.EventArgs e)
        {
            await DismissAsync();
            confirmTaskCompletionSource?.SetResult(true);
        }

        private async void NegativeHandlerClicked(object sender, System.EventArgs e)
        {
            await DismissAsync();
            confirmTaskCompletionSource?.SetResult(false);
        }

        public async Task DismissAsync()
            => await App.Current.MainPage.Navigation.PopModalAsync();
    }
}