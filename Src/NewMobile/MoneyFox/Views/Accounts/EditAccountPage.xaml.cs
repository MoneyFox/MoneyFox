using MoneyFox.ViewModels.Accounts;
using System;
using Xamarin.Forms;

namespace MoneyFox.Views.Accounts
{
    [QueryProperty("AccountId", "accountid")]

    public partial class EditAccountPage : ContentPage
    {
        public string AccountId
        {
            set
            {
                ViewModel.Init(Uri.UnescapeDataString(value));
            }
        }
        private EditAccountViewModel ViewModel => BindingContext as EditAccountViewModel;


        public EditAccountPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.EditAccountViewModel;

        }
    }
}