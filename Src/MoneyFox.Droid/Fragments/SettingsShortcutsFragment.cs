using Android.Runtime;
using MoneyManager.Core.ViewModels;

namespace MoneyFox.Droid.Fragments
{
	[Register("moneymanager.droid.fragments.SettingsShortcutsFragment")]
    public class SettingsShortcutsFragment : BaseFragment<SettingsShortcutsViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_settings_shortcuts;
    }
}

