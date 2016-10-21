using Android.Runtime;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Resources;

namespace MoneyFox.Droid.Fragments
{
    [Register("moneyfox.droid.fragments.SettingsSecurityFragment")]
    public class SettingsSecurityFragment : BaseFragment<SettingsSecurityViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_settings_security;
        protected override string Title => Strings.SettingsLabel;

        public override void OnStart()
        {
            ViewModel.LoadCommand.Execute();
            base.OnStart();
        }
    }
}