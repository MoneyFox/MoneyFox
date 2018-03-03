using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using MikePhil.Charting.Animation;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Components;
using MikePhil.Charting.Data;
using MikePhil.Charting.Formatter;
using MoneyFox.Business.ViewModels;
using MoneyFox.Business.ViewModels.Statistic;
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

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_category_spreading);
            Title = Strings.CategorySpreadingLabel;

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            await ViewModel.LoadCommand.ExecuteAsync();

            SetChartData();
        }

        private void SetChartData()
        {
            var chart = FindViewById<PieChart>(Resource.Id.chart);

            chart.RotationAngle = 0;
            chart.RotationEnabled = true;
            chart.HighlightPerTapEnabled = false;
            chart.SetUsePercentValues(true);
            chart.Description.Enabled = false;
            chart.SetExtraOffsets(5, 10, 5, 5);

            chart.DrawHoleEnabled = true;
            chart.SetHoleColor(Color.Transparent);
            chart.HoleRadius = 7f;

            chart.AnimateY(1400, Easing.EasingOption.EaseInOutQuad);

            var entries = new List<PieEntry>();
            var values = ViewModel.StatisticItems.Select(x => x.Value).ToList();
            var labels = ViewModel.StatisticItems.Select(x => x.Label).ToList();

            for (int i = 0; i < values.Count; i++)
            {
                entries.Add(new PieEntry((float)values[i], labels[i]));
            }

            var dataSet = new PieDataSet(entries, "")
            {
                SliceSpace = 1f
            };

            dataSet.AddColor(Resources.GetColor(Resource.Color.color_spreading1, Theme));
            dataSet.AddColor(Resources.GetColor(Resource.Color.color_spreading2, Theme));
            dataSet.AddColor(Resources.GetColor(Resource.Color.color_spreading3, Theme));
            dataSet.AddColor(Resources.GetColor(Resource.Color.color_spreading4, Theme));
            dataSet.AddColor(Resources.GetColor(Resource.Color.color_spreading5, Theme));
            dataSet.AddColor(Resources.GetColor(Resource.Color.color_spreading6, Theme));
            dataSet.AddColor(Resources.GetColor(Resource.Color.color_spreading7, Theme));

            var data = new PieData(dataSet);
            data.SetValueFormatter(new PercentFormatter());
            data.SetValueTextSize(11f);
            data.SetValueTextColor(Color.White);

            chart.SetDrawEntryLabels(false);
            chart.Data = data;

            var legend = chart.Legend;
            legend.TextSize = 12f;
            legend.Orientation = Legend.LegendOrientation.Vertical;
            legend.SetDrawInside(false);
            legend.VerticalAlignment = Legend.LegendVerticalAlignment.Top;
            legend.HorizontalAlignment = Legend.LegendHorizontalAlignment.Left;
            legend.XEntrySpace = 7f;
            legend.YEntrySpace = 0;
            legend.YOffset = 0f;

            chart.HighlightValues(null);
            chart.Invalidate();
        }

        protected override void OnStart()
        {
            base.OnStart();
            ViewModel.LoadCommand.Execute();
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