using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Components;
using MikePhil.Charting.Data;
using MoneyFox.Business.ViewModels;
using MoneyFox.Droid.Dialogs;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "ModifyAccountActivity",
        Name = "moneyfox.droid.activities.StatisticCategorySpreadingActivity",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class StatisticCategorySpreadingActivity : MvxAppCompatActivity<StatisticCategorySpreadingViewModel>,
        IDialogCloseListener
    {
        public void HandleDialogClose()
        {
            SetChartData();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_category_spreading);
            Title = Strings.CategorySpreadingLabel;

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            ViewModel.LoadCommand.Execute();

            SetChartData();
        }

        private void SetChartData()
        {
            var chart = FindViewById<PieChart>(Resource.Id.chart);

            // enable rotation of the chart by touch
            chart.RotationAngle = 0;
            chart.RotationEnabled = true;
            chart.HighlightPerTapEnabled = true;
            chart.SetUsePercentValues(true);
            chart.Description.Enabled = false;

            chart.DrawHoleEnabled = true;
            chart.SetHoleColor(Color.Transparent);
            chart.HoleRadius = 7f;

            var entries = new List<PieEntry>();
            var values = ViewModel.StatisticItems.Select(x => x.Value).ToList();
            for (int i = 0; values.Count <= i; i++)
            {
                entries.Add(new PieEntry((float)values[i], 0));
            }

            var labels = ViewModel.StatisticItems.Select(x => x.Label);

            var dataset = new PieDataSet(entries, "Categories");
            
            var data = new PieData(dataset);

            chart.Data = data;
            chart.Invalidate();
        }


        //protected override void OnStart()
        //{
        //    base.OnStart();
        //    ViewModel.LoadCommand.Execute();
        //}

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