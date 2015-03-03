#region

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.Common;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;

#endregion

namespace MoneyManager.Views {
	public sealed partial class AddAccount {
		public AddAccount() {
			InitializeComponent();
			NavigationHelper = new NavigationHelper(this);
		}

		public Account SelectedAccount {
			get { return ServiceLocator.Current.GetInstance<AddAccountViewModel>().SelectedAccount; }
		}

		private void DoneClick(object sender, RoutedEventArgs e) {
			if (String.IsNullOrEmpty(SelectedAccount.Name)) {
				SelectedAccount.Name = Translation.GetTranslation("NoNamePlaceholderLabel");
			}

			ServiceLocator.Current.GetInstance<AddAccountViewModel>().Save();
			ServiceLocator.Current.GetInstance<BalanceViewModel>().UpdateBalance();
		}

		private void CancelClick(object sender, RoutedEventArgs e) {
			ServiceLocator.Current.GetInstance<AddAccountViewModel>().Cancel();
		}

		#region NavigationHelper registration

		public NavigationHelper NavigationHelper { get; }

		protected override void OnNavigatedTo(NavigationEventArgs e) {
			NavigationHelper.OnNavigatedTo(e);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e) {
			NavigationHelper.OnNavigatedFrom(e);
		}

		#endregion NavigationHelper registration
	}
}