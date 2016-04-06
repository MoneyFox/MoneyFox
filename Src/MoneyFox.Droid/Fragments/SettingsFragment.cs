using System.Collections.Generic;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;
using MoneyFox.Shared.Resources;
using MoneyManager.Core.ViewModels;
using MoneyManager.Droid.Fragments;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;

namespace MoneyFox.Droid.Fragments
{    
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.SettingsFragment")]
    public class SettingsFragment : BaseFragment<SettingsViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_settings;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            ((Activities.MainActivity)Activity).SetSupportActionBar(view.FindViewById<Toolbar>(Resource.Id.toolbar));
            ((Activities.MainActivity)Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            var viewPager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
            if (viewPager != null)
            {
                var fragments = new List<MvxFragmentPagerAdapter.FragmentInfo>
                    {
//                        new MvxFragmentPagerAdapter.FragmentInfo(Strings.ShortcutsTitle, typeof (SettingsShortcutsFragment),
//                            typeof (SettingsShortcutsViewModel)),
                        new MvxFragmentPagerAdapter.FragmentInfo(Strings.SecurityTitle, typeof (SettingsSecurityFragment),
                            typeof (SettingsSecurityViewModel))
                    };
                viewPager.Adapter = new MvxFragmentPagerAdapter(Activity, ChildFragmentManager, fragments);
            }

            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabs);
            tabLayout.SetupWithViewPager(viewPager);

            return view;
        }
    }
}

