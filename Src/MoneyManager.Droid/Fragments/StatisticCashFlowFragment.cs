using Android.Runtime;
using MoneyManager.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using Android.Views;
using Android.OS;
using OxyPlot.Xamarin.Android;

namespace MoneyManager.Droid.Fragments
{
    [MvxFragment(typeof(StatisticViewModel), Resource.Id.content_frame)]
    [Register("moneymanager.droid.fragments.StatisticCashFlowFragment")]
    public class StatisticCashFlowFragment :  BaseFragment<StatisticCashFlowViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_graphical_statistic;
      
        private PlotView plotModel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            plotModel = view.FindViewById<PlotView>(Resource.Id.plotViewModel);
            plotModel.Model = ViewModel.CashFlowModel;

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