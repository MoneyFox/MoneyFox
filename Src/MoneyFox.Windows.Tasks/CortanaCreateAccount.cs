using EntityFramework.DbContextScope;
using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Windows.Business;

namespace MoneyFox.Windows.Tasks
{
   public sealed class CortanaCreateAccount : IBackgroundTask
    {
        BackgroundTaskDeferral deferral;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
             deferral = taskInstance.GetDeferral();
            AppServiceTriggerDetails trigger = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            VoiceCommandServiceConnection connection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(trigger);
            VoiceCommand voiceCommand = await connection.GetVoiceCommandAsync();
            taskInstance.Canceled += TaskInstance_Canceled;
            string step = CortanaFunctions.ReadStepFile();
            if (step == "")
            {
                CortanaFunctions.SerializeAsync(new AccountEntity(), "account");
                CortanaFunctions.Updatestepfile("createaccount");
                VoiceCommandUserMessage userMessage = CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageAccountName"), CortanaFunctions.GetResourceString("CortanaUserMessageAccountName"));
                await connection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(userMessage));
                deferral?.Complete();
            }
            else
            {
                await connection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageIncorrectStepError"), CortanaFunctions.GetResourceString("CortanaUserMessageIncorrectStepError"))));
                deferral?.Complete();
                return;
            }
         }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            deferral?.Complete();
        }
    }
}