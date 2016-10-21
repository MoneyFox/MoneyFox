using Android.Runtime;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Resources;

namespace MoneyFox.Droid.Fragments
{
    [Register("moneyfox.droid.fragments.SettingsGeneralFragment")]
    public class SettingsGeneralFragment : BaseFragment<SettingsGeneralViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_settings_general;
        protected override string Title => Strings.SettingsLabel;
    }
}