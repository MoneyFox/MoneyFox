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
using Windows.Foundation.Collections;

namespace MoneyFox.Windows.Tasks
{
    public sealed class CortanaCreatePayment : IBackgroundTask
    {
        BackgroundTaskDeferral serviceDeferral;
        PaymentEntity payment;
        string commandType;
        ValueSet valueset = new ValueSet(); 
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            serviceDeferral = taskInstance.GetDeferral();
            taskInstance.Canceled += TaskInstance_Canceled;
           var local_folder = ApplicationData.Current.LocalFolder;
            AppServiceTriggerDetails trigger = taskInstance.TriggerDetails as AppServiceTriggerDetails;
           VoiceCommandServiceConnection connection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(trigger);
            AppServiceConnection appServiceConnection = trigger.AppServiceConnection;
            appServiceConnection.RequestReceived += AppServiceConnection_RequestReceived;
            VoiceCommand voiceCommand = await connection.GetVoiceCommandAsync();
            VoiceCommandUserMessage userMessage;
            VoiceCommandUserMessage repromptMessage;
            AccountService Accounts = new AccountService(new AmbientDbContextLocator(), new DbContextScopeFactory());
            string step = CortanaFunctions.ReadStepFile();
            if (!step.Contains("account") && step=="")
            {
                List<Account> newlistaccounts = new List<Account>();
                try
                {
                    var test = await Accounts.GetAllAccounts();
                    newlistaccounts = test.ToList<Account>();
                }
                catch (Exception e)
                {
                    await CortanaFunctions.Logging(e.ToString());
                    await connection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageNoAccountsPaymentCreationSpoken"), CortanaFunctions.GetResourceString("CortanaUserMessageNoAccountsPaymentCreationText"))));
                    serviceDeferral?.Complete();
                }
                userMessage = CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageReoccuranceSpoken"), CortanaFunctions.GetResourceString("CortanaUserMessageReoccuranceText"));
                List<VoiceCommandContentTile> Allothercontents = new List<VoiceCommandContentTile>();
                var payementreoccurs = Enum.GetNames(typeof(PaymentRecurrence)).Cast<string>().ToList();
                for (int i = 0; i < payementreoccurs.Count - 1; i++)
                {
                    Allothercontents.Add(CortanaFunctions.CreateTile(payementreoccurs[i], string.Format(CortanaFunctions.GetResourceString("CortanaContentTileReoccuranceText"), CortanaFunctions.GetResourceString(payementreoccurs[i] + "Label"))));
                }

                Allothercontents.Add(CortanaFunctions.CreateTile("None", string.Format(CortanaFunctions.GetResourceString("CortanaContentTileReoccuranceText"), CortanaFunctions.GetResourceString("CortanaContentTileNoRecurranceTitle"))));
                repromptMessage = CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageReccuranceRepromptSpoken"), CortanaFunctions.GetResourceString("CortanaUserMessageReccuranceRepromptDisplay"));
                VoiceCommandResponse voiceCommandResponse = VoiceCommandResponse.CreateResponseForPrompt(userMessage, repromptMessage, Allothercontents);
                VoiceCommandDisambiguationResult vcdr = await connection.RequestDisambiguationAsync(voiceCommandResponse);
                if (vcdr != null)
                {
                    var reoccur = vcdr.SelectedItem.Title;
                    if (reoccur == "None" || reoccur == null)
                    {
                        payment = new PaymentEntity
                        {
                            IsRecurring = false
                        };
                        commandType = "payment";
                        CortanaFunctions.Updatestepfile("create-payment,payment");
                    }
                    else
                    {
                        payment = new PaymentEntity
                        {
                            IsRecurring = true,
                            RecurringPayment = new RecurringPaymentEntity(),

                        };
                        payment.RecurringPayment.Recurrence = (PaymentRecurrence)Enum.Parse(typeof(PaymentRecurrence), vcdr.SelectedItem.Title);
                        commandType = "reccuring";

                        CortanaFunctions.Updatestepfile("create-payment,recurring");
                    }

                }
                userMessage = CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessagePaymentTypeSpoken"), CortanaFunctions.GetResourceString("CortanaUserMessagePaymentTypeText"));
                repromptMessage = CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessagePaymentTypeRepromptSpoken"), CortanaFunctions.GetResourceString("CortanaUserMessagePaymentTypeRepromptDisplay"));
                Allothercontents.Clear();
                Allothercontents.Add(CortanaFunctions.CreateTile(CortanaFunctions.GetResourceString("AddIncomeLabel"), string.Format(CortanaFunctions.GetResourceString("CortanaContentTilePaymentTypeText"), CortanaFunctions.GetResourceString("AddIncomeLabel"))));
                Allothercontents.Add(CortanaFunctions.CreateTile(CortanaFunctions.GetResourceString("AddExpenseLabel"), CortanaFunctions.GetResourceString("CortanaContentTilePaymentTypeText")));
                Allothercontents.Add(CortanaFunctions.CreateTile(CortanaFunctions.GetResourceString("AddTransferLabel"), CortanaFunctions.GetResourceString("CortanaContentTilePaymentTypeText")));



                vcdr = await connection.RequestDisambiguationAsync(VoiceCommandResponse.CreateResponseForPrompt(userMessage, repromptMessage, Allothercontents));

                if (vcdr.SelectedItem.Title == CortanaFunctions.GetResourceString("AddIncomeLabel"))
                {
                    userMessage = CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageWhichAccountSpoken"), string.Format(CortanaFunctions.GetResourceString("CortanaUserMessageWhichAccountText"), CortanaFunctions.GetResourceString("AddIncomeLabel")));
                    repromptMessage = CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageWhichAccountRepromptSpoken"), string.Format(CortanaFunctions.GetResourceString("CortanaUserMessageWhichAccountRepromptText"), CortanaFunctions.GetResourceString("AddIncomeLabel")));

                    Allothercontents.Clear();
                    if (newlistaccounts.Count() >= 2)
                    {
                        foreach (Account x in newlistaccounts)
                        {
                            Allothercontents.Add(CortanaFunctions.CreateTile(x.Data.Name, string.Format(CortanaFunctions.GetResourceString("CortanaContentTitleIncomeAccount"), x.Data.Name)));
                        }
                        VoiceCommandDisambiguationResult newvcdr;
                        newvcdr = await connection.RequestDisambiguationAsync(VoiceCommandResponse.CreateResponseForPrompt(userMessage, repromptMessage, Allothercontents));

                        switch (commandType)
                        {
                            case "payment":
                                payment.Type = PaymentType.Income;
                                if (newvcdr != null)
                                {
                                    payment.ChargedAccount = newlistaccounts.FirstOrDefault<Account>(x => x.Data.Name == newvcdr.SelectedItem.Title).Data;
                                }
                                CortanaFunctions.SerializeAsync(payment, "payment");
                                CortanaFunctions.Updatestepfile("create-payment,recurring");
                                break;

                            case "recurring":
                                payment.RecurringPayment.Type = PaymentType.Income;
                                payment.RecurringPayment.ChargedAccount = newlistaccounts.FirstOrDefault<Account>(x => x.Data.Name == newvcdr.SelectedItem.Title).Data;
                                CortanaFunctions.SerializeAsync(payment, "payment");
                                CortanaFunctions.Updatestepfile("create-payment,recurring");
                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        Allothercontents.Add(CortanaFunctions.CreateTile(newlistaccounts[0].Data.Name, string.Format(CortanaFunctions.GetResourceString("CortanaContentTitleIncomeAccount"), newlistaccounts[0].Data.Name)));
                        var vcdr2 = await connection.RequestConfirmationAsync(VoiceCommandResponse.CreateResponse(CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageOnlyOneAccountIncomeSpoken"), string.Format(CortanaFunctions.GetResourceString("CortanaUserMessageOnlyOneAccountIncomeText"), newlistaccounts[0].Data.Name))));
                        if (vcdr2.Confirmed == true)
                        {
                            switch (commandType)
                            {
                                case "payment":
                                    payment.Type = PaymentType.Income;
                                    payment.ChargedAccount = newlistaccounts[0].Data;
                                    CortanaFunctions.SerializeAsync(payment, "payment");
                                    CortanaFunctions.Updatestepfile("create-payment,recurring");
                                    break;

                                case "recurring":
                                    payment.RecurringPayment.Type = PaymentType.Income;
                                    payment.RecurringPayment.ChargedAccount = newlistaccounts[0].Data;
                                    CortanaFunctions.SerializeAsync(payment, "payment");
                                    CortanaFunctions.Updatestepfile("create-payment,recurring");
                                    break;

                                default:
                                    break;
                            }
                        }
                        serviceDeferral?.Complete();
                    }

                }
                else if (vcdr.SelectedItem.Title == CortanaFunctions.GetResourceString("AddExpenseLabel"))
                {
                    userMessage = CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageWhichAccountSpoken"), string.Format(CortanaFunctions.GetResourceString("CortanaUserMessageWhichAccountText"), CortanaFunctions.GetResourceString("AddExpenseLabel")));
                    repromptMessage = CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageWhichAccountRepromptSpoken"), string.Format(CortanaFunctions.GetResourceString("CortanaUserMessageWhichAccountRepromptText"), CortanaFunctions.GetResourceString("AddExpenseLabel")));
                    Allothercontents.Clear();
                    if (newlistaccounts.Count() >= 2)
                    {
                        foreach (Account x in newlistaccounts)
                        {
                            Allothercontents.Add(CortanaFunctions.CreateTile(x.Data.Name, string.Format(CortanaFunctions.GetResourceString("CortanaContentTitleExpenseAccount"), x.Data.Name)));
                        }
                        VoiceCommandDisambiguationResult newvcdr;
                        newvcdr = await connection.RequestDisambiguationAsync(VoiceCommandResponse.CreateResponseForPrompt(userMessage, repromptMessage, Allothercontents));
                        switch (commandType)
                        {
                            case "payment":
                                payment.Type = PaymentType.Expense;
                                payment.ChargedAccount = newlistaccounts.FirstOrDefault<Account>(x => x.Data.Name == newvcdr.SelectedItem.Title).Data;
                                CortanaFunctions.SerializeAsync(payment, "payment");
                                CortanaFunctions.Updatestepfile("create-payment,recurring");
                                break;
                            case "recurring":
                                payment.RecurringPayment.Type = PaymentType.Expense;
                                payment.RecurringPayment.ChargedAccount = newlistaccounts.FirstOrDefault<Account>(x => x.Data.Name == newvcdr.SelectedItem.Title).Data;
                                CortanaFunctions.SerializeAsync(payment, "payment");
                                CortanaFunctions.Updatestepfile("create-payment,recurring");
                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        Allothercontents.Add(CortanaFunctions.CreateTile(newlistaccounts[0].Data.Name, string.Format(CortanaFunctions.GetResourceString("CortanaContentTitleIncomeAccount"), newlistaccounts[0].Data.Name)));
                        var vcdr2 = await connection.RequestConfirmationAsync(VoiceCommandResponse.CreateResponse(CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageOnlyOneAccountExpenseSpoken"), string.Format(CortanaFunctions.GetResourceString("CortanaUserMessageOnlyOneAccountExpenseText"), newlistaccounts[0].Data.Name))));
                        if (vcdr2.Confirmed == true)
                        {
                            switch (commandType)
                            {
                                case "payment":
                                    payment.Type = PaymentType.Income;
                                    payment.ChargedAccount = newlistaccounts[0].Data;
                                    CortanaFunctions.SerializeAsync(payment, "payment");
                                    CortanaFunctions.Updatestepfile("create-payment,recurring");
                                    break;

                                case "recurring":
                                    payment.RecurringPayment.Type = PaymentType.Income;
                                    payment.RecurringPayment.ChargedAccount = newlistaccounts[0].Data;
                                    CortanaFunctions.SerializeAsync(payment, "payment");
                                    CortanaFunctions.Updatestepfile("create-payment,recurring");
                                    break;

                                default:
                                    break;
                            }
                        }
                        serviceDeferral?.Complete();
                    }
                }
                else if (vcdr.SelectedItem.Title == CortanaFunctions.GetResourceString("AddTransferLabel"))
                {
                    userMessage = CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageTransferFromAccountSpoken"), CortanaFunctions.GetResourceString("CortanaUserMessageTransferFromAccountText"));
                    repromptMessage = CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageTransferFromAccountRepromptSpoken"), CortanaFunctions.GetResourceString("CortanaUserMessageTransferFromAccountRepromptText"));
                    VoiceCommandUserMessage transferto;
                    VoiceCommandUserMessage transfertoreprompt;
                    transferto = CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageTransferToAccountSpoken"), CortanaFunctions.GetResourceString("CortanaUserMessageTransferToAccountText"));
                    transfertoreprompt = CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageTransferToAccountRepromptSpoken"), CortanaFunctions.GetResourceString("CortanaUserMessageTransferToAccountRepromptText"));
                    List<VoiceCommandContentTile> transferfrom = new List<VoiceCommandContentTile>();
                    List<VoiceCommandContentTile> transfertoaccount = new List<VoiceCommandContentTile>();
                    if (newlistaccounts.Count() >= 2)
                    {
                        foreach (Account x in newlistaccounts)
                        {
                            transferfrom.Add(CortanaFunctions.CreateTile(x.Data.Name, string.Format(CortanaFunctions.GetResourceString("CortanaContentTileTransferFromAccount"), x.Data.Name)));
                            transfertoaccount.Add(CortanaFunctions.CreateTile(x.Data.Name, string.Format(CortanaFunctions.GetResourceString("CortanaContentTileTransferToAccount"), x.Data.Name)));
                        }
                        VoiceCommandDisambiguationResult newvcdr;
                        VoiceCommandDisambiguationResult anothervcdr;
                        newvcdr = await connection.RequestDisambiguationAsync(VoiceCommandResponse.CreateResponseForPrompt(userMessage, repromptMessage, transferfrom));
                        anothervcdr = await connection.RequestDisambiguationAsync(VoiceCommandResponse.CreateResponseForPrompt(transferto, transfertoreprompt, transfertoaccount));

                        switch (commandType)
                        {
                            case "payment":
                                {
                                    payment.Type = PaymentType.Transfer;
                                    payment.ChargedAccount = newlistaccounts.FirstOrDefault<Account>(x => x.Data.Name == newvcdr.SelectedItem.Title).Data;
                                    payment.TargetAccount = newlistaccounts.FirstOrDefault<Account>(x => x.Data.Name == anothervcdr.SelectedItem.Title).Data;
                                    CortanaFunctions.SerializeAsync(payment, "payment");
                                    CortanaFunctions.Updatestepfile("create-payment,recurring");
                                    break;
                                }
                            case "recurring":
                                {
                                    payment.RecurringPayment.Type = PaymentType.Transfer;
                                    payment.RecurringPayment.ChargedAccount = newlistaccounts.FirstOrDefault<Account>(x => x.Data.Name == newvcdr.SelectedItem.Title).Data;
                                    payment.RecurringPayment.TargetAccount = newlistaccounts.FirstOrDefault<Account>(x => x.Data.Name == anothervcdr.SelectedItem.Title).Data;
                                    CortanaFunctions.SerializeAsync(payment, "payment");
                                    CortanaFunctions.Updatestepfile("create-payment,recurring");
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                    else
                    {
                        await connection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageOnlyOneAccountTransferSpoken"), CortanaFunctions.GetResourceString("CortanaUserMessageOnlyOneAccountTransferText"))));
                        serviceDeferral?.Complete();
                    }
                }

            }
            else
            {
                userMessage = CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageIncorrectStepError"), CortanaFunctions.GetResourceString("CortanaUserMessageIncorrectStepError"));
                await connection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(userMessage));
                serviceDeferral?.Complete();
                return;
            }
            userMessage = CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessagePaymentAmountSpoken"), CortanaFunctions.GetResourceString("CortanaUserMessagePaymentAmountText"));
            await connection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(userMessage));
            serviceDeferral?.Complete();
           
            
        }

        private void AppServiceConnection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            serviceDeferral?.Complete();
        }
    }
    public sealed class CortanaCreateRecurringPayment : IBackgroundTask
    
{
    }
    public static class PaymentCommonTasks
    {
        public static string ReturnReccurrance()
        {

        }
        public static string
    }


}
