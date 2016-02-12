using Android.OS;
using Android.Runtime;
using Android.Views;
using MoneyManager.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using OxyPlot.Xamarin.Android;

namespace MoneyManager.Droid.Fragments
{
    [MvxFragment(typeof(StatisticViewModel), Resource.Id.content_frame)]
    [Register("moneymanager.droid.fragments.CategorySpreadingFragment")]
    public class CategorySpreadingFragment : BaseFragment<CategorySpreadingViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_graphical_statistic;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            var model = view.FindViewById<PlotView>(Resource.Id.plotViewModel);
            model.Model = ViewModel.SpreadingModel;

            return view;
        }

        public override void OnStart()
        {
            OnResume();

            ViewModel.LoadCommand.Execute();
        }
    }
}