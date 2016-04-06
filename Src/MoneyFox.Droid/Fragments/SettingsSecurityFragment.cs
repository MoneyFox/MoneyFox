using MoneyManager.Core.ViewModels;
using Android.Runtime;
using MoneyFox.Droid;
using MoneyManager.Droid.Fragments;

namespace MoneyManager.Droid.Fragments
{
	[Register("moneyfox.droid.fragments.SettingsSecurityFragment")]
    public class SettingsSecurityFragment : BaseFragment<SettingsSecurityViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_settings_security;
    }
}

