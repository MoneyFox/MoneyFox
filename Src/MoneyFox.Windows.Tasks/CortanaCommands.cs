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
using Windows.Foundation.Collections;
using MoneyFox.Windows.Business;

namespace MoneyFox.Windows.Tasks
{
    public sealed class CortanaCommands : IBackgroundTask
    {
        BackgroundTaskDeferral serviceDeferral;
        PaymentEntity payment;
        AccountEntity account;
        AccountService Accounts;
        PaymentService Payments;
      

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            serviceDeferral = taskInstance.GetDeferral();
            taskInstance.Canceled += TaskInstance_Canceled;
            var local_folder = ApplicationData.Current.LocalFolder;
            AppServiceTriggerDetails trigger = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            VoiceCommandServiceConnection connection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(trigger);
            VoiceCommand voiceCommand = await connection.GetVoiceCommandAsync();
            VoiceCommandUserMessage userMessage;
            VoiceCommandUserMessage repromptMessage;
            AppServiceConnection newconnection = new AppServiceConnection();
            newconnection.PackageFamilyName = "57598ApplySolutionsSoftwa.MoneyFox";
            AppServiceConnectionStatus status;
            AppServiceResponse response;
            var rule = voiceCommand.SpeechRecognitionResult.RulePath[0];
           string commandType = "";

            switch (rule)
            {
                case "create-payment":
                    {
                    
                        string step = await ReadStepFile();
                       if (!step.Contains("account")&&step=="")
                        {
                            ValueSet vs = new ValueSet();
                            userMessage = CreateUserMessage(GetResourceString("CortanaUserMessageReoccuranceSpoken"), GetResourceString("CortanaUserMessageReoccuranceText"));
                            List<VoiceCommandContentTile> Allothercontents = new List<VoiceCommandContentTile>();
                            var payementreoccurs = Enum.GetNames(typeof(PaymentRecurrence)).Cast<string>().ToList();
                            for (int i = 0; i < payementreoccurs.Count - 1; i++)
                            {
                                Allothercontents.Add(CreateTile(payementreoccurs[i], string.Format(GetResourceString("CortanaContentTileReoccuranceText"), GetResourceString(payementreoccurs[i] + "Label"))));
                            }

                            Allothercontents.Add(CreateTile(GetResourceString("CortanaContentTileNoRecurranceTitle"), GetResourceString("CortanaContentTileNoRecurranceText")));
                            repromptMessage = CreateUserMessage(GetResourceString("CortanaUserMessageReccuranceRepromptSpoken"), GetResourceString("CortanaUserMessageReccuranceRepromptDisplay"));
                            VoiceCommandResponse voiceCommandResponse = VoiceCommandResponse.CreateResponseForPrompt(userMessage, repromptMessage, Allothercontents);
                            VoiceCommandDisambiguationResult vcdr = await connection.RequestDisambiguationAsync(voiceCommandResponse);
                            if (vcdr != null)
                            {
                                var reoccur = vcdr.SelectedItem.Title;
                                if (reoccur == "None" || reoccur == null)
                                {
                                    vs.Add("recurring", "false");
                                    newconnection.AppServiceName = "CortanaCreatePayment";
                                    status = await newconnection.OpenAsync();
                                    if (status == AppServiceConnectionStatus.Success)
                                    {
                                        response = await newconnection.SendMessageAsync(vs);
                                    }
                                    else
                                    {
                                        await connection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageDefaultErrorMessage"),CortanaFunctions.GetResourceString("CortanaUserMessageDefaultErrorMessage"))));
                                        serviceDeferral?.Complete();
                                    }
                                    
                                }
                                else
                                {
                                 
                                    vs.Add("recurring", true);
                                    vs.Add("recurrs",reoccur);
                                    newconnection.AppServiceName = "CortanaCreateRecurringPayment";
                                    status = await newconnection.OpenAsync();
                                    if (status == AppServiceConnectionStatus.Success)
                                    {
                                        response = await newconnection.SendMessageAsync(vs);
                                        CortanaFunctions.SerializeAsync(response, "recurring");
                                    }
                                    else
                                    {
                                        await connection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(CortanaFunctions.CreateUserMessage(CortanaFunctions.GetResourceString("CortanaUserMessageDefaultErrorMessage"), CortanaFunctions.GetResourceString("CortanaUserMessageDefaultErrorMessage"))));
                                        serviceDeferral?.Complete();
                                    }
                                }

                            }
                            userMessage = CreateUserMessage(GetResourceString("CortanaUserMessagePaymentTypeSpoken"), GetResourceString("CortanaUserMessagePaymentTypeText"));
                            repromptMessage = CreateUserMessage(GetResourceString("CortanaUserMessagePaymentTypeRepromptSpoken"), GetResourceString("CortanaUserMessagePaymentTypeRepromptDisplay"));
                            Allothercontents.Clear();
                            Allothercontents.Add(CreateTile(GetResourceString("AddIncomeLabel"),string.Format(GetResourceString("CortanaContentTilePaymentTypeText"),GetResourceString("AddIncomeLabel"))));
                            Allothercontents.Add(CreateTile(GetResourceString("AddExpenseLabel"), GetResourceString("CortanaContentTilePaymentTypeText")));
                            Allothercontents.Add(CreateTile(GetResourceString("AddTransferLabel"), GetResourceString("CortanaContentTilePaymentTypeText")));



                            vcdr = await connection.RequestDisambiguationAsync(VoiceCommandResponse.CreateResponseForPrompt(userMessage, repromptMessage, Allothercontents));

                            if (vcdr.SelectedItem.Title == GetResourceString("AddIncomeLabel"))
                            {
                                userMessage = CreateUserMessage(GetResourceString("CortanaUserMessageWhichAccountSpoken"), string.Format(GetResourceString("CortanaUserMessageWhichAccountText"), GetResourceString("AddIncomeLabel")));
                                repromptMessage = CreateUserMessage(GetResourceString("CortanaUserMessageWhichAccountRepromptSpoken"), string.Format(GetResourceString("CortanaUserMessageWhichAccountRepromptText"), GetResourceString("AddIncomeLabel")));

                                Allothercontents.Clear();
                                if (newlistaccounts.Count() >= 2)
                                {
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
                                            break;

                                        case "recurring":
                                            payment.RecurringPayment.Type = PaymentType.Income;
                                            payment.RecurringPayment.ChargedAccount = newlistaccounts.FirstOrDefault<Account>(x => x.Data.Name == newvcdr.SelectedItem.Title).Data;
                                            await SerializeAsync(payment, "payment");
                                            await Updatestepfile("create-payment,recurring");
                                            break;

                                        default:
                                            break;
                                    }
                                }
                                else
                                {
                                    Allothercontents.Add(CreateTile(newlistaccounts[0].Data.Name, string.Format(GetResourceString("CortanaContentTitleIncomeAccount"), newlistaccounts[0].Data.Name)));
                                    var vcdr2 = await connection.RequestConfirmationAsync(VoiceCommandResponse.CreateResponse(CreateUserMessage(GetResourceString("CortanaUserMessageOnlyOneAccountIncomeSpoken"), string.Format(GetResourceString("CortanaUserMessageOnlyOneAccountIncomeText"), newlistaccounts[0].Data.Name))));
                                    if (vcdr2.Confirmed == true)
                                    {
                                        switch (commandType)
                                        {
                                            case "payment":
                                                payment.Type = PaymentType.Income;
                                                payment.ChargedAccount = newlistaccounts[0].Data;
                                                await SerializeAsync(payment, "payment");
                                                await Updatestepfile("create-payment,recurring");
                                                break;

                                            case "recurring":
                                                payment.RecurringPayment.Type = PaymentType.Income;
                                                payment.RecurringPayment.ChargedAccount = newlistaccounts[0].Data;
                                                await SerializeAsync(payment, "payment");
                                                await Updatestepfile("create-payment,recurring");
                                                break;

                                            default:
                                                break;
                                        }
                                    }
                                    serviceDeferral?.Complete();
                                }

                            }
                            else if (vcdr.SelectedItem.Title == GetResourceString("AddExpenseLabel"))
                            {
                                userMessage = CreateUserMessage(GetResourceString("CortanaUserMessageWhichAccountSpoken"), string.Format(GetResourceString("CortanaUserMessageWhichAccountText"), GetResourceString("AddExpenseLabel")));
                                repromptMessage = CreateUserMessage(GetResourceString("CortanaUserMessageWhichAccountRepromptSpoken"), string.Format(GetResourceString("CortanaUserMessageWhichAccountRepromptText"), GetResourceString("AddExpenseLabel")));
                                Allothercontents.Clear();
                                if (newlistaccounts.Count() >= 2)
                                {
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
                                            break;
                                        case "recurring":
                                            payment.RecurringPayment.Type = PaymentType.Expense;
                                            payment.RecurringPayment.ChargedAccount = newlistaccounts.FirstOrDefault<Account>(x => x.Data.Name == newvcdr.SelectedItem.Title).Data;
                                            await SerializeAsync(payment, "payment");
                                            await Updatestepfile("create-payment,recurring");
                                            break;

                                        default:
                                            break;
                                    }
                                }
                                else
                                {
                                    Allothercontents.Add(CreateTile(newlistaccounts[0].Data.Name, string.Format(GetResourceString("CortanaContentTitleIncomeAccount"), newlistaccounts[0].Data.Name)));
                                    var vcdr2 = await connection.RequestConfirmationAsync(VoiceCommandResponse.CreateResponse(CreateUserMessage(GetResourceString("CortanaUserMessageOnlyOneAccountExpenseSpoken"), string.Format(GetResourceString("CortanaUserMessageOnlyOneAccountExpenseText"), newlistaccounts[0].Data.Name))));
                                    if (vcdr2.Confirmed == true)
                                    {
                                        switch (commandType)
                                        {
                                            case "payment":
                                                payment.Type = PaymentType.Income;
                                                payment.ChargedAccount = newlistaccounts[0].Data;
                                                await SerializeAsync(payment, "payment");
                                                await Updatestepfile("create-payment,recurring");
                                                break;

                                            case "recurring":
                                                payment.RecurringPayment.Type = PaymentType.Income;
                                                payment.RecurringPayment.ChargedAccount = newlistaccounts[0].Data;
                                                await SerializeAsync(payment, "payment");
                                                await Updatestepfile("create-payment,recurring");
                                                break;

                                            default:
                                                break;
                                        }
                                    }
                                    serviceDeferral?.Complete();
                                }
                            }
                            else if (vcdr.SelectedItem.Title == GetResourceString("AddTransferLabel"))
                            {
                                userMessage = CreateUserMessage(GetResourceString("CortanaUserMessageTransferFromAccountSpoken"), GetResourceString("CortanaUserMessageTransferFromAccountText"));
                                repromptMessage = CreateUserMessage(GetResourceString("CortanaUserMessageTransferFromAccountRepromptSpoken"), GetResourceString("CortanaUserMessageTransferFromAccountRepromptText"));
                                VoiceCommandUserMessage transferto;
                                VoiceCommandUserMessage transfertoreprompt;
                                transferto = CreateUserMessage(GetResourceString("CortanaUserMessageTransferToAccountSpoken"), GetResourceString("CortanaUserMessageTransferToAccountText"));
                                transfertoreprompt = CreateUserMessage(GetResourceString("CortanaUserMessageTransferToAccountRepromptSpoken"), GetResourceString("CortanaUserMessageTransferToAccountRepromptText"));
                                List<VoiceCommandContentTile> transferfrom = new List<VoiceCommandContentTile>();
                                List<VoiceCommandContentTile> transfertoaccount = new List<VoiceCommandContentTile>();
                                if (newlistaccounts.Count() >= 2)
                                {
                                    foreach (Account x in newlistaccounts)
                                    {
                                        transferfrom.Add(CreateTile(x.Data.Name, string.Format(GetResourceString("CortanaContentTileTransferFromAccount"), x.Data.Name)));
                                        transfertoaccount.Add(CreateTile(x.Data.Name, string.Format(GetResourceString("CortanaContentTileTransferToAccount"), x.Data.Name)));
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
                                                await SerializeAsync(payment, "payment");
                                                await Updatestepfile("create-payment,recurring");
                                                break;
                                            }
                                        case "recurring":
                                            {
                                                payment.RecurringPayment.Type = PaymentType.Transfer;
                                                payment.RecurringPayment.ChargedAccount = newlistaccounts.FirstOrDefault<Account>(x => x.Data.Name == newvcdr.SelectedItem.Title).Data;
                                                payment.RecurringPayment.TargetAccount = newlistaccounts.FirstOrDefault<Account>(x => x.Data.Name == anothervcdr.SelectedItem.Title).Data;
                                                await SerializeAsync(payment, "payment");
                                                await Updatestepfile("create-payment,recurring");
                                                break;
                                            }
                                        default:
                                            break;
                                    }
                                }
                                else
                                {
                                    await connection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(CreateUserMessage(GetResourceString("CortanaUserMessageOnlyOneAccountTransferSpoken"), GetResourceString("CortanaUserMessageOnlyOneAccountTransferText"))));
                                    serviceDeferral?.Complete();
                                }
                            }

                        }
                        else
                        {
                            userMessage = CreateUserMessage(GetResourceString("CortanaUserMessageIncorrectStepError"), GetResourceString("CortanaUserMessageIncorrectStepError"));
                            await connection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(userMessage));
                            serviceDeferral?.Complete();
                            return;
                        }
                        userMessage = CreateUserMessage(GetResourceString("CortanaUserMessagePaymentAmountSpoken"),GetResourceString("CortanaUserMessagePaymentAmountText"));
                        await connection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(userMessage));
                        serviceDeferral?.Complete();
                        break;
                    }
                case "payment-amount":
                    {
                        string step = await ReadStepFile();
                        if (!step.Contains("account"))
                        {
                            string paymenttype = step.Split(",")[0];
                            commandType = step.Split(",")[1];
                            double amount = double.Parse(SemanticInterpretation("amount", voiceCommand));
                            payment = (PaymentEntity)await DeserializeAsync("payment");
                            if (paymenttype == "create-payment")
                            {
                                switch (commandType)
                                {
                                    case "payment":
                                        payment.Amount = amount;
                                        await SerializeAsync(payment, "payment");
                                        await Updatestepfile("payment-amount,payment");
                                        break;
                                    case "recurring":
                                        payment.RecurringPayment.Amount = amount;
                                        await SerializeAsync(payment, "payment");
                                        await Updatestepfile("payment-amount,recurring");
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else if (paymenttype == "payment-date")
                            {
                                Payment savePayment = new Payment();
                                switch (commandType)
                                {
                                    case "payment":
                                        payment.Amount = amount;

                                        savePayment.Data = payment;
                                        await Payments.SavePayments(savePayment);
                                        await Updatestepfile("");
                                        serviceDeferral?.Complete();
                                        return;
                                    case "recurring":
                                        payment.RecurringPayment.Amount = amount;
                                        savePayment.Data = payment;
                                        await Payments.SavePayments(savePayment);
                                        await Updatestepfile("");
                                        serviceDeferral?.Complete();
                                        return;
                                    default:
                                        break;
                                }
                            }
                        }
                        else
                        {
                            userMessage = CreateUserMessage(GetResourceString("CortanaUserMessageIncorrectStepError"), GetResourceString("CortanaUserMessageIncorrectStepError"));
                            await connection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(userMessage));
                            serviceDeferral?.Complete();
                            return;
                        }
                        userMessage = CreateUserMessage(GetResourceString("CortanaUserMessagePaymentDateSpoken"),GetResourceString("CortanaUserMessagePaymentDateText"));
                        await connection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(userMessage));
                        serviceDeferral?.Complete();
                        break;
                    }
                case "payment-date":
                    {
                        string step = await ReadStepFile();
                        if (!step.Contains("account"))
                        {
                            string paymenttype = step.Split(",")[0];
                            commandType = step.Split(",")[1];
                            DateTime date = DateTime.Parse(SemanticInterpretation("date", voiceCommand));
                            payment = (PaymentEntity)await DeserializeAsync("payment");
                            if (paymenttype == "create-payment")
                            {
                                switch (commandType)
                                {
                                    case "payment":
                                        payment.Date = date;
                                        await SerializeAsync(payment, "payment");
                                        await Updatestepfile("payment-date,payment");
                                        break;
                                    case "recurring":
                                        payment.RecurringPayment.StartDate = date;
                                        await SerializeAsync(payment, "payment");
                                        await Updatestepfile("payment-date,recurring");
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else if (paymenttype == "payment-amount")
                            {
                                Payment savePayment = new Payment();
                                switch (commandType)
                                {
                                    case "payment":
                                        payment.Date = date;
                                        savePayment.Data = payment;
                                        await Payments.SavePayments(savePayment);
                                        await Updatestepfile("");
                                        serviceDeferral?.Complete();
                                        return;
                                    case "recurring":
                                        payment.RecurringPayment.StartDate = date;
                                        savePayment.Data = payment;
                                        await Payments.SavePayments(savePayment);
                                        await Updatestepfile("");
                                        serviceDeferral?.Complete();
                                        return;
                                    default:
                                        break;
                                }
                            }
                        }
                        else
                        {
                            userMessage = CreateUserMessage(GetResourceString("CortanaUserMessageIncorrectStepError"), GetResourceString("CortanaUserMessageIncorrectStepError"));
                            await connection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(userMessage));
                            serviceDeferral?.Complete();
                            return;

                        }
                        userMessage = CreateUserMessage(GetResourceString("CortanaUserMessagePaymentAmountSpoken"), GetResourceString("CortanaUserMessagePaymentAmountText"));
                        await connection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(userMessage));
                        serviceDeferral?.Complete();
                        break;
                    }
                case "create-account":
                    {
                        string step = await ReadStepFile();
                        if (!step.Contains("payment"))
                        {
                            account = new AccountEntity();
                            await SerializeAsync(account, "account");
                            await Updatestepfile("create-account,account");
                        }
                        else
                        {
                            userMessage = CreateUserMessage(GetResourceString("CortanaUserMessageIncorrectStepError"), GetResourceString("CortanaUserMessageIncorrectStepError"));
                            await connection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(userMessage));
                            serviceDeferral?.Complete();
                            return;
                        }
                        userMessage = CreateUserMessage(GetResourceString("CortanaUserMessageAccountName"), GetResourceString("CortanaUserMessageAccountName"));
                        await connection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(userMessage));
                        break;
                    }
                case "account-name":
                    {
                        string step = await ReadStepFile();
                        if (!step.Contains("payment"))
                        {
                            if (step.Split(",")[0] == "create-account")
                            {


                                account = (AccountEntity)await DeserializeAsync("account");
                                account.Name = SemanticInterpretation("name", voiceCommand);
                                await SerializeAsync(account, "account");
                                await Updatestepfile("account-name,account");
                            }
                            else if (step.Split(",")[0] == "account-amount")
                            {
                                Account savingAccount = new Account();
                                account.Name = SemanticInterpretation("name", voiceCommand);
                                await Accounts.SaveAccount(savingAccount);
                                await Updatestepfile("");
                                serviceDeferral?.Complete();
                                return;
                            }
                        }
                        else
                        {
                            userMessage = CreateUserMessage(GetResourceString("CortanaUserMessageIncorrectStepError"), GetResourceString("CortanaUserMessageIncorrectStepError"));
                            await connection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(userMessage));
                            serviceDeferral?.Complete();
                            return;
                        }
                        userMessage = CreateUserMessage(GetResourceString("CortanaUserMessageAccountAmount"),GetResourceString("CortanaUserMessageAccountAmount"));
                        await connection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(userMessage));
                        serviceDeferral?.Complete();
                        break;
                    }
                case "account-amount":
                    {
                        string step = await ReadStepFile();
                        if (!step.Contains("payment"))
                        {
                            if (step.Split(",")[0] == "create-account")
                            {
                                account = (AccountEntity)await DeserializeAsync("account");
                                account.CurrentBalance = double.Parse(SemanticInterpretation("amount", voiceCommand));
                                await SerializeAsync(account, "account");
                                await Updatestepfile("account-amount,account");
                            }
                            else if (step.Split(",")[0] == "account-name")
                            {
                                Account savingAccount = new Account();
                                account.CurrentBalance = double.Parse(SemanticInterpretation("amount", voiceCommand));
                                savingAccount.Data = account;
                                await Accounts.SaveAccount(savingAccount);
                                await Updatestepfile("");
                                serviceDeferral?.Complete();
                                return;
                            }
                        }
                        else
                        {
                            userMessage = CreateUserMessage(GetResourceString("CortanaUserMessageIncorrectStepError"), GetResourceString("CortanaUserMessageIncorrectStepError"));
                            await connection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(userMessage));
                            serviceDeferral?.Complete();
                            return;
                        }
                        userMessage = CreateUserMessage(GetResourceString("CortanaUserMessageAccountName"), GetResourceString("CortanaUserMessageAccountName"));
                        await connection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(userMessage));
                        serviceDeferral?.Complete();
                        break;
                    }
                default:
                    userMessage = CreateUserMessage(GetResourceString("CortanaCommandError"), GetResourceString("CortanaCommandError"));
                    await connection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(userMessage));
                    serviceDeferral?.Complete();
                    break;
            }

        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
         
            serviceDeferral?.Complete();
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
            await FileIO.AppendTextAsync(await stors.CreateFileAsync("logfile.txt", CreationCollisionOption.GenerateUniqueName), texttolog);
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
            var te = FileIO.ReadTextAsync(storageFile);
            
            return te.GetResults();
        }
    }
}