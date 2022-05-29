namespace MoneyFox.Views.Popups;

using Core._Pending_.Common.Messages;
using ViewModels.Dialogs;

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
        Close();
    }
}
