using Android.OS;
using Android.Views;
using MoneyManager.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.FullFragging.Fragments;

namespace MoneyManager.Droid.Fragments
{
    public class BackupFragment : MvxFragment
    {
        public new BackupViewModel ViewModel
        {
            get { return (BackupViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.BackupLayout, null);

            return view;
        }
    }
}