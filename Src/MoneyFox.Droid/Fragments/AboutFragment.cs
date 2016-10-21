using Android.Runtime;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Resources;
using MvvmCross.Droid.Shared.Attributes;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.AboutFragment")]
    public class AboutFragment : BaseFragment<AboutViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_about;
        protected override string Title => Strings.AboutTitle;
    }
}