namespace MoneyFox.Ui.Views.Payments;

using CommunityToolkit.Maui.Views;
using Popups;

public partial class ModifyPaymentContentView
{
    public ModifyPaymentContentView()
    {
        InitializeComponent();
    }

    private void AmountFieldGotFocus(object sender, FocusEventArgs e)
    {
        AmountEntry.CursorPosition = 0;
        AmountEntry.SelectionLength = AmountEntry.Text?.Length ?? 0;
    }

    private void Button_OnClicked(object? sender, EventArgs e)
    {
        var popup = new CategorySelectionPopup();
        Shell.Current.ShowPopup(popup);
    }
}
