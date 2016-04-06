using Android.Runtime;
using MoneyManager.Core.ViewModels;
using MoneyManager.Droid.Fragments;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.StatisticSelectorFragment")]
    public class StatisticSelectorFragment : BaseFragment<StatisticSelectorViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_statistic_selector;
    }
}