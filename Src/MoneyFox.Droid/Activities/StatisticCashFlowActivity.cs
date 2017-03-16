using System.Collections.Generic;
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
using MikePhil.Charting.Data;
using MikePhil.Charting.Charts;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "ModifyAccountActivity",
        Name = "moneyfox.droid.activities.StatisticCashFlowActivity",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class StatisticCashFlowActivity : MvxAppCompatActivity<StatisticCashFlowViewModel>, IDialogCloseListener
    {
        public void HandleDialogClose()
        {
            SetChartData();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_cash_flow);
            Title = Strings.CashflowLabel;

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SetChartData();
        }

        private void SetChartData()
        {
            var chart = FindViewById<BarChart>(Resource.Id.chart);

            var dataSetdExpenses = new BarDataSet(new List<BarEntry> { new BarEntry(1, (float)ViewModel.CashFlow.Expense.Value) }, ViewModel.CashFlow.Expense.Label);
            dataSetdExpenses.SetColors(Resources.GetColor(Resource.Color.color_expense, Theme));

            var dataSetIncome = new BarDataSet(new List<BarEntry> { new BarEntry(2, (float)ViewModel.CashFlow.Income.Value) }, ViewModel.CashFlow.Income.Label);
            dataSetIncome.SetColors(Resources.GetColor(Resource.Color.color_income, Theme));

            var dataSetRevenue = new BarDataSet(new List<BarEntry> { new BarEntry(3, (float)ViewModel.CashFlow.Revenue.Value) }, ViewModel.CashFlow.Revenue.Label);
            dataSetRevenue.SetColors(Resources.GetColor(Resource.Color.color_revenue, Theme));

            var barData = new BarData(dataSetdExpenses, dataSetIncome, dataSetRevenue);
            barData.BarWidth = 0.9f;
            chart.Data = barData;
            chart.SetPinchZoom(false);
            chart.SetFitBars(true);
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