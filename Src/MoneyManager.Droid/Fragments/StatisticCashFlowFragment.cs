using Android.Runtime;
using MoneyManager.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using Android.Views;
using Android.OS;
using OxyPlot.Xamarin.Android;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;
using MvvmCross.Binding.Droid.BindingContext;
using MoneyManager.Droid.Activities;
using Android.Support.V7.Widget;

namespace MoneyManager.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneymanager.droid.fragments.StatisticCashFlowFragment")]
    public class StatisticCashFlowFragment : MvxFragment<StatisticCashFlowViewModel>
    {      
        private PlotView plotModel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_graphical_statistic, null);

            ((MainActivity)Activity).SetSupportActionBar(view.FindViewById<Toolbar>(Resource.Id.toolbar));

            plotModel = view.FindViewById<PlotView>(Resource.Id.plotViewModel);        

            return view;
        }

        public override void OnStart()
        {
            OnResume();

            ViewModel.LoadCommand.Execute();
            plotModel.Model = ViewModel.CashFlowModel;
        }
    }
}