using Android.Runtime;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;

namespace MoneyFox.Droid.Fragments
{
    [Register("moneyfox.droid.fragments.SettingsSecurityFragment")]
    public class SettingsSecurityFragment : BaseFragment<SettingsSecurityViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_settings_security;
        protected override string Title => Strings.SettingsLabel;
    }
}