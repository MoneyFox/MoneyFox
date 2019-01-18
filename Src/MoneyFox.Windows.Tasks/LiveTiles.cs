using EntityFramework.DbContextScope;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Windows.Business;
using Windows.ApplicationModel.Background;

namespace MoneyFox.Windows.Tasks
{
    public sealed class LiveTiles : IBackgroundTask
    {
        private BackgroundTaskDeferral serviceDeferral;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var accountService = new AccountService(new AmbientDbContextLocator(), new DbContextScopeFactory());

            serviceDeferral = taskInstance.GetDeferral();
            taskInstance.Canceled += OnTaskCanceled;
            await CommonFunctions.UpdatePrimaryLiveTile();
            await CommonFunctions.UpdateSecondaryLiveTiles();
            serviceDeferral?.Complete();
        }

        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            serviceDeferral?.Complete();
            serviceDeferral = null;
        }
    }
}