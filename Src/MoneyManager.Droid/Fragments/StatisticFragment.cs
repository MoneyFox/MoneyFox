using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Support.Fragging.Fragments;
using MoneyManager.Core.ViewModels;
using OxyPlot;
using OxyPlot.Xamarin.Android;

namespace MoneyManager.Droid.Fragments
{
    public class StatisticFragment : MvxFragment
    {
        private PlotView plotViewModel;

        public new StatisticViewModel ViewModel
        {
            get { return (StatisticViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public PlotModel MyModel { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.StatisticLayout, null);

            plotViewModel = view.FindViewById<PlotView>(Resource.Id.plotViewModel);
            plotViewModel.Model = ViewModel.CashFlowModel;

            return view;
        }
    }
}