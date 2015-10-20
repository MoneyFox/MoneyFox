using Android.OS;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Support.Fragging.Fragments;
using MoneyManager.Core.ViewModels;
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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.StatisticLayout, null);

            var spinner = view.FindViewById<Spinner>(Resource.Id.spinner_select_statistic);
            spinner.ItemSelected += SpinnerOnItemSelected;

            plotViewModel = view.FindViewById<PlotView>(Resource.Id.plotViewModel);

            return view;
        }

        private void SpinnerOnItemSelected(object sender, AdapterView.ItemSelectedEventArgs itemSelectedEventArgs)
        {
            switch (itemSelectedEventArgs.Id)
            {
                case 0:
                    plotViewModel.Model = ViewModel.CashFlowModel;
                    break;
                case 1:
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