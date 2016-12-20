using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using MoneyFox.Business.ViewModels;
using MoneyFox.Droid.Dialogs;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Droid.Support.V7.AppCompat;
using OxyPlot.Xamarin.Android;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "ModifyAccountActivity",
        Name = "moneyfox.droid.activities.StatisticCategorySpreadingActivity",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class StatisticCategorySpreadingActivity : MvxAppCompatActivity<StatisticCategorySpreadingViewModel>,
        IDialogCloseListener
    {
        private PlotView plotModel;

        public void HandleDialogClose()
        {
            plotModel.Model = ViewModel.SpreadingModel;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_graphical_statistic_with_legend);

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            plotModel = FindViewById<PlotView>(Resource.Id.plotViewModel);

            Title = Strings.CategorySpreadingLabel;
        }

        protected override void OnStart()
        {
            OnResume();

            ViewModel.LoadCommand.Execute();
            plotModel.Model = ViewModel.SpreadingModel;
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

                case Resource.Id.action_select:
                    var dialog = new SelectDateRangeDialog();
                    dialog.Show(SupportFragmentManager.BeginTransaction(), Strings.SelectDateTitle);
                    return true;

                default:
                    return false;
            }
        }
    }
}