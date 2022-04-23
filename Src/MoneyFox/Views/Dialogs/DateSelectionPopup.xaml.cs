namespace MoneyFox.Views.Dialogs
{

    using System;
    using System.Threading.Tasks;
    using Core._Pending_.Common.Messages;
    using ViewModels.Dialogs;
    using Xamarin.CommunityToolkit.Extensions;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DateSelectionPopup
    {
        public DateSelectionPopup(DateTime dateFrom, DateTime dateTo)
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.SelectDateRangeDialogViewModel;
            ViewModel.StartDate = dateFrom;
            ViewModel.EndDate = dateTo;
        }

        private SelectDateRangeDialogViewModel ViewModel => (SelectDateRangeDialogViewModel)BindingContext;

        public async Task ShowAsync()
        {
            await Application.Current.MainPage.Navigation.ShowPopupAsync(this);
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            ViewModel.DoneCommand.Execute(null);
            Dismiss(null);
        }
    }

}
