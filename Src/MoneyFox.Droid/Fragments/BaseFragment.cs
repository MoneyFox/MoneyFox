using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using MoneyFox.Droid.Activities;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;

namespace MoneyFox.Droid.Fragments
{
    public abstract class BaseFragment : MvxFragment
    {
        private MvxActionBarDrawerToggle _drawerToggle;
        private Toolbar _toolbar;

        protected BaseFragment()
        {
            RetainInstance = true;
        }

        protected abstract int FragmentId { get; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(FragmentId, null);

            //TODO: I guess this can be removed?
            _toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            if (_toolbar != null)
            {
                ((MainActivity) Activity).SetSupportActionBar(_toolbar);
                ((MainActivity) Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);

                _drawerToggle = new MvxActionBarDrawerToggle(
                    Activity, // host Activity
                    ((MainActivity) Activity).DrawerLayout, // DrawerLayout object
                    _toolbar, // nav drawer icon to replace 'Up' caret
                    Resource.String.drawer_open, // "open drawer" description
                    Resource.String.drawer_close // "close drawer" description
                    );

                ((MainActivity) Activity).DrawerLayout.SetDrawerListener(_drawerToggle);
            }

            return view;
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            if (_toolbar != null)
            {
                _drawerToggle.OnConfigurationChanged(newConfig);
            }
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            if (_toolbar != null)
            {
                _drawerToggle.SyncState();
            }
        }
    }

    public abstract class BaseFragment<TViewModel> : BaseFragment where TViewModel : class, IMvxViewModel
    {
        public TViewModel ViewModel
        {
            get { return (TViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}