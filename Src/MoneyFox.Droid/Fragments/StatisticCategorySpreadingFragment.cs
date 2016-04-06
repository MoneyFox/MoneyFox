using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using MoneyManager.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;
using OxyPlot.Xamarin.Android;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.StatisticCategorySpreadingFragment")]
    public class StatisticCategorySpreadingFragment : MvxFragment<StatisticCategorySpreadingViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_graphical_statistic, null);

            ((Activities.MainActivity)Activity).SetSupportActionBar(view.FindViewById<Toolbar>(Resource.Id.toolbar));
            ((Activities.MainActivity)Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);

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