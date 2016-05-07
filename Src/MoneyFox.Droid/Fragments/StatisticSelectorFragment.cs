using Android.OS;
using Android.Runtime;
using Android.Views;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Droid.Shared.Attributes;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragment(typeof (MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.StatisticSelectorFragment")]
    public class StatisticSelectorFragment : BaseFragment<StatisticSelectorViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_statistic_selector;
        protected override string Title => Strings.StatisticTitle;
    }
}