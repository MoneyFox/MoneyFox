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
using System.Linq;
using Newtonsoft.Json;
using Windows.ApplicationModel.Resources;
using System.Threading.Tasks;
using MoneyFox.Foundation;
using MoneyFox.DataAccess.Entities;
using MoneyFox.Foundation.Resources;
using stor = Windows.Storage;
namespace MoneyFox.Windows.Cortana
{
    public sealed class CortanaCommands : IBackgroundTask
    {
        BackgroundTaskDeferral serviceDeferral;
        PaymentEntity payment;
        AccountEntity account;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            serviceDeferral = taskInstance.GetDeferral();
            var local_folder = ApplicationData.Current.LocalFolder;
            AppServiceTriggerDetails trigger = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            VoiceCommandServiceConnection connection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(trigger);
            VoiceCommand voiceCommand = await connection.GetVoiceCommandAsync();
            VoiceCommandUserMessage userMessage;
            VoiceCommandUserMessage repromptMessage;
            var rule = voiceCommand.SpeechRecognitionResult.RulePath[0];
           
            string commandType="";
          ///  await Logging(rule, local_folder);
            switch (rule)
            {
                case "create-payment":
                    AmbientDbContextLocator te = new AmbientDbContextLocator();
                    AccountService allAccounts = new AccountService(new AmbientDbContextLocator(), new DbContextScopeFactory());
                    List<Account> newlistaccounts = new List<Account>();
                    IEnumerable<Account> Accounts;
                    try
                    {
                        Accounts = await allAccounts.GetAllAccounts();
                        newlistaccounts = Accounts.ToList<Account>();
                    }
                    catch (Exception e)
                    {
                        await Logging(e.ToString());
                        VoiceCommandUserMessage newmessage = new VoiceCommandUserMessage();
                        newmessage.SpokenMessage = GetResourceString("CortanaUserMessageNoAccountsPaymentCreationSpoken");
                        newmessage.DisplayMessage = GetResourceString("CortanaUserMessageNoAccountsPaymentCreationText");
                        await connection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(newmessage));
                        serviceDeferral?.Complete();
                    }
                    userMessage = CreateUserMessage(GetResourceString("CortanaUserMessageReoccuranceSpoken"),GetResourceString("CortanaUserMessageReoccuranceText"));
                    List<VoiceCommandContentTile> Allothercontents = new List<VoiceCommandContentTile>();
                    var payementreoccurs = Enum.GetNames(typeof(PaymentRecurrence)).Cast<string>().ToList();
                    for (int i = 0; i < payementreoccurs.Count - 1; i++)
                    {
                        Allothercontents.Add(CreateTile(payementreoccurs[i], string.Format(GetResourceString("CortanaContentTileReoccuranceText"), GetResourceString(payementreoccurs[i] + "Label"))));
                    }

                    Allothercontents.Add(CreateTile("None", string.Format(GetResourceString("CortanaContentTileReoccuranceText"), GetResourceString("CortanaContentTileNoRecurranceTitle"))));
                    repromptMessage = CreateUserMessage(GetResourceString("CortanaUserMessageReccuranceRepromptSpoken"), GetResourceString("CortanaUserMessageReccuranceRepromptDisplay"));
                   VoiceCommandResponse voiceCommandResponse = VoiceCommandResponse.CreateResponseForPrompt(userMessage,repromptMessage,Allothercontents);
                 ///   voiceCommandResponse = VoiceCommandResponse.CreateResponseForPrompt(UserMessage,UserMessage,Allothercontents);
                    VoiceCommandDisambiguationResult vcdr = await connection.RequestDisambiguationAsync(voiceCommandResponse);

                    if (vcdr != null)
                    {
                        var reoccur = vcdr.SelectedItem.Title;
                        if (reoccur == "None" || reoccur == null)
                        {
                            payment = new PaymentEntity();
                            payment.IsRecurring = false;
                            commandType = "payment";
                            await SerializeAsync(payment, "payment");
                            await Updatestepfile("create-payment,payment");
                        }
                        else
                        {
                            payment.IsRecurring = true;
                            payment.RecurringPayment = new RecurringPaymentEntity();
                            payment.RecurringPayment.Recurrence = (PaymentRecurrence)Enum.Parse(typeof(PaymentRecurrence), vcdr.SelectedItem.Title);
                            commandType = "reccuring";
                           
                            await SerializeAsync(payment, "payment");
                            await Updatestepfile("create-payment,recurring");
                        }

                    }
                    userMessage = CreateUserMessage(GetResourceString("CortanaUserMessagePaymentTypeSpoken"), GetResourceString("CortanaUserMessagePaymentTypeText"));
                    repromptMessage = CreateUserMessage(GetResourceString("CortanaUserMessagePaymentTypeRepromptSpoken"), GetResourceString("CortanaUserMessagePaymentTypeRepromptDisplay"));
                    Allothercontents.Clear();
                    Allothercontents.Add(CreateTile(GetResourceString("AddIncomeLabel"), GetResourceString("CortanaContentTilePaymentTypeText")));
                    Allothercontents.Add(CreateTile(GetResourceString("AddExpenseLabel"), GetResourceString("CortanaContentTilePaymentTypeText")));
                    Allothercontents.Add(CreateTile(GetResourceString("AddTransferLabel"), GetResourceString("CortanaContentTilePaymentTypeText")));

                 

                    vcdr = await connection.RequestDisambiguationAsync(VoiceCommandResponse.CreateResponseForPrompt(userMessage, repromptMessage, Allothercontents));
                   
                    if (vcdr.SelectedItem.Title == GetResourceString("AddIncomeLabel"))
                    {
                        userMessage = CreateUserMessage(GetResourceString("CortanaUserMessageWhichAccountSpoken"), string.Format(GetResourceString("CortanaUserMessageWhichAccountText"), GetResourceString("AddIncomeLabel")));
                        repromptMessage = CreateUserMessage(GetResourceString("CortanaUserMessageWhichAccountRepromptSpoken"), string.Format(GetResourceString("CortanaUserMessageWhichAccountRepromptText"), GetResourceString("AddIncomeLabel")));

                        Allothercontents.Clear();
                        foreach (Account x in newlistaccounts)
                        {
                            Allothercontents.Add(CreateTile(x.Data.Name, string.Format(GetResourceString("CortanaContentTitleIncomeAccount"), x.Data.Name)));
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
                                await SerializeAsync(payment, "payment");
                                await Updatestepfile("create-payment,recurring");
                                serviceDeferral?.Complete();
                                break;

                            case "recurring":
                                payment.RecurringPayment.Type = PaymentType.Income;
                                payment.RecurringPayment.ChargedAccount = newlistaccounts.FirstOrDefault<Account>(x => x.Data.Name == newvcdr.SelectedItem.Title).Data;
                                await SerializeAsync(payment, "payment");
                                await Updatestepfile("create-payment,recurring");
                                serviceDeferral?.Complete();
                                break;

                            default:
                                break;
                        }
                    }
                    else if (vcdr.SelectedItem.Title == GetResourceString("AddExpenseLabel"))
                    {
                        userMessage = CreateUserMessage(GetResourceString("CortanaUserMessageWhichAccountSpoken"), string.Format(GetResourceString("CortanaUserMessageWhichAccountText"), GetResourceString("AddExpenseLabel")));
                        repromptMessage = CreateUserMessage(GetResourceString("CortanaUserMessageWhichAccountRepromptSpoken"), string.Format(GetResourceString("CortanaUserMessageWhichAccountRepromptText"), GetResourceString("AddExpenseLabel")));

                        Allothercontents.Clear();
                        foreach (Account x in newlistaccounts)
                        {
                            Allothercontents.Add(CreateTile(x.Data.Name, string.Format(GetResourceString("CortanaContentTitleExpenseAccount"), x.Data.Name)));
                        }
                        VoiceCommandDisambiguationResult newvcdr;
                        newvcdr = await connection.RequestDisambiguationAsync(VoiceCommandResponse.CreateResponseForPrompt(userMessage, repromptMessage, Allothercontents));
                        switch (commandType)
                        {
                            case "payment":
                                payment.Type = PaymentType.Expense;
                                payment.ChargedAccount = newlistaccounts.FirstOrDefault<Account>(x => x.Data.Name == newvcdr.SelectedItem.Title).Data;
                                await SerializeAsync(payment, "payment");
                                await Updatestepfile("create-payment,recurring");
                                serviceDeferral?.Complete();
                                break;
                            case "recurring":
                                payment.RecurringPayment.Type = PaymentType.Expense;
                                payment.RecurringPayment.ChargedAccount = newlistaccounts.FirstOrDefault<Account>(x => x.Data.Name == newvcdr.SelectedItem.Title).Data;
                                await SerializeAsync(payment, "payment");
                                await Updatestepfile("create-payment,recurring");
                                serviceDeferral?.Complete();
                                break;

                            default:
                                break;
                        }
                    }
                    else if (vcdr.SelectedItem.Title == GetResourceString("AddTransferLabel"))
                    {
                        userMessage = CreateUserMessage(GetResourceString("CortanaUserMessageTransferFromAccountSpoken"),GetResourceString("CortanaUserMessageTransferFromAccountText"));
                        repromptMessage = CreateUserMessage(GetResourceString("CortanaUserMessageTransferFromAccountRepromptSpoken"),GetResourceString("CortanaUserMessageTransferFromAccountRepromptText"));
                        VoiceCommandUserMessage transferto;
                        VoiceCommandUserMessage transfertoreprompt;
                        transferto = CreateUserMessage(GetResourceString("CortanaUserMessageTransferToAccountSpoken"),GetResourceString("CortanaUserMessageTransferToAccountText"));
                        transfertoreprompt = CreateUserMessage(GetResourceString("CortanaUserMessageTransferToAccountRepromptSpoken"),GetResourceString("CortanaUserMessageTransferToAccountRepromptText"));
                        List<VoiceCommandContentTile> transferfrom = new List<VoiceCommandContentTile>();
                        List<VoiceCommandContentTile> transfertoaccount = new List<VoiceCommandContentTile>();
                        foreach (Account x in newlistaccounts)
                        {
                            transferfrom.Add(CreateTile(x.Data.Name, string.Format(GetResourceString("CortanaContentTileTransferFromAccount"), x.Data.Name)));
                            transfertoaccount.Add(CreateTile(x.Data.Name, string.Format(GetResourceString("CortanaContentTileTransferToAccount"),x.Data.Name)));
                        }
                        VoiceCommandDisambiguationResult newvcdr;
                        VoiceCommandDisambiguationResult anothervcdr;
                        newvcdr = await connection.RequestDisambiguationAsync(VoiceCommandResponse.CreateResponseForPrompt(userMessage, repromptMessage, transferfrom));
                        anothervcdr = await connection.RequestDisambiguationAsync(VoiceCommandResponse.CreateResponseForPrompt(transferto, transfertoreprompt, transfertoaccount));

                        switch (commandType)
                        {
                            case "payment":
                                payment.Type = PaymentType.Transfer;
                                payment.ChargedAccount = newlistaccounts.FirstOrDefault<Account>(x => x.Data.Name == newvcdr.SelectedItem.Title).Data;
                                payment.TargetAccount = newlistaccounts.FirstOrDefault<Account>(x => x.Data.Name == anothervcdr.SelectedItem.Title).Data;
                                await SerializeAsync(payment, "payment");
                                await Updatestepfile("create-payment,recurring");
                                serviceDeferral?.Complete();
                                break;
                            case "recurring":
                                payment.RecurringPayment.Type = PaymentType.Transfer;
                                payment.RecurringPayment.ChargedAccount = newlistaccounts.FirstOrDefault<Account>(x => x.Data.Name == newvcdr.SelectedItem.Title).Data;
                                payment.RecurringPayment.TargetAccount = newlistaccounts.FirstOrDefault<Account>(x => x.Data.Name == anothervcdr.SelectedItem.Title).Data;
                                await SerializeAsync(payment, "payment");
                                await Updatestepfile("create-payment,recurring");
                                serviceDeferral?.Complete();
                                break;

                            default:
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }

        }
        private VoiceCommandContentTile CreateTile(string Title, string Text)
        {
            VoiceCommandContentTile vcct = new VoiceCommandContentTile
            {
                Title = Title ?? Strings.ApplicationTitle,
                ContentTileType = VoiceCommandContentTileType.TitleWithText,
                TextLine1 = Text ?? null,
            };
            return vcct;
        }

        private string SemanticInterpretation(string interpretationKey, VoiceCommand command)
        {
            return command.SpeechRecognitionResult.SemanticInterpretation.Properties[interpretationKey].FirstOrDefault();
        }

        private VoiceCommandUserMessage CreateUserMessage(string spokenmessage, string displaymessage)
        {
            VoiceCommandUserMessage vcum = new VoiceCommandUserMessage()
            {
                DisplayMessage = displaymessage ?? GetResourceString("CortanaUserMessageDefaultErrorMessage"),
                SpokenMessage = spokenmessage ?? GetResourceString("CortanaUserMessageDefaultErrorMessage")
            };

            return vcum;
        }

        private async Task<object> DeserializeAsync(string paymenttypes)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalCacheFolder;
            StorageFile contentFile = await storageFolder.CreateFileAsync("contnent.json", CreationCollisionOption.OpenIfExists);
            List<PropertyInfo> tempinfo = new List<PropertyInfo>();
            string te = FileIO.ReadTextAsync(contentFile).GetResults();
            var test = JsonConvert.DeserializeObject<Dictionary<string, string>>(te);
            switch (paymenttypes)
            {

                case "payment":
                    PaymentEntity pm = new PaymentEntity();
                    foreach (KeyValuePair<string, string> x in test)
                    {
                        var z = pm.GetType().GetProperty(x.Key);
                        z.SetValue(pm, x.Value);
                    }
                    return pm;
                case "account":
                    AccountEntity am = new AccountEntity();
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

        private string GetResourceString(string resourcekey)
        {
            System.Resources.ResourceManager keyValuePairs = Strings.ResourceManager;
            return keyValuePairs.GetString(resourcekey);
        }

        private async Task Logging(string texttolog)
        {
            StorageFolder stors = ApplicationData.Current.LocalFolder;
            await FileIO.AppendTextAsync(await stors.CreateFileAsync("logfile.txt",CreationCollisionOption.OpenIfExists), texttolog);
            await FileIO.AppendTextAsync(await stors.CreateFileAsync("logfile.txt", CreationCollisionOption.OpenIfExists), Environment.NewLine);
        }

        private async Task SerializeAsync(object temppayments, string paymenttypes)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalCacheFolder;
         StorageFile contentFile = await storageFolder.CreateFileAsync("contnent.json", CreationCollisionOption.ReplaceExisting);
            List<PropertyInfo> tempinfo = new List<PropertyInfo>();
            switch (paymenttypes)
            {

                case "payment":
                    var temppayment = (PaymentEntity)temppayments;
                    tempinfo = temppayment.GetType().GetProperties().ToList();
                    break;
                case "account":
                    var tempaccount = (AccountEntity)temppayments;
                    tempinfo = tempaccount.GetType().GetProperties().ToList();
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

        private async Task Updatestepfile(string stringtoenter)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await storageFolder.CreateFileAsync("step.txt", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(storageFile, stringtoenter);
        }

        private async Task<string> ReadStepFile()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await storageFolder.CreateFileAsync("step.txt", CreationCollisionOption.OpenIfExists);
            return FileIO.ReadTextAsync(storageFile).GetResults();
        }
    }
}