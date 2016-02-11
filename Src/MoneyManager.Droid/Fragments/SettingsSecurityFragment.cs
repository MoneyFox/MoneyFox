using MoneyManager.Core.ViewModels;
using Android.Runtime;
using MoneyManager.Droid.Fragments;

namespace MoneyManager.Droid.Fragments
{
	[Register("moneymanager.droid.fragments.SettingsSecurityFragment")]
    public class SettingsSecurityFragment : BaseFragment<PasswordUserControlViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_settings_security;
    }
}

