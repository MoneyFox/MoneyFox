using EntityFramework.DbContextScope;
using Microsoft.Toolkit.Uwp.Notifications;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Core;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.Storage;
using MoneyFox.Windows.Business;
using Windows.Foundation.Collections;

namespace MoneyFox.Windows.Tasks
{
	public sealed class LiveTiles : IBackgroundTask
	{
		BackgroundTaskDeferral serviceDeferral;
		AppServiceConnection connection;
		AccountService accountService = new AccountService(new AmbientDbContextLocator(), new DbContextScopeFactory());
		PaymentService paymentService = new PaymentService(new AmbientDbContextLocator(), new DbContextScopeFactory());
		List<CommonFunctions.Paymentitem> allpayment = new List<CommonFunctions.Paymentitem>();
		ApplicationDataContainer appsettings = ApplicationData.Current.LocalSettings;
		Dictionary<Foundation.PaymentRecurrence, Func<CommonFunctions.iReccurance>> strategy = new Dictionary<Foundation.PaymentRecurrence, Func<CommonFunctions.iReccurance>>();
		public async void Run(IBackgroundTaskInstance taskInstance)
		{

			serviceDeferral = taskInstance.GetDeferral();
			taskInstance.Canceled += OnTaskCanceled;
			strategy.Add(Foundation.PaymentRecurrence.Daily, () => new CommonFunctions.RecurrDaily());
			strategy.Add(Foundation.PaymentRecurrence.DailyWithoutWeekend, () => new CommonFunctions.RecurrWeekdays());
			strategy.Add(Foundation.PaymentRecurrence.Weekly, () => new CommonFunctions.RecurrWeekly());
			strategy.Add(Foundation.PaymentRecurrence.Biweekly, () => new CommonFunctions.RecurrBiWeekly());
			strategy.Add(Foundation.PaymentRecurrence.Monthly, () => new CommonFunctions.RecurrMonthly());
			strategy.Add(Foundation.PaymentRecurrence.Bimonthly, () => new CommonFunctions.RecurrBiMonthly());
			strategy.Add(Foundation.PaymentRecurrence.Quarterly, () => new CommonFunctions.RecurrQuarterly());
			strategy.Add(Foundation.PaymentRecurrence.Yearly, () => new CommonFunctions.RecurrYearly());
			strategy.Add(Foundation.PaymentRecurrence.Biannually, () => new CommonFunctions.RecurrbiYearly());
			await CommonFunctions.UpdatePrimaryLiveTile();
			await UpdateSecondaryLiveTiles();
			serviceDeferral?.Complete();


		}
		
		private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
		{
			serviceDeferral?.Complete();
			serviceDeferral = null;
		}

	}

}
