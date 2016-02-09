using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.FullFragging.Fragments;

namespace MoneyManager.Droid.Fragments
{
    public class CategorySummaryFragment : MvxFragment
    {
        //public new StatisticViewModel ViewModel
        //{
        //    get { return (StatisticViewModel)base.ViewModel; }
        //    set { base.ViewModel = value; }
        //}

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.fragment_category_summary, null);
        }
    }
}