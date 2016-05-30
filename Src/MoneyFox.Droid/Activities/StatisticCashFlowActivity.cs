using Android.App;
using Android.OS;
using MvvmCross.Droid.Support.V7.AppCompat;
using MoneyFox.Shared.ViewModels;
using Android.Content.PM;
using Android.Support.V7.Widget;
using OxyPlot.Xamarin.Android;
using Android.Views;
using MoneyFox.Shared.Resources;
using MoneyFox.Droid.Dialogs;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "ModifyAccountActivity",
        Name = "moneyfox.droid.activities.StatisticCashFlowActivity",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class StatisticCashFlowActivity : MvxAppCompatActivity<StatisticCashFlowViewModel>
    {
        private PlotView plotModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.graphical_statistic_activity);

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            plotModel = FindViewById<PlotView>(Resource.Id.plotViewModel);

            Title = Strings.CashflowLabel;
        }

        protected override void OnStart()
        {
            OnResume();

            ViewModel.LoadCommand.Execute();
            plotModel.Model = ViewModel.CashFlowModel;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_select, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>
        ///     This hook is called whenever an item in your options menu is selected.
        /// </summary>
        /// <param name="item">The menu item that was selected.</param>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                case Resource.Id.action_add:
                    var dialog = new SelectDateRangeDialog(this);
                    dialog.Show(FragmentManager.BeginTransaction(), Strings.SelectDateTitle);
                    return true;

                default:
                    return false;
            }
        }
    }
}