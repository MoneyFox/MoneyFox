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
    /// <summary>
    /// Cortana AppService to add a name during account creation
    /// </summary>
    public sealed class CortanaAccountName : IBackgroundTask
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
            if (!step.Contains("payment") && step.Contains("account"))
            {
                if (step.Contains("create"))
                {
                    AccountEntity account = (AccountEntity)CortanaFunctions.DeserializeAsync("account");
                    account.Name = CortanaFunctions.SemanticInterpretation("name", voiceCommand);
                    CortanaFunctions.SerializeAsync(account, "account");
                    CortanaFunctions.Updatestepfile("accountname");
                    VoiceCommandUserMessage userMessage = CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageAccountAmount"), CortanaFunctions.GetResourceString("CortanaUserMessageAccountAmount"));
                    await connection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(userMessage));
                    deferral?.Complete();
                }
                else
                {
                    AccountEntity account = (AccountEntity)CortanaFunctions.DeserializeAsync("account");
                    account.Name = CortanaFunctions.SemanticInterpretation("name", voiceCommand);
                    AccountService accountService = new AccountService(new AmbientDbContextLocator(), new DbContextScopeFactory());
                    Account saveaccount = new Account(account);
                    await accountService.SaveAccount(saveaccount);
                    CortanaFunctions.Updatestepfile("");
                    await connection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageAccountCreated"), CortanaFunctions.GetResourceString("CortanaUserMessageAccountCreated"))));
                }
            }
            else
            {

                await connection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageIncorrectStepError"), CortanaFunctions.GetResourceString("CortanaUserMessageIncorrectStepError"))));
                deferral?.Complete();
                return;
            }
            deferral?.Complete();
         }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            deferral?.Complete();
        }
    }
}