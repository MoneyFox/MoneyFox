using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using MoneyFox.Business.ViewModels;
using MoneyFox.Droid.Jobs;
using MoneyFox.Foundation.Interfaces;
using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Views;
using PCLAppConfig;
using Rg.Plugins.Popup;

#if !DEBUG
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
#endif

namespace MoneyFox.Droid
{
	 [Activity(Label = "MoneyFox", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		  Exported = true, Name = "com.applysolutions.moneyfox.MainActivity")]
	 public class MainActivity : MvxFormsAppCompatActivity
	 {
		  /// <summary>
		  ///     Constant for the ClearPayment Service.
		  /// </summary>
		  public const int MESSAGE_SERVICE_CLEAR_PAYMENTS = 1;

		  /// <summary>
		  ///     Constant for the recurring payment Service.
		  /// </summary>
		  public const int MESSAGE_SERVICE_RECURRING_PAYMENTS = 2;

		  /// <summary>
		  ///     Constant for the sync backup Service.
		  /// </summary>
		  public const int MESSAGE_SERVICE_SYNC_BACKUP = 3;

		  /// <summary>
		  ///    Constant for the add expense shortcut
		  /// </summary>
		  const string ACTION_ADD_EXPENSE_VIEW = "com.applysolutions.moneyfox.shortcuts.ADD_EXPENSE";

		  /// <summary>
		  ///	  Constant for the add income shortcut
		  /// </summary>
		  const string ACTION_ADD_INCOME_VIEW = "com.applysolutions.moneyfox.shortcuts.ADD_INCOME";

		  /// <summary>
		  ///	  Constant for the add income shortcut
		  /// </summary>
		  const string ACTION_ADD_TRANSFER_VIEW = "com.applysolutions.moneyfox.shortcuts.ADD_TRANSFER";

		  Handler handler;
		  private ClearPaymentsJob clearPaymentsJob;
		  private RecurringPaymentJob recurringPaymentJob;

		  protected override void OnCreate(Bundle bundle)
		  {
			   if (ConfigurationManager.AppSettings == null)
					ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
#if !DEBUG
         AppCenter.Start("6d9840ff-d832-4c1b-a2ee-bac7f15d89bd", typeof(Analytics), typeof(Crashes));
#endif

			   TabLayoutResource = Resource.Layout.Tabbar;
			   ToolbarResource = Resource.Layout.Toolbar;

			   base.OnCreate(bundle);

			   Popup.Init(this, bundle);

			   // Handler to create jobs.
			   handler = new Handler(msg =>
			   {
					switch (msg.What)
					{
						 case MESSAGE_SERVICE_CLEAR_PAYMENTS:
							  clearPaymentsJob = (ClearPaymentsJob)msg.Obj;
							  clearPaymentsJob.ScheduleTask();
							  break;
						 case MESSAGE_SERVICE_RECURRING_PAYMENTS:
							  recurringPaymentJob = (RecurringPaymentJob)msg.Obj;
							  recurringPaymentJob.ScheduleTask();
							  break;
					}
			   });

			   // Start services and provide it a way to communicate with us.
			   var startServiceIntentClearPayment = new Intent(this, typeof(ClearPaymentsJob));
			   startServiceIntentClearPayment.PutExtra("messenger", new Messenger(handler));
			   StartService(startServiceIntentClearPayment);

			   var startServiceIntentRecurringPayment = new Intent(this, typeof(RecurringPaymentJob));
			   startServiceIntentRecurringPayment.PutExtra("messenger", new Messenger(handler));
			   StartService(startServiceIntentRecurringPayment);

			   if (Mvx.IoCProvider.CanResolve<IBackgroundTaskManager>() && Mvx.IoCProvider.CanResolve<ISettingsManager>())
			   {
					Mvx.IoCProvider.Resolve<IBackgroundTaskManager>()
					   .StartBackupSyncTask(Mvx.IoCProvider.Resolve<ISettingsManager>().BackupSyncRecurrence);
			   }

			   // If the user opened the app via one of the shortcuts,
			   // navigate the user to the right page
			   NavigateToShortcuts();
		  }

		  public override void OnBackPressed()
		  {
			   Popup.SendBackPressed(base.OnBackPressed);
		  }

		  private void NavigateToShortcuts()
		  {
			   switch (Intent.Action)
			   {
					case ACTION_ADD_EXPENSE_VIEW:
						 {
							  System.Diagnostics.Debug.WriteLine("AddExpense");
							  Mvx.IoCProvider.Resolve<AccountListViewActionViewModel>().GoToAddExpenseCommand.Execute(null);
						 }
						 break;
					case ACTION_ADD_INCOME_VIEW:
						 {
							  System.Diagnostics.Debug.WriteLine("AddIncome");
							  Mvx.IoCProvider.Resolve<AccountListViewActionViewModel>().GoToAddIncomeCommand.Execute(null);
						 }
						 break;
					case ACTION_ADD_TRANSFER_VIEW:
						 {
							  System.Diagnostics.Debug.WriteLine("AddTransfer");
							  Mvx.IoCProvider.Resolve<AccountListViewActionViewModel>().GoToAddTransferCommand.Execute(null);
						 }
						 break;
			   }
		  }
	 }
}

