using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using MoneyFox.Droid.Dialogs;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using OxyPlot.Xamarin.Android;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "ModifyAccountActivity",
        Name = "moneyfox.droid.activities.StatisticCashFlowActivity",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class StatisticCashFlowActivity : MvxAppCompatActivity<StatisticCashFlowViewModel>, IDialogCloseListener
    {
        private PlotView plotModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_generic_graphical_statistic);

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            plotModel = FindViewById<PlotView>(Resource.Id.plotViewModel);

            Title = Strings.CashflowLabel;
        }

        protected override void OnStart()
        {
            OnResume();

            ViewModel.LoadCommand.Execute();
            //we have to assign this here since binding won't work.
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

        public void HandleDialogClose()
        {
            plotModel.Model = ViewModel.CashFlowModel;
        }
    }
}