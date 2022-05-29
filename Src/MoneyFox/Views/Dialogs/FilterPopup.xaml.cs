namespace MoneyFox.Views.Dialogs
{

    using System;
    using System.Threading.Tasks;
    using Core._Pending_.Common.Messages;
    using Rg.Plugins.Popup.Extensions;
    using ViewModels.Dialogs;
    using Xamarin.Forms;

    public partial class FilterPopup
    {
        public FilterPopup()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.SelectFilterDialogViewModel;
        }

        public FilterPopup(PaymentListFilterChangedMessage message)
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.SelectFilterDialogViewModel;
            ViewModel.Initialize(message);
        }

        private SelectFilterDialogViewModel ViewModel => (SelectFilterDialogViewModel)BindingContext;

        private void Button_OnClicked(object sender, EventArgs e)
        {
            ViewModel.FilterSelectedCommand.Execute(null);
            Dismiss(null);
        }
    }

}
