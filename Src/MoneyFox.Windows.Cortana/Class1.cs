using EntityFramework.DbContextScope;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Pocos;
using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Resources.Core;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;
using Windows.System.Threading;
using MoneyFox.Foundation.Resources;
using MoneyFox.Foundation;
using System.Linq;
using Newtonsoft.Json;

namespace MoneyFox.Windows.Cortana
{
    public sealed class CortanaCommands : IBackgroundTask
    {
        ThreadPoolTimer poolTimer;
        BackgroundTaskDeferral serviceDeferral;
        ResourceMap cortanaResourceMap;
        ResourceContext cortanaContext;
        AccountService accountService = new AccountService(new AmbientDbContextLocator(), new DbContextScopeFactory());
        BackgroundTaskDeferral backgroundTaskDeferral;
        PaymentService paymentService = new PaymentService(new AmbientDbContextLocator(), new DbContextScopeFactory());
        VoiceCommandUserMessage UserMessage;
        VoiceCommandResponse voiceCommandResponse;
        StorageFolder storageFolder = ApplicationData.Current.LocalCacheFolder;
        StorageFile stepFile;
        StorageFile contentFile;
        Payment payment;
        RecurringPayment recurringPayment;
        string commandType;
        string LastStep;


        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            serviceDeferral = taskInstance.GetDeferral();
            taskInstance.Canceled += OnTaskCanceled;
            AppServiceTriggerDetails triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            VoiceCommandServiceConnection voiceCommandServiceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails);
            VoiceCommand command = await voiceCommandServiceConnection.GetVoiceCommandAsync();
            cortanaResourceMap = ResourceManager.Current.MainResourceMap;
            cortanaContext = ResourceContext.GetForViewIndependentUse();
            string rule = command.SpeechRecognitionResult.RulePath[0];
            stepFile = await storageFolder.CreateFileAsync("file.txt", CreationCollisionOption.OpenIfExists);
            commandType = FileIO.ReadTextAsync(stepFile)?.GetResults()?.Split(",")[1];
            LastStep = FileIO.ReadTextAsync(stepFile)?.GetResults()?.Split(",")[0];
            if (triggerDetails != null && triggerDetails.Name == "MoneyFoxCortanaIntegration")
            {





            }
        }

        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            throw new NotImplementedException();
        }

        private string SemanticInterpretation(string interpretationKey, VoiceCommand command)
        {
            return command.SpeechRecognitionResult.SemanticInterpretation.Properties[interpretationKey].FirstOrDefault();
        }
        private async System.Threading.Tasks.Task<VoiceCommandContentTile> CreateTileAsync(string Title, VoiceCommandContentTileType type, StorageFile file, string Text)
        {
            VoiceCommandContentTile vcct = new VoiceCommandContentTile
            {
                Title = Title ?? Foundation.Resources.Strings.ApplicationTitle,
                ContentTileType = type,
                TextLine1 = Text ?? null,
                Image = file ?? await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-app:///MoneyFox.Windows/Assets/Square44x44Logo.targetsize-48.png"))
            };
            return vcct;
        }
        private VoiceCommandUserMessage CreateUserMessage(string spokenmessage, string displaymessage)
        {
            VoiceCommandUserMessage vcum = new VoiceCommandUserMessage()
            {
                DisplayMessage = displaymessage ?? Foundation.Resources.Strings.CortanaDefaultUserMessageText,
                SpokenMessage = spokenmessage ?? Foundation.Resources.Strings.CortanaDefaultUserMessageSpoken
            };

            return vcum;
        }
        private async System.Threading.Tasks.Task SerializeAsync(object temppayments, string paymenttypes)
        {
            contentFile = await storageFolder.CreateFileAsync("contnent.json", CreationCollisionOption.ReplaceExisting);
            List<PropertyInfo> tempinfo = new List<PropertyInfo>();
            switch (paymenttypes)
            {

                case "payment":
                    var temppayment = (Payment)temppayments;
                    tempinfo = temppayment.Data.GetType().GetProperties().ToList();
                    break;
                case "reoccuring":
                    var tempreccurpayment = (RecurringPayment)temppayments;
                    tempinfo = tempreccurpayment.Data.GetType().GetProperties().ToList();
                    break;
                case "account":
                    var tempaccount = (Account)temppayments;
                    tempinfo = tempaccount.Data.GetType().GetProperties().ToList();
                    break;
                default:
                    return;
            }
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            foreach (PropertyInfo x in tempinfo)
            {
                keyValuePairs.Add(x.Name, x.GetValue(temppayments).ToString());
            }

            string json = JsonConvert.SerializeObject(keyValuePairs);
            await FileIO.WriteTextAsync(contentFile, json);
        }
        private string GetResourceString(string resourcekey)
        {
            ResourceLoader keyValuePairs = ResourceLoader.GetForViewIndependentUse();
            return keyValuePairs.GetString(resourcekey);
        }
        private void CreateThreadPoolTimer(TimeSpan timeSpan)
        {
            poolTimer = ThreadPoolTimer.CreatePeriodicTimer(async (source) =>
            {
                UserMessage = CreateUserMessage(Foundation.Resources.Strings.CortanaContinuationMessageSpoken, Foundation.Resources.Strings.CortanaContinuationMessageText);
                await this.connection.ReportProgressAsync(VoiceCommandResponse.CreateResponse(UserMessage));
            },
    timeSpan,
    (source) =>
    {

    });
        }
        private async System.Threading.Tasks.Task<object> DeserializeAsync(string paymenttypes)
        {
            contentFile = await storageFolder.CreateFileAsync("contnent.json", CreationCollisionOption.OpenIfExists);
            List<PropertyInfo> tempinfo = new List<PropertyInfo>();
            string te = FileIO.ReadTextAsync(contentFile).GetResults();
            var test = JsonConvert.DeserializeObject<Dictionary<string, string>>(te);
            switch (paymenttypes)
            {

                case "payment":
                    DataAccess.Entities.PaymentEntity pm = new DataAccess.Entities.PaymentEntity();
                    foreach (KeyValuePair<string, string> x in test)
                    {
                        var z = pm.GetType().GetProperty(x.Key);
                        z.SetValue(pm, x.Value);
                    }
                    return pm;

                case "reoccuring":
                    DataAccess.Entities.RecurringPaymentEntity rpm = new DataAccess.Entities.RecurringPaymentEntity();
                    foreach (KeyValuePair<string, string> x in test)
                    {
                        var z = rpm.GetType().GetProperty(x.Key);
                        z.SetValue(rpm, x.Value);
                    }
                    return rpm;
                case "account":
                    DataAccess.Entities.AccountEntity am = new DataAccess.Entities.AccountEntity();
                    foreach (KeyValuePair<string, string> x in test)
                    {
                        var z = am.GetType().GetProperty(x.Key);
                        z.SetValue(am, x.Value);
                    }
                    return am;

                default:
                    return null;
            }
        }


    }
}
