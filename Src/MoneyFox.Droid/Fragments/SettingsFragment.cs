using System.Collections.Generic;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;

namespace MoneyFox.Droid.Fragments {
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.SettingsFragment")]
    public class SettingsFragment : BaseFragment<SettingsViewModel> {
        protected override int FragmentId => Resource.Layout.fragment_settings;
        protected override string Title => Strings.SettingsLabel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            var viewPager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
            if (viewPager != null) {
                var fragments = new List<MvxCachingFragmentStatePagerAdapter.FragmentInfo> {
                    new MvxCachingFragmentStatePagerAdapter.FragmentInfo(Strings.GeneralTitle, typeof(SettingsGeneralFragment),
                        typeof(SettingsGeneralViewModel)),
                    new MvxCachingFragmentStatePagerAdapter.FragmentInfo(Strings.SecurityTitle, typeof(SettingsSecurityFragment),
                        typeof(SettingsSecurityViewModel))
                };
                viewPager.Adapter = new MvxCachingFragmentStatePagerAdapter(Activity, ChildFragmentManager, fragments);
            }

            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabs);
            tabLayout.SetupWithViewPager(viewPager);

            return view;
        }
    }
}