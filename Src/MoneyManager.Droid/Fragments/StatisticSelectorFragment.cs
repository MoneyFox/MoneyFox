using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MoneyManager.Core.ViewModels;
using MoneyManager.Localization;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;

namespace MoneyManager.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneymanager.droid.fragments.AboutFragment")]
    public class StatisticSelectorFragment : BaseFragment<StatisticSelectorViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_statistic_selector, null);

            var listview = view.FindViewById<ListView>(Resource.Id.statistic_list);
            listview.Adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleListItem1,
                new[] {Strings.CashflowLabel, Strings.SpreadingLabel, Strings.CategorySummary});
            listview.ItemClick += ListviewOnItemClick;

            return view;
        }

        protected override int FragmentId => Resource.Layout.fragment_statistic_selector;

        private void ListviewOnItemClick(object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
        {
            switch (itemClickEventArgs.Position)
            {
                case 0:
                    ViewModel.GoToCashFlowChartCommand.Execute();
                    break;

                case 1:
                    ViewModel.GoToCategorySpreadingChartCommand.Execute();
                    break;

                case 2:
                    ViewModel.GoToCategorySummaryCommand.Execute();
                    break;
            }
        }
    }
}