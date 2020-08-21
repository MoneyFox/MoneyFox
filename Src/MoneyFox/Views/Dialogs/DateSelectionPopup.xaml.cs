using MoneyFox.ViewModels.Dialogs;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Dialogs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DateSelectionPopup
    {
        public DateSelectionPopup()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.SelectDateRangeDialogViewModel;
        }

        public async Task ShowAsync() => await App.Current.MainPage.Navigation.PushPopupAsync(this);

        public async Task DismissAsync() => await App.Current.MainPage.Navigation.PopPopupAsync();

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            (BindingContext as SelectDateRangeDialogViewModel)?.DoneCommand.Execute(null);
            await DismissAsync();
        }
    }
}
