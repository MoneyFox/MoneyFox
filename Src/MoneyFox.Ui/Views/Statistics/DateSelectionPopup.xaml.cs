namespace MoneyFox.Ui.Views.Statistics;

public partial class DateSelectionPopup
{
    public DateSelectionPopup(SelectDateRangeDialogViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
