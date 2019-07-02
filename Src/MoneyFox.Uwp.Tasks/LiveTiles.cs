using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using GenericServices.PublicButHidden;
using GenericServices.Setup;
using MoneyFox.DataLayer;
using MoneyFox.Domain;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Uwp.Business.Tiles;
using MoneyFox.Foundation;

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
                var context = new EfCoreContext();
                var utData = context.SetupSingleDtoAndEntities<AccountViewModel>();
                utData.AddSingleDto<CategoryViewModel>();
                utData.AddSingleDto<PaymentViewModel>();
                utData.AddSingleDto<RecurringPaymentViewModel>();

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