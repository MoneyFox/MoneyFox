namespace MoneyFox.Views.Dialogs
{

    using System;
    using System.Threading.Tasks;
    using ViewModels.Dialogs;

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

        private void Button_OnClicked(object sender, EventArgs e)
        {
            ViewModel.DoneCommand.Execute(null);
            Close();
        }
    }

}
