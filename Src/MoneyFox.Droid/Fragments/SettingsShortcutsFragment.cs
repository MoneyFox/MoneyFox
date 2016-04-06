using Android.Runtime;
using MoneyFox.Droid;
using MoneyManager.Core.ViewModels;
using MoneyManager.Droid.Fragments;

namespace MoneyManager.Droid.Fragments
{
	[Register("moneyfox.droid.fragments.SettingsShortcutsFragment")]
    public class SettingsShortcutsFragment : BaseFragment<SettingsShortcutsViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_settings_shortcuts;
    }
}

