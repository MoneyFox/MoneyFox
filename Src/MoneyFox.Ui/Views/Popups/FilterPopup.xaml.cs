namespace MoneyFox.Views.Popups;

using Core.Common.Messages;
using Ui;
using ViewModels.Dialogs;

public partial class FilterPopup
{
    public FilterPopup()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SelectFilterDialogViewModel>();
    }

    public FilterPopup(PaymentListFilterChangedMessage message)
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SelectFilterDialogViewModel>();
        ViewModel.Initialize(message);
    }

    private SelectFilterDialogViewModel ViewModel => (SelectFilterDialogViewModel)BindingContext;

    private void Button_OnClicked(object sender, EventArgs e)
    {
        ViewModel.FilterSelectedCommand.Execute(null);
        Close();
    }
}
