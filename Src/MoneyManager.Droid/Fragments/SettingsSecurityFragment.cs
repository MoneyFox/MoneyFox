using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using MoneyManager.Core.ViewModels;
using Android.Runtime;
using MoneyManager.Droid.Fragments;

namespace MoneyManager.Droid
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
	[Register("moneymanager.droid.fragments.SettingsSecurityFragment")]
    public class SettingsSecurityFragment : BaseFragment<PasswordUserControlViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_settings_security;
    }
}

