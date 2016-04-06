using Android.OS;
using Android.Runtime;
using Android.Views;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragment(typeof (MainViewModel), Resource.Id.content_frame)]
    [Register("moneymanager.droid.fragments.AboutFragment")]
    public class AboutFragment : BaseFragment<AboutViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_about;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            RetainInstance = true;

            return view;
        }
    }
}