using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "StatisticActivity",
        Name = "moneyfox.droid.activities.StatisticActivity",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class StatisticActivity : MvxCachingFragmentCompatActivity<StatisticViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_frame);

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

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