namespace MoneyFox.Views.Dialogs
{

    using System;
    using System.Threading.Tasks;
    using ViewModels.Dialogs;
    using Xamarin.CommunityToolkit.Extensions;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterPopup
    {
        public FilterPopup()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.SelectFilterDialogViewModel;
        }

        private SelectFilterDialogViewModel ViewModel => (SelectFilterDialogViewModel)BindingContext;

        public async Task ShowAsync()
        {
            await Application.Current.MainPage.Navigation.ShowPopupAsync(this);
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            ViewModel.FilterSelectedCommand.Execute(null);
            Dismiss(null);
        }
    }

}
