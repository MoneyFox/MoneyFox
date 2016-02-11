using Android.Runtime;
using MoneyManager.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;

namespace MoneyManager.Droid.Fragments
{
    [MvxFragment(typeof(StatisticViewModel), Resource.Id.content_frame)]
    [Register("moneymanager.droid.fragments.StatisticCashFlowFragment")]
    public class StatisticCashFlowFragment :  BaseFragment<StatisticCashFlowViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_graphical_statistic;



        public override void OnStart()
        {
            OnResume();

            ViewModel.LoadCommand.Execute();
        }

    }
}