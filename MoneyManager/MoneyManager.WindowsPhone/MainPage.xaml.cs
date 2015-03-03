#region

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MoneyManager.Business.Logic;
using MoneyManager.Common;
using MoneyManager.Foundation;
using MoneyManager.Views;

#endregion

namespace MoneyManager {
	public sealed partial class MainPage {
		public MainPage() {
			InitializeComponent();

			NavigationHelper = new NavigationHelper(this);
		}

		public NavigationHelper NavigationHelper { get; }

		private void AddAccountClick(object sender, RoutedEventArgs e) {
			AccountLogic.PrepareAddAccount();
			Frame.Navigate(typeof (AddAccount));
		}

		private void SettingsClick(object sender, RoutedEventArgs e) {
			Frame.Navigate(typeof (SettingsOverview));
		}

		private void GoToAbout(object sender, RoutedEventArgs e) {
			Frame.Navigate(typeof (About));
		}

		private void AddIncomeClick(object sender, RoutedEventArgs e) {
			AddTransaction(TransactionType.Income);
		}

		private void AddSpendingClick(object sender, RoutedEventArgs e) {
			AddTransaction(TransactionType.Spending);
		}

		private void AddTransferClick(object sender, RoutedEventArgs e) {
			AddTransaction(TransactionType.Transfer);
		}

		private static void AddTransaction(TransactionType type) {
			TransactionLogic.GoToAddTransaction(type);
			((Frame) Window.Current.Content).Navigate(typeof (AddTransaction));
		}

		private async void OpenStatisticClick(object sender, RoutedEventArgs e) {
			((Frame) Window.Current.Content).Navigate(typeof (StatisticView));
		}

		private void LicenseClick(object sender, RoutedEventArgs e) {
			((Frame) Window.Current.Content).Navigate(typeof (LicenseView));
		}

		#region NavigationHelper registration

		protected override void OnNavigatedTo(NavigationEventArgs e) {
			NavigationHelper.OnNavigatedTo(e);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e) {
			NavigationHelper.OnNavigatedFrom(e);
		}

		#endregion NavigationHelper registration
	}
}