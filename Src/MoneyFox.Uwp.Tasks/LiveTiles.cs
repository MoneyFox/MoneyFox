using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using MoneyFox.Application;
using MoneyFox.Uwp.Business.Tiles;
using MoneyFox.Persistence;

namespace MoneyFox.Uwp.Tasks
{
    public sealed class LiveTiles : IBackgroundTask
    {
        private BackgroundTaskDeferral serviceDeferral;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                serviceDeferral = taskInstance.GetDeferral();
                taskInstance.Canceled += OnTaskCanceled;

                ExecutingPlatform.Current = AppPlatform.UWP;

                var context = EfCoreContextFactory.Create();

                var crudService = new CrudServicesAsync(context, utData.ConfigAndMapper);

                var liveTileManager = new LiveTileManager(crudService);
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
