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

namespace MoneyFox.Windows.Tasks
{
    public sealed class CortanaAccountAmount : IBackgroundTask
    {
        BackgroundTaskDeferral deferral;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            deferral = taskInstance.GetDeferral();
            taskInstance.Canceled += TaskInstance_Canceled;
            AppServiceTriggerDetails trigger = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            VoiceCommandServiceConnection connection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(trigger);
            VoiceCommand voiceCommand = await connection.GetVoiceCommandAsync();
            string step = await CortanaFunctions.ReadStepFile();
            if (!step.Contains("payment") && step.Contains("account"))
            {
                if (step.Contains("create"))
                {
                    AccountEntity account = (AccountEntity)await CortanaFunctions.DeserializeAsync("account");
                    account.CurrentBalance =double.Parse(CortanaFunctions.SemanticInterpretation("amount", voiceCommand));
                    await CortanaFunctions.SerializeAsync(account, "account");
                    CortanaFunctions.Updatestepfile("accountamount");
                    VoiceCommandUserMessage userMessage = CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageAccountName"), CortanaFunctions.GetResourceString("CortanaUserMessageAccountName"));
                    await connection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(userMessage));
                    deferral?.Complete();
                }
                else
                {
                    AccountEntity account = (AccountEntity)await CortanaFunctions.DeserializeAsync("account");
                    account.CurrentBalance = Double.Parse(CortanaFunctions.SemanticInterpretation("amount", voiceCommand));
                    AccountService accountService = new AccountService(new AmbientDbContextLocator(), new DbContextScopeFactory());
                    Account saveaccount = new Account(account);
                    await accountService.SaveAccount(saveaccount);
                    CortanaFunctions.Updatestepfile("");
                    await connection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageAccountCreated"), CortanaFunctions.GetResourceString("CortanaUserMessageAccountCreated"))));
                    deferral?.Complete();
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