using Android.App;
using Android.Content.PM;
using MoneyManager.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace MoneyManager.Droid.Activities
{
    [Activity(Label = "StatisticActivity",
       Name = "moneymanager.droid.activities.StatisticActivity",
       Theme = "@style/AppTheme",
       LaunchMode = LaunchMode.SingleTop)]
    public class StatisticActivity : MvxCachingFragmentCompatActivity<StatisticViewModel>
    {
    }
}