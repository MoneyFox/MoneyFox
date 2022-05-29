namespace MoneyFox.Views.Popups
{

    using System;
    using MoneyFox.Core._Pending_.Common.Messages;
    using MoneyFox.ViewModels.Dialogs;

    public partial class DateSelectionPopup
    {
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

        private SelectDateRangeDialogViewModel ViewModel => (SelectDateRangeDialogViewModel)BindingContext;

        private void Button_OnClicked(object sender, EventArgs e)
        {
            ViewModel.DoneCommand.Execute(null);
            Dismiss(null);
        }
    }

}
