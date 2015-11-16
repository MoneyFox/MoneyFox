using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Support.Fragging.Fragments;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using OxyPlot.Xamarin.Android;

namespace MoneyManager.Droid.Fragments
{
    public class GraphicalStatisticFragment : MvxFragment
    {
        private PlotView plotViewModel;

        public new StatisticViewModel ViewModel
        {
            get { return (StatisticViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public StatisticType SelectedStatistic { get; set; }
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.StatisticLayout, null);

            SetGraphicalStatistic(view);

            return view;
        }

        private void SetGraphicalStatistic(View view)
        {
            plotViewModel = view.FindViewById<PlotView>(Resource.Id.plotViewModel);

            switch (SelectedStatistic)
            {
                case StatisticType.Cashflow:
                    plotViewModel.Model = ViewModel.CashFlowModel;
                    break;
                case StatisticType.CategorySpreading:
                    plotViewModel.Model = ViewModel.SpreadingModel;
                    break;
            }
        }

        public override void OnPause()
        {
            plotViewModel.Model = null;

            base.OnPause();
        }
    }
}