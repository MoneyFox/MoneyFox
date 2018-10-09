using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace MoneyFox.Windows.LiveandLockScreenTiles
{
    public sealed class UpdateliveandLockscreenTiles : IBackgroundTask
    {
        BackgroundTaskDeferral serviceDeferral;
        AppServiceConnection connection;
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            serviceDeferral = taskInstance.GetDeferral();
            taskInstance.Canceled += OnTaskCanceled;

            var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            connection = details.AppServiceConnection;

            //Listen for incoming app service requests
            connection.RequestReceived += OnRequestReceivedAsync;
        }

        private async Task OnRequestReceivedAsync(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var messageDeferral = args.GetDeferral();

            try
            {
                var input = args.Request.Message;
                if (input != null)
                {


                    string command = input["Command"] as string;

                    switch (command)
                    {
                        case "GetProductUnitCountForRegion":
                            {
                                var productId = (int)input["ProductId"];
                                var regionId = (int)input["RegionId"];
                                var inventoryData = GetInventoryData(productId, regionId);
                                var result = new ValueSet();
                                result.Add("UnitCount", inventoryData.UnitCount);
                                result.Add("LastUpdated", inventoryData.LastUpdated.ToString());

                                await args.Request.SendResponseAsync(result);
                            }
                            break;

                        //Other commands

                        default:
                            return;
                    }
                }
            finally
            {
                //Complete the message deferral so the operating system knows we're done responding
                messageDeferral.Complete();
            }
        }


        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            throw new NotImplementedException();
        }
        private void UpdatePrimaryLiveTile()
        {

        }
        private void UpdateSecondaryLiveTiles()
        {

        }

    }
}

