using MoneyFox.Application.Common.Messages;
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
        private SelectDateRangeDialogViewModel ViewModel => (SelectDateRangeDialogViewModel)BindingContext;
        public DateSelectionPopup(DateTime dateFrom, DateTime dateTo)
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.SelectDateRangeDialogViewModel;

            ViewModel.StartDate = dateFrom;
            ViewModel.EndDate = dateTo;
        }

        public DateSelectionPopup(DateSelectedMessage message)
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.SelectDateRangeDialogViewModel;

            ViewModel.Initialize(message);
        }

        public async Task ShowAsync() => await App.Current.MainPage.Navigation.PushPopupAsync(this);

        private static async Task DismissAsync() => await App.Current.MainPage.Navigation.PopPopupAsync();

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            ViewModel.DoneCommand.Execute(null);
            await DismissAsync();
        }
    }
}
