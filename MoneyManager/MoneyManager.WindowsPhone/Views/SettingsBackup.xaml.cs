using System;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.Common;
using Xamarin;

namespace MoneyManager.Views {
	public sealed partial class SettingsBackup {
		public SettingsBackup() {
			InitializeComponent();

			NavigationHelper = new NavigationHelper(this);
		}

		public NavigationHelper NavigationHelper { get; }

		private BackupViewModel backupView {
			get { return ServiceLocator.Current.GetInstance<BackupViewModel>(); }
		}

		#region NavigationHelper registration

		protected override async void OnNavigatedTo(NavigationEventArgs e) {
			try {
				await backupView.LogInToOneDrive();
				await backupView.LoadBackupCreationDate();
				NavigationHelper.OnNavigatedTo(e);
			}
			catch (Exception ex) {
				Insights.Report(ex, ReportSeverity.Error);
			}
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e) {
			NavigationHelper.OnNavigatedFrom(e);
		}

		#endregion NavigationHelper registration
	}
}