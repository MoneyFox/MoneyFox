using MoneyFox.Application.Common;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Dialogs;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Presentation.ViewModels.Interfaces;
using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
    public partial class AccountListPage
    {
        private IAccountListViewModel ViewModel => BindingContext as AccountListViewModel;

        public AccountListPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.AccountListVm;

            if(Device.RuntimePlatform == Device.iOS)
            {
                var addItem = new ToolbarItem
                              {
                                  Text = Strings.AddTitle,
                                  Priority = 0,
                                  Order = ToolbarItemOrder.Primary
                              };
                addItem.Clicked += AddItem_Clicked;

                ToolbarItems.Add(addItem);
            }
        }

        protected override void OnAppearing()
        {
            ViewModel?.LoadDataCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }

        private void AddItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushPopupAsync(new AddAccountAndPaymentPopup { BindingContext = ViewModel?.ViewActionViewModel })
                      .FireAndForgetSafeAsync();
        }

        private void EditAccount(object sender, EventArgs e)
        {
            if(!(sender is MenuItem menuItem))
                return;

            ViewModel?.EditAccountCommand.Execute(menuItem.CommandParameter as AccountViewModel);
        }

        private void DeleteAccount(object sender, EventArgs e)
        {
            if(!(sender is MenuItem menuItem))
                return;

            ViewModel?.DeleteAccountCommand.ExecuteAsync(menuItem.CommandParameter as AccountViewModel).FireAndForgetSafeAsync();
        }
    }
}
