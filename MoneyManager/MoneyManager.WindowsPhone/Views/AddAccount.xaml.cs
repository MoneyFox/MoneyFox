using MoneyManager.Common;
using MoneyManager.Models;
using MoneyManager.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace MoneyManager.Views
{
    public sealed partial class AddAccount
    {
        private readonly NavigationHelper navigationHelper;

        public AddAccount()
        {
            InitializeComponent();
            navigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        public Account SelectedAccount
        {
            get { return new ViewModelLocator().AccountViewModel.SelectedAccount; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            var isEditMode = new ViewModelLocator().AddAccount.IsEditMode;

            if (isEditMode)
            {
                new ViewModelLocator().AccountViewModel.Update(SelectedAccount);
            }
            else
            {
                SelectedAccount.Currency = new ViewModelLocator().Settings.Currency;
                new ViewModelLocator().AccountViewModel.Save(SelectedAccount);
            }
            NavigationHelper.GoBack();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.GoBack();
        }
    }
}