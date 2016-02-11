using Android.Runtime;
using MoneyManager.Core.ViewModels;
using MoneyManager.Droid.Fragments;

namespace MoneyManager.Droid.Fragments
{
	[Register("moneymanager.droid.fragments.SettingsShortcutsFragment")]
    public class SettingsShortcutsFragment : BaseFragment<TileSettingsViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_settings_shortcuts;
    }
}

