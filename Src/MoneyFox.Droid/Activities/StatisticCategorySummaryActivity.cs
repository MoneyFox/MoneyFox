using Android.App;
using Android.OS;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using Android.Content.PM;
using Android.Support.V7.Widget;
using Android.Views;
using MoneyFox.Shared.Resources;
using MoneyFox.Droid.Dialogs;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "StatisticCategorySummaryActivity",
        Name = "moneyfox.droid.activities.StatisticCategorySummaryActivity",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class StatisticCategorySummaryActivity : MvxAppCompatActivity<StatisticCategorySummaryViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_category_summary);

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            Title = Strings.CategorySummaryLabel;
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

                case Resource.Id.action_add:
                    var dialog = new SelectDateRangeDialog(this);
                    dialog.Show(FragmentManager.BeginTransaction(), Strings.SelectDateTitle);
                    ViewModel.LoadCommand.Execute();
                    return true;

                default:
                    return false;
            }
        }
    }
}