using Android.App;
using Android.OS;
using MvvmCross.Droid.Support.V7.AppCompat;
using MoneyFox.Shared.ViewModels;
using OxyPlot.Xamarin.Android;
using Android.Support.V7.Widget;
using Android.Content.PM;
using Android.Views;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "ModifyAccountActivity",
        Name = "moneyfox.droid.activities.StatisticCategorySpreadingActivity",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class StatisticCategorySpreadingActivity : MvxAppCompatActivity<StatisticCategorySpreadingViewModel>
    {
        private PlotView plotModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.graphical_statistic_activity);

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            plotModel = FindViewById<PlotView>(Resource.Id.plotViewModel);

        }

        protected override void OnStart()
        {
            OnResume();

            ViewModel.LoadCommand.Execute();
            //plotModel.Model = ViewModel.SpreadingModel;
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

                default:
                    return false;
            }
        }
    }
}