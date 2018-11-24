using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using MoneyFox.Windows.Business;

namespace MoneyFox.Windows.Tasks
{
	public sealed class LiveTiles : IBackgroundTask
	{
		BackgroundTaskDeferral serviceDeferral;
		AppServiceConnection connection;
		public async void Run(IBackgroundTaskInstance taskInstance)
		{

			serviceDeferral = taskInstance.GetDeferral();
			taskInstance.Canceled += OnTaskCanceled;
			//await CommonFunctions.UpdatePrimaryLiveTile();
			//await CommonFunctions.UpdateSecondaryLiveTiles();
			serviceDeferral?.Complete();


		}
		
		private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
		{
			serviceDeferral?.Complete();
			serviceDeferral = null;
		}

	}

}
