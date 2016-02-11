using MoneyManager.Droid.Fragments;
using Android.Runtime;
using MoneyManager.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using Android.Views;
using Android.OS;
using Android.Support.V4.View;
using MvvmCross.Droid.Support.V4;
using System.Collections.Generic;
using MoneyManager.Localization;
using Android.Support.Design.Widget;

namespace MoneyManager.Droid.Fragments
{    
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneymanager.droid.fragments.SettingsFragment")]
    public class SettingsFragment : BaseFragment<SettingsViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_settings;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            var viewPager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
            if (viewPager != null)
            {
                var fragments = new List<MvxFragmentPagerAdapter.FragmentInfo>
                    {
                        new MvxFragmentPagerAdapter.FragmentInfo(Strings.ShortcutsTitle, typeof (SettingsShortcutsFragment),
                            typeof (TileSettingsViewModel)),
                        new MvxFragmentPagerAdapter.FragmentInfo(Strings.SecurityTitle, typeof (SettingsSecurityFragment),
                            typeof (PasswordUserControlViewModel))
                    };
                viewPager.Adapter = new MvxFragmentPagerAdapter(Activity, ChildFragmentManager, fragments);
            }

            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabs);
            tabLayout.SetupWithViewPager(viewPager);

            return view;
        }
    }
}

