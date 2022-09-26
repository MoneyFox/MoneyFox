namespace MoneyFox.Ui.Views.Budget;

public partial class ModifyBudgetView
{
    public ModifyBudgetView()
    {
        InitializeComponent();
    }

    private void AmountFieldGotFocus(object sender, FocusEventArgs e)
    {
        AmountEntry.CursorPosition = 0;
        AmountEntry.SelectionLength = AmountEntry.Text?.Length ?? 0;
    }
}
