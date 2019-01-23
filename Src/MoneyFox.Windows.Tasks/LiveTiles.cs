using EntityFramework.DbContextScope;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Windows.Business.Tiles;
using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;

namespace MoneyFox.Windows.Tasks
{
    public sealed class LiveTiles : IBackgroundTask
    {
        private BackgroundTaskDeferral serviceDeferral;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                var accountService = new AccountService(new AmbientDbContextLocator(), new DbContextScopeFactory());

                serviceDeferral = taskInstance.GetDeferral();
                taskInstance.Canceled += OnTaskCanceled;

                var liveTileManager = new LiveTileManager(accountService);
                await liveTileManager.UpdatePrimaryLiveTile();
                await liveTileManager.UpdateSecondaryLiveTiles();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Debug.WriteLine("LiveTile update failed.");
            }
            finally
            {
                serviceDeferral?.Complete();
            }
        }

        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            serviceDeferral?.Complete();
            serviceDeferral = null;
        }
    }
}