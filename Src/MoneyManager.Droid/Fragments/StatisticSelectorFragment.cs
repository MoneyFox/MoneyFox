using Android.OS;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels.Statistics;
using MoneyManager.Foundation;
using MoneyManager.Localization;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;

namespace MoneyManager.Droid.Fragments
{
    public class StatisticSelectorFragment : MvxFragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.StatisticSelectorLayout, null);

            var listview = view.FindViewById<ListView>(Resource.Id.statistic_list);
            listview.Adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleListItem1,
                new[] {Strings.CashflowLabel, Strings.SpreadingLabel, Strings.CategorySummary});
            listview.ItemClick += ListviewOnItemClick;

            return view;
        }

        private void ListviewOnItemClick(object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
        {
            switch (itemClickEventArgs.Position)
            {
                case 0:
                    Activity.SupportFragmentManager.BeginTransaction()
                        .Replace(Resource.Id.content_pane, GetGraphicalStatisticFragment(StatisticType.Cashflow))
                        .AddToBackStack("Cash Flow")
                        .Commit();
                    break;

                case 1:
                    Activity.SupportFragmentManager.BeginTransaction()
                        .Replace(Resource.Id.content_pane,
                            GetGraphicalStatisticFragment(StatisticType.CategorySpreading))
                        .AddToBackStack("Category Spreading")
                        .Commit();
                    break;

                case 2:
                    Activity.SupportFragmentManager.BeginTransaction()
                        .Replace(Resource.Id.content_pane, new CategorySummaryFragment())
                        .AddToBackStack("Category Spreading")
                        .Commit();
                    break;
            }
        }

        private GraphicalStatisticFragment GetGraphicalStatisticFragment(StatisticType type)
        {
            return new GraphicalStatisticFragment
            {
                ViewModel = Mvx.Resolve<StatisticViewModel>()
            };
        }
    }
}