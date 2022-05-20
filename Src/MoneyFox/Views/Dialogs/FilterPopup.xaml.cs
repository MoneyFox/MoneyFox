namespace MoneyFox.Views.Dialogs
{

    using System;
    using ViewModels.Dialogs;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterPopup
    {
        public FilterPopup()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.SelectFilterDialogViewModel;
        }

        private SelectFilterDialogViewModel ViewModel => (SelectFilterDialogViewModel)BindingContext;

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            ViewModel.FilterSelectedCommand.Execute(null);
            Close();
        }
    }

}
