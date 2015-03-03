#region

using Windows.UI.Xaml.Navigation;
using MoneyManager.Common;

#endregion

namespace MoneyManager.Views {
	public sealed partial class SettingsTiles {
		public SettingsTiles() {
			InitializeComponent();

			NavigationHelper = new NavigationHelper(this);
		}

		public NavigationHelper NavigationHelper { get; }
		public ObservableDictionary DefaultViewModel { get; } = new ObservableDictionary();

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