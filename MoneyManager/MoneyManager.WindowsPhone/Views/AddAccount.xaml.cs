using MoneyManager.Common;
using MoneyManager.Models;
using MoneyTracker.Src;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace MoneyManager.Views
{
    public sealed partial class AddAccount
    {
        private readonly NavigationHelper navigationHelper;
        private Parameters parameters = new Parameters();

        public AddAccount()
        {
            InitializeComponent();
            navigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            parameters = e.Parameter as Parameters;

            if (parameters.Edit)
            {
                LblTitle.Text = Utilities.GetTranslation("EditTitle");
            }
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            if (parameters != null && Convert.ToBoolean(parameters.Edit))
            {
                App.AccountViewModel.Update(App.AccountViewModel.SelectedAccount);
            }
            else
            {
                App.AccountViewModel.SelectedAccount.Currency = App.Settings.Currency;
                App.AccountViewModel.Save(App.AccountViewModel.SelectedAccount);
            }
            NavigationHelper.GoBack();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.GoBack();
        }
    }
}