using Android.Runtime;
using MoneyManager.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;

namespace MoneyManager.Droid.Fragments
{
    [MvxFragment(typeof(StatisticViewModel), Resource.Id.content_frame)]
    [Register("moneymanager.droid.fragments.CategorySpreadingFragment")]
    public class CategorySpreadingFragment : BaseFragment<CategorySpreadingViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_graphical_statistic;
    }
}