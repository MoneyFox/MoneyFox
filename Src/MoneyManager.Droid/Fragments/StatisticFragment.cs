using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Support.Fragging.Fragments;
using MoneyManager.Core.ViewModels;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Xamarin.Android;

namespace MoneyManager.Droid.Fragments
{
    public class StatisticFragment : MvxFragment
    {
        public new StatisticViewModel ViewModel
        {
            get { return (StatisticViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        private PlotView plotViewModel;
        public PlotModel MyModel { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.StatisticLayout, null);

            plotViewModel = view.FindViewById<PlotView>(Resource.Id.plotViewModel);

            //Model Allocation Pie char
            var model = new PlotModel();
            var pieSeries = new PieSeries();

            foreach (var item in ViewModel.MonthlySpreading)
            {
                pieSeries.Slices.Add(new PieSlice(item.Label, item.Value));
            }

            model.IsLegendVisible = true;
            model.LegendPosition = LegendPosition.BottomLeft;

            model.Series.Add(pieSeries);
            MyModel = model;
            plotViewModel.Model = MyModel;

            return view;
        }
    }
}