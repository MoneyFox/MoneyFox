using Android.OS;
using Android.Runtime;
using Android.Views;
using MoneyManager.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;
using OxyPlot.Xamarin.Android;
using MvvmCross.Binding.Droid.BindingContext;

namespace MoneyManager.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneymanager.droid.fragments.StatisticCategorySpreadingFragment")]
    public class StatisticCategorySpreadingFragment : MvxFragment<StatisticCategorySpreadingViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_graphical_statistic, null);

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