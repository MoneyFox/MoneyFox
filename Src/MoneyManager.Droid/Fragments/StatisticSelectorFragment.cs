using Android.OS;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Support.Fragging.Fragments;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Localization;

namespace MoneyManager.Droid.Fragments
{
    public class StatisticSelectorFragment : MvxFragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.StatisticSelectorLayout, null);

            var listview = view.FindViewById<ListView>(Resource.Id.statistic_list);
            listview.Adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleListItem1, new [] {Strings.CashflowLabel, Strings.SpreadingLabel, Strings.CategorySummary});
            listview.ItemClick += ListviewOnItemClick;

            return view;
        }

        private void ListviewOnItemClick(object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
        { 

            var graphicalStatisticFragment = new GraphicalStatisticFragment
            {
                ViewModel = Mvx.Resolve<StatisticViewModel>()
            };

            switch (itemClickEventArgs.Position)
            {
                case 0:
                    graphicalStatisticFragment.SelectedStatistic = StatisticType.Cashflow;
                    Activity.SupportFragmentManager.BeginTransaction()
                        .Replace(Resource.Id.content_pane, graphicalStatisticFragment)
                        .AddToBackStack("Cash Flow")
                        .Commit();
                    break;

                case 1:
                    graphicalStatisticFragment.SelectedStatistic = StatisticType.CategorySpreading;
                    Activity.SupportFragmentManager.BeginTransaction()
                        .Replace(Resource.Id.content_pane, graphicalStatisticFragment)
                        .AddToBackStack("Category Spreading")
                        .Commit();
                    break;

                case 2:
                    break;
            }
        }
    }
}