namespace MoneyFox.Ui.Views.Statistics;

public partial class DateSelectionPopup
{
    public DateSelectionPopup(SelectDateRangeDialogViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private SelectDateRangeDialogViewModel ViewModel => (SelectDateRangeDialogViewModel)BindingContext;

    private void Button_OnClicked(object sender, EventArgs e)
    {
        if (ViewModel.IsDateRangeValid)
        {
            ViewModel.DoneCommand.Execute(null);
            Close();
        }
    }
}
