using Android.Runtime;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using MoneyManager.Core.ViewModels;
using MoneyManager.Droid.Fragments;

namespace MoneyManager.Droid
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
	[Register("moneymanager.droid.fragments.SettingsShortcutsFragment")]
    public class SettingsShortcutsFragment : BaseFragment<TileSettingsViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_settings_shortcuts;
    }
}

