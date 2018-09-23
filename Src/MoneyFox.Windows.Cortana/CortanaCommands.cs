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

namespace MoneyFox.Windows.Cortana
{
    public sealed class CortanaCommands : IBackgroundTask
    {
        ThreadPoolTimer poolTimer;
        BackgroundTaskDeferral serviceDeferral;
        ResourceMap cortanaResourceMap;
        ResourceContext cortanaContext;
        AccountService accountService = new AccountService(new AmbientDbContextLocator(), new DbContextScopeFactory());
        PaymentService paymentService = new PaymentService(new AmbientDbContextLocator(), new DbContextScopeFactory());
        VoiceCommandUserMessage UserMessage;
        VoiceCommandResponse voiceCommandResponse;
        StorageFolder storageFolder = ApplicationData.Current.LocalCacheFolder;
        StorageFile stepFile;
        StorageFile contentFile;
        string commandType;
        string LastStep;
        TimeSpan timeSpan = TimeSpan.FromSeconds(5);
        Dictionary<string, PropertyInfo> objectInfo = new Dictionary<string, PropertyInfo>();
        object payaccountinfo;
        PaymentEntity payment;
         AccountEntity account;
        AppServiceTriggerDetails triggerDetails;
        VoiceCommandServiceConnection voiceCommandServiceConnection;



        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            serviceDeferral = taskInstance.GetDeferral();
           /// taskInstance.Canceled += OnTaskCanceled;
            CreateThreadPoolTimer(timeSpan);
            triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            voiceCommandServiceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails);
            VoiceCommand command = await voiceCommandServiceConnection.GetVoiceCommandAsync();
            cortanaResourceMap = ResourceManager.Current.MainResourceMap;
            cortanaContext = ResourceContext.GetForViewIndependentUse();
            string rule = command.SpeechRecognitionResult.RulePath[0];
            stepFile = await storageFolder.CreateFileAsync("file.txt", CreationCollisionOption.OpenIfExists);
            var te = FileIO.ReadTextAsync(stepFile)?.GetResults()?.Split(",");
            commandType =te?[1];
            LastStep = te?[0];

            if (triggerDetails != null && triggerDetails.Name == "MoneyFoxCortanaIntegration")
            {
                switch (commandType)
                {
                    case "payment":
                        payment = (PaymentEntity)await DeserializeAsync("payment");
                        break;
                   case "recurring":
                         payment= (PaymentEntity)await DeserializeAsync("payment");
                        break;
                    case "account":
                        account = (AccountEntity) await DeserializeAsync("account");
                        break;
                    default:
                        payment = new PaymentEntity();
                        account = new AccountEntity();
                        break;
                }
                switch (rule)
                {
                    case "create-account":
                        switch (LastStep)
                        {
                            case "":
                                UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessageAccountName"), GetResourceString("CortanaUserMessageAccountName"));
                                await SerializeAsync(account, "account");
                                await FileIO.WriteTextAsync(stepFile, "create-account,account");
                                await voiceCommandServiceConnection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(UserMessage));
                                poolTimer.Cancel();
                                serviceDeferral?.Complete();
                                break;
                            default:
                                UserMessage = CreateUserMessage(GetResourceString("CortanaIncorrectStepError"), GetResourceString("CortanaIncorrectStepError"));
                                await voiceCommandServiceConnection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(UserMessage));
                                poolTimer?.Cancel();
                                serviceDeferral?.Complete();
                                return;
                               
                        }
                        break;
                    case "account-name":
                        if (!LastStep.Contains("payment"))
                        {
                            switch (LastStep)
                            {
                                case "create-acccount":
                                    account.Name = SemanticInterpretation("name", command);
                                    await SerializeAsync(account, "account");
                                    await FileIO.WriteTextAsync(stepFile, "");
                                    await FileIO.WriteTextAsync(stepFile, "account-name,account");
                                    poolTimer.Cancel();
                                    serviceDeferral?.Complete();
                                    break;
                                case "account-amount":
                                    Account ac = new Account();
                                    account.Name = SemanticInterpretation("name", command);
                                    ac.Data = account;
                                    await accountService.SaveAccount(ac);
                                    await FileIO.WriteTextAsync(stepFile, "");
                                    poolTimer.Cancel();
                                    serviceDeferral?.Complete();
                                    return;
                                                                   
                                default:
                                    await voiceCommandServiceConnection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(CreateUserMessage(GetResourceString("CortanaUserMessageAccountNameError"), GetResourceString("CortanaUserMessageAccountNameError"))));
                                    poolTimer.Cancel();
                                    serviceDeferral?.Complete();
                                    return;
                            }
                            await voiceCommandServiceConnection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(CreateUserMessage(GetResourceString("CortanaUserMessageAccountAmount"), GetResourceString("CortanaUserMessageAccountAmount"))));
                            poolTimer.Cancel();
                            serviceDeferral?.Complete();
                                
                        }
                        else
                        {
                            UserMessage = CreateUserMessage(GetResourceString("CortanaIncorrectStepError"), GetResourceString("CortanaIncorrectStepError"));
                            await voiceCommandServiceConnection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(UserMessage));
                            poolTimer?.Cancel();
                            serviceDeferral?.Complete();
                            return;
                        }
                        break;
                    case "account-amount":
                        if (!LastStep.Contains("payment"))
                        {
                            switch (LastStep)
                            {
                                case "create-acccount":
                                    account.CurrentBalance = double.Parse(SemanticInterpretation("amount", command));
                                    await SerializeAsync(account, "account");
                                    await FileIO.WriteTextAsync(stepFile, "");
                                    await FileIO.WriteTextAsync(stepFile, "account-amount,account");
                                    poolTimer.Cancel();
                                    serviceDeferral?.Complete();
                                    break;
                                case "account-name":
                                    Account ac = new Account();
                                    account.CurrentBalance = double.Parse(SemanticInterpretation("amount", command));
                                    ac.Data = account;
                                    await accountService.SaveAccount(ac);
                                    await FileIO.WriteTextAsync(stepFile, "");
                                    poolTimer.Cancel();
                                    serviceDeferral?.Complete();
                                    break;
                                default:
                                    await voiceCommandServiceConnection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(CreateUserMessage(GetResourceString("CortanaUserMessageAccountNameError"), GetResourceString("CortanaUserMessageAccountNameError"))));
                                    poolTimer.Cancel();
                                    serviceDeferral?.Complete();
                                    return;
                            }
                            await voiceCommandServiceConnection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(CreateUserMessage(GetResourceString("CortanaUserMessageAccountAmount"), GetResourceString("CortanaUserMessageAccountAmount"))));
                            poolTimer.Cancel();
                            serviceDeferral?.Complete();

                        }
                        else
                        {
                            UserMessage = CreateUserMessage(GetResourceString("CortanaIncorrectStepError"), GetResourceString("CortanaIncorrectStepError"));
                            await voiceCommandServiceConnection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(UserMessage));
                            poolTimer?.Cancel();
                            serviceDeferral?.Complete();
                            return;
                        }
                        break;
                    case "create-payment":
                        switch (LastStep)
                        {
                            case "":
                                List<VoiceCommandContentTile> allAccounttiles = new List<VoiceCommandContentTile>();
                                List<VoiceCommandContentTile> Allothercontents = new List<VoiceCommandContentTile>();
                                var Accounts = await accountService.GetAllAccounts();
                                if (Accounts != null || Accounts.Count() == 0)
                                {
                                    foreach (Account item in Accounts)
                                    {
                                        allAccounttiles.Add(CreateTile(item.Data.Name, string.Format(GetResourceString("CortanaContentTitleIncomeAccount"), item.Data.Name)));
                                    }
                                }
                                else
                                {
                                    UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessageNoAccountsPaymentCreationSpoken"), GetResourceString("CortanaUserMessageNoAccountsPaymentCreationText"));
                                    voiceCommandResponse = VoiceCommandResponse.CreateResponse(UserMessage);
                                    await voiceCommandServiceConnection.ReportSuccessAsync(voiceCommandResponse);
                                    serviceDeferral?.Complete();

                                }
                                UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessageReoccuranceSpoken"), GetResourceString("CortanaUserMessageReoccuranceText"));
                                voiceCommandResponse = VoiceCommandResponse.CreateResponse(UserMessage);
                                var payementreoccurs = Enum.GetNames(typeof(PaymentRecurrence)).Cast<string>().ToList();
                                for (int i = 0; i < payementreoccurs.Count - 1; i++)
                                {
                                    Allothercontents.Add(CreateTile(payementreoccurs[i],string.Format(GetResourceString("CortanaContentTileReoccuranceText"),GetResourceString(payementreoccurs[i]+"Label"))));
                                }
                                Allothercontents.Add(CreateTile("None",string.Format(GetResourceString("CortanaContentTileReoccuranceText"),GetResourceString("CortanaContentTileNoRecurranceTitle"))));
                                voiceCommandResponse = VoiceCommandResponse.CreateResponse(UserMessage, Allothercontents);
                                VoiceCommandDisambiguationResult vcdr = await voiceCommandServiceConnection.RequestDisambiguationAsync(voiceCommandResponse);
                                if (vcdr != null)
                                {
                                    var reoccur = vcdr.SelectedItem.Title;
                                    if (reoccur ==  "None" || reoccur == null)
                                    {
                                        payment = new PaymentEntity();
                                         payment.IsRecurring= false;
                                        commandType = "payment";
                                        await SerializeAsync(payment, "payment");
                                        await FileIO.WriteTextAsync(stepFile, "");
                                        await FileIO.WriteTextAsync(stepFile, "create-payment,payment");
                                    }
                                    else
                                    {
                                        payment.IsRecurring = true;
                                        payment.RecurringPayment = new RecurringPaymentEntity();
                                        payment.RecurringPayment.Recurrence = (PaymentRecurrence)Enum.Parse(typeof(PaymentRecurrence), vcdr.SelectedItem.Title);
                                        commandType = "reccuring";
                                        await SerializeAsync(payment, "payment");
                                        await FileIO.WriteTextAsync(stepFile, "");
                                        await FileIO.WriteTextAsync(stepFile, "create-payment,recurring");
                                    }

                                }

                                UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessagePaymentTypeSpoken"),GetResourceString("CortanaUserMessagePaymentTypeText"));
                                allAccounttiles.Add(CreateTile(GetResourceString("AddIncomeLabel"),GetResourceString("CortanaContentTilePaymentTypeText")));
                                allAccounttiles.Add(CreateTile( GetResourceString("AddExpenseLabel"),GetResourceString("CortanaContentTilePaymentTypeText")));
                                allAccounttiles.Add(CreateTile(GetResourceString("AddTransferLabel"), GetResourceString("CortanaContentTilePaymentTypeText")));
                                voiceCommandResponse = VoiceCommandResponse.CreateResponse(UserMessage, allAccounttiles);
                                vcdr = await voiceCommandServiceConnection.RequestDisambiguationAsync(voiceCommandResponse);
                                allAccounttiles.Clear();
                                if (vcdr.SelectedItem.Title == GetResourceString("AddIncomeLabel"))
                                {
                                    foreach (Account item in Accounts)
                                    {
                                        allAccounttiles.Add(CreateTile(item.Data.Name, string.Format(GetResourceString("CortanaContentTitleIncomeAccount"), item.Data.Name)));
                                    }
                                    UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessageWhichAccount"), GetResourceString("CortanaUserMessageWhichAccount"));
                                    voiceCommandResponse = VoiceCommandResponse.CreateResponse(UserMessage, allAccounttiles);
                                    vcdr = await voiceCommandServiceConnection.RequestDisambiguationAsync(voiceCommandResponse);
                                    if (vcdr != null)
                                    {
                                        
                                        switch (commandType)
                                        {
                                            case "payment":
                                                payment.ChargedAccount = Accounts.FirstOrDefault<Account>(x => x.Data.Name == vcdr.SelectedItem.Title).Data;
                                                payment.Type = PaymentType.Income;
                                                await SerializeAsync(payment, "payment");
                                                await FileIO.WriteTextAsync(stepFile, "");
                                                await FileIO.WriteTextAsync(stepFile, "create-payment,payment");
                                                break;
                                            case "reccuring":
                                                payment.RecurringPayment.ChargedAccount= Accounts.FirstOrDefault<Account>(x => x.Data.Name == vcdr.SelectedItem.Title).Data;
                                                payment.RecurringPayment.Type = PaymentType.Income;
                                                await SerializeAsync(payment, "payment");
                                                await FileIO.WriteTextAsync(stepFile, "");
                                                await FileIO.WriteTextAsync(stepFile, "create-payment,recurring");
                                                break;
                                            default:
                                                break;
                                        }

                                    }
                                    UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessagePaymentAmountSpoken"), GetResourceString("CortanaUserMessagePaymentAmountText"));
                                    voiceCommandResponse = VoiceCommandResponse.CreateResponse(UserMessage);

                                    await voiceCommandServiceConnection.ReportSuccessAsync(voiceCommandResponse);
                                    poolTimer?.Cancel();
                                }
                                else if (vcdr.SelectedItem.Title == GetResourceString("AddExpenseLabel"))
                                    
                                {
                                    foreach (Account item in Accounts)
                                    {
                                        allAccounttiles.Add(CreateTile(item.Data.Name, string.Format(GetResourceString("CortanaContentTitleExpenseAccount"), item.Data.Name)));
                                    }
                                    UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessageWhichAccount"), GetResourceString("CortanaUserMessageWhichAccount"));
                                    voiceCommandResponse = VoiceCommandResponse.CreateResponse(UserMessage, allAccounttiles);
                                    vcdr = await voiceCommandServiceConnection.RequestDisambiguationAsync(voiceCommandResponse);
                                    if (vcdr != null)
                                    {
                                        switch (commandType)
                                        {
                                            case "payment":
                                                payment.ChargedAccount = Accounts.FirstOrDefault<Account>(x => x.Data.Name == vcdr.SelectedItem.Title).Data;
                                                payment.Type = PaymentType.Expense;
                                                await SerializeAsync(payment, "payment");
                                                await FileIO.WriteTextAsync(stepFile, "");
                                                await FileIO.WriteTextAsync(stepFile, "create-payment,payment");
                                                break;
                                            case "reccuring":
                                                payment.RecurringPayment.ChargedAccount = Accounts.FirstOrDefault<Account>(x => x.Data.Name == vcdr.SelectedItem.Title).Data;
                                                payment.RecurringPayment.Type = PaymentType.Expense;
                                                await SerializeAsync(payment, "payment");
                                                await FileIO.WriteTextAsync(stepFile, "");
                                                await FileIO.WriteTextAsync(stepFile, "create-payment,recurring");
                                                break;
                                            default:
                                                break;
                                        }

                                    }
                                    UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessagePaymentAmountSpoken"), GetResourceString("CortanaUserMessagePaymentAmountText"));
                                    voiceCommandResponse = VoiceCommandResponse.CreateResponse(UserMessage);

                                    await voiceCommandServiceConnection.ReportSuccessAsync(voiceCommandResponse);
                                    poolTimer?.Cancel();
                                    serviceDeferral?.Complete();

                                }
                                else if (vcdr.SelectedItem.Title == GetResourceString("AddTransferLabel"))
                                {
                                    foreach (Account item in Accounts)
                                    {
                                        allAccounttiles.Add(CreateTile(item.Data.Name, string.Format(GetResourceString("CortanaContentTileTransferFromAccount"), item.Data.Name)));
                                    }

                                    UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessageWhichAccount"),GetResourceString("CortanaUserMessageWhichAccount"));
                                    voiceCommandResponse = VoiceCommandResponse.CreateResponse(UserMessage, allAccounttiles);
                                    vcdr = await voiceCommandServiceConnection.RequestDisambiguationAsync(voiceCommandResponse);
                                    if (vcdr != null)
                                    {
                                        switch (commandType)
                                        {
                                            case "payment":
                                                payment.ChargedAccount = Accounts.FirstOrDefault<Account>(x => x.Data.Name == vcdr.SelectedItem.Title).Data;
                                                payment.Type = PaymentType.Transfer;
                                                await SerializeAsync(payment, "payment");
                                                await FileIO.WriteTextAsync(stepFile, "");
                                                await FileIO.WriteTextAsync(stepFile, "create-payment,payment");
                                                break;
                                            case "reccuring":
                                                payment.RecurringPayment.ChargedAccount = Accounts.FirstOrDefault<Account>(x => x.Data.Name == vcdr.SelectedItem.Title).Data;
                                                payment.RecurringPayment.Type = PaymentType.Transfer;
                                                await SerializeAsync(payment, "payment");
                                                await FileIO.WriteTextAsync(stepFile, "");
                                                await FileIO.WriteTextAsync(stepFile, "create-payment,recurring");
                                                break;
                                            default:
                                                UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessageDefaultErrorMessage"), GetResourceString("CortanaUserMessageDefaultErrorMessage"));
                                                await voiceCommandServiceConnection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(UserMessage));
                                                serviceDeferral?.Complete();
                                                return;
                                        }

                                    }
                                    foreach (Account item in Accounts)
                                    {
                                        allAccounttiles.Add(CreateTile(item.Data.Name, string.Format(GetResourceString("CortanaContentTileTransferToAccount"), item.Data.Name)));
                                    }
                                    UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessageWhichAccount"), GetResourceString("CortanaUserMessageWhichAccount"));
                                    voiceCommandResponse = VoiceCommandResponse.CreateResponse(UserMessage, allAccounttiles);
                                    vcdr = await voiceCommandServiceConnection.RequestDisambiguationAsync(voiceCommandResponse);
                                    if (vcdr != null)
                                    {
                                        switch (commandType)
                                        {
                                            case "payment":
                                                payment.TargetAccount = Accounts.FirstOrDefault<Account>(x => x.Data.Name == vcdr.SelectedItem.Title).Data;
                                                payment.Type = PaymentType.Transfer;
                                                await SerializeAsync(payment, "payment");
                                                await FileIO.WriteTextAsync(stepFile, "");
                                                await FileIO.WriteTextAsync(stepFile, "create-payment,payment");

                                                break;
                                            case "reccuring":
                                                payment.RecurringPayment.TargetAccount = Accounts.FirstOrDefault<Account>(x => x.Data.Name == vcdr.SelectedItem.Title).Data;
                                                payment.RecurringPayment.Type = PaymentType.Transfer;
                                                await SerializeAsync(payment, "payment");
                                                await FileIO.WriteTextAsync(stepFile, "");
                                                await FileIO.WriteTextAsync(stepFile, "create-payment,recurring");
                                                break;
                                            default:
                                                UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessageDefaultErrorMessage"), GetResourceString("CortanaUserMessageDefaultErrorMessage"));
                                                await voiceCommandServiceConnection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(UserMessage));
                                                poolTimer.Cancel();
                                                serviceDeferral?.Complete();
                                                return;
                                                
                                        }

                                    }

                                    UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessagePaymentAmountSpoken"), GetResourceString("CortanaUserMessagePaymentAmountText"));
                                    voiceCommandResponse = VoiceCommandResponse.CreateResponse(UserMessage);

                                    await voiceCommandServiceConnection.ReportSuccessAsync(voiceCommandResponse);
                                    poolTimer?.Cancel();
                                    serviceDeferral?.Complete();
                                }
                                else
                                {
                                    await voiceCommandServiceConnection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(CreateUserMessage(GetResourceString("CortanaUserMessagePaymentTypeNoSelected"), GetResourceString("CortanaUserMessagePaymentTypeNoSelected"))));
                                    await FileIO.WriteTextAsync(stepFile, "");
                                    poolTimer?.Cancel();
                                    serviceDeferral?.Complete();
                                    return;
                                    
                                    
                                }

                                break;
                            default:
                                UserMessage = CreateUserMessage(GetResourceString("CortanaIncorrectStepError"),GetResourceString("CortanaIncorrectStepError"));
                                await voiceCommandServiceConnection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(UserMessage));
                                await FileIO.WriteTextAsync(stepFile, "");
                                poolTimer?.Cancel();
                                serviceDeferral?.Complete();
                                return;
                        }

                                break;
                    case "payment-amount":
                        double amount = double.Parse(SemanticInterpretation("dollars", command) + "." + SemanticInterpretation("cents", command));
                        if (!LastStep.Contains("account"))
                        {                           
                                  payment = (PaymentEntity)await DeserializeAsync("payment");
                             
                          
                            switch (LastStep)
                            {
                                case "create-payment":
                                    switch (commandType)
                                    {
                                        case "payment":
                                           payment.Amount = amount;
                                            await FileIO.WriteTextAsync(stepFile, "");
                                            await FileIO.WriteTextAsync(stepFile, "payment-amount,payment");
                                            await SerializeAsync(payment, "payment");
                                            break;
                                        case "recurring":
                                           payment.RecurringPayment.Amount = amount;
                                            await FileIO.WriteTextAsync(stepFile, "");
                                            await FileIO.WriteTextAsync(stepFile, "payment-amount,recurring");
                                            await SerializeAsync(payment, "payment");
                                            break;
                                        default:
                                            UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessageDefaultErrorMessage"), GetResourceString("CortanaUserMessageDefaultErrorMessage"));
                                            await voiceCommandServiceConnection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(UserMessage));
                                            poolTimer.Cancel();
                                            serviceDeferral?.Complete();
                                            return;
                                    }
                                    UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessagePaymentDateSpoken"), GetResourceString("CortanaUserMessagePaymentDateText"));
                                    await voiceCommandServiceConnection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(UserMessage));
                                    poolTimer.Cancel();
                                    serviceDeferral?.Complete();
                                    break;
                                case "payment-date":
                                    Payment pm = new Payment();
                                    switch (commandType)
                                    {
                                        case "payment":
                                            payment.Amount = amount;
                                            pm.Data = payment;
                                            await paymentService.SavePayments(pm);
                                            await FileIO.WriteTextAsync(stepFile, "");
                                            poolTimer.Cancel();
                                            serviceDeferral?.Complete();
                                            return;
                                        case "recurring":
                                            payment.RecurringPayment.Amount = amount;
                                            pm.Data = payment;
                                            await paymentService.SavePayments(pm);
                                            await FileIO.WriteTextAsync(stepFile, "");
                                            poolTimer.Cancel();
                                            serviceDeferral?.Complete();
                                            return;
                                        default:
                                            UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessageDefaultErrorMessage"), GetResourceString("CortanaUserMessageDefaultErrorMessage"));
                                            await voiceCommandServiceConnection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(UserMessage));
                                            poolTimer.Cancel();
                                            serviceDeferral?.Complete();
                                            return;
                                    }
                                    
                                default:
                                    UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessageDefaultErrorMessage"), GetResourceString("CortanaUserMessageDefaultErrorMessage"));
                                    await voiceCommandServiceConnection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(UserMessage));
                                    poolTimer.Cancel();
                                    serviceDeferral?.Complete();
                                    return;
                            }

                        }
                       else
                        {
                            UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessagePaymentAmountError"), GetResourceString("CortanaUserMessagePaymentAmountError"));
                            await voiceCommandServiceConnection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(UserMessage));
                            poolTimer.Cancel();
                            serviceDeferral?.Complete();
                            return;

                        }
                        break;
                    case "payment-date":
                        DateTime dt = DateTime.Parse(SemanticInterpretation("date", command));
                        if (!LastStep.Contains("account"))
                        {
                            payment = (PaymentEntity)await DeserializeAsync("payment");
                            
                            switch (LastStep)
                            {
                                case "create-payment":
                                   
                                    switch (commandType)
                                    {
                                        case "payment":
                                            payment.Date = dt;
                                            await FileIO.WriteTextAsync(stepFile, "");
                                            await FileIO.WriteTextAsync(stepFile, "payment-date,payment");
                                            await SerializeAsync(payment, "payment");
                                            break;
                                        case "recurring":
                                            payment.RecurringPayment.StartDate = dt;
                                            await FileIO.WriteTextAsync(stepFile, "");
                                            await FileIO.WriteTextAsync(stepFile, "payment-date,recurring");
                                            await SerializeAsync(payment, "payment");
                                            break;
                                        default:
                                            UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessageDefaultErrorMessage"), GetResourceString("CortanaUserMessageDefaultErrorMessage"));
                                            await voiceCommandServiceConnection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(UserMessage));
                                            poolTimer.Cancel();
                                            serviceDeferral?.Complete();
                                            return;
                                    }
                                    UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessagePaymentAmountSpoken"), GetResourceString("CortanaUserMessagePaymentAmountText"));
                                    await voiceCommandServiceConnection.ReportSuccessAsync(VoiceCommandResponse.CreateResponse(UserMessage));
                                    poolTimer.Cancel();
                                    serviceDeferral?.Complete();
                                    break;
                                case "payment-amount":
                                    Payment pm = new Payment();
                                    switch (commandType)
                                    {
                                        case "payment":
                                            payment.Date = dt;
                                            pm.Data = payment;
                                            await paymentService.SavePayments(pm);
                                            await FileIO.WriteTextAsync(stepFile, "");
                                            poolTimer.Cancel();
                                            serviceDeferral?.Complete();
                                            return;
                                        case "recurring":
                                            payment.RecurringPayment.StartDate = dt;
                                            pm.Data = payment;
                                            await paymentService.SavePayments(pm);
                                            await FileIO.WriteTextAsync(stepFile, "");
                                            poolTimer.Cancel();
                                            serviceDeferral?.Complete();
                                            return;
                                        default:
                                            UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessageDefaultErrorMessage"), GetResourceString("CortanaUserMessageDefaultErrorMessage"));
                                            await voiceCommandServiceConnection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(UserMessage));
                                            poolTimer.Cancel();
                                            serviceDeferral?.Complete();
                                            return;
                                    }
                                    
                                default:
                                    UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessagePaymentDateError"), GetResourceString("CortanaUserMessagePaymentDateError"));
                                    await voiceCommandServiceConnection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(UserMessage));
                                    poolTimer.Cancel();
                                    serviceDeferral?.Complete();
                                    return;
                            }

                        }
                        else
                        {
                            UserMessage = CreateUserMessage(GetResourceString("CortanaUserMessagePaymentAmountError"), GetResourceString("CortanaUserMessagePaymentAmountError"));
                            await voiceCommandServiceConnection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(UserMessage));
                            poolTimer.Cancel();
                            serviceDeferral?.Complete();
                            return;
                        }
                        break;
                    default:
                        UserMessage = CreateUserMessage(GetResourceString("CortanaCommandError"), GetResourceString("CortanaCommandError"));
                        await voiceCommandServiceConnection.ReportFailureAsync(VoiceCommandResponse.CreateResponse(UserMessage));
                        poolTimer.Cancel();
                        serviceDeferral?.Complete();
                        return;
                     
                }




            }
        }

        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            poolTimer?.Cancel();
            serviceDeferral?.Complete();

        }

        private string SemanticInterpretation(string interpretationKey, VoiceCommand command)
        {
            return command.SpeechRecognitionResult.SemanticInterpretation.Properties[interpretationKey].FirstOrDefault();
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


        private VoiceCommandUserMessage CreateUserMessage(string spokenmessage, string displaymessage)
        {
            VoiceCommandUserMessage vcum = new VoiceCommandUserMessage()
            {
                DisplayMessage = displaymessage ?? GetResourceString("CortanaUserMessageDefaultErrorMessage"),
                SpokenMessage = spokenmessage ??  GetResourceString("CortanaUserMessageDefaultErrorMessage")
            };

            return vcum;
        }
        

        private async Task SerializeAsync(object temppayments, string paymenttypes)
        {
            contentFile = await storageFolder.CreateFileAsync("contnent.json", CreationCollisionOption.ReplaceExisting);
            List<PropertyInfo> tempinfo = new List<PropertyInfo>();
            switch (paymenttypes)
            {

                case "payment":
                    var temppayment = (DataAccess.Entities.PaymentEntity)temppayments;
                    tempinfo = temppayment.GetType().GetProperties().ToList();
                    break;
                case "account":
                    var tempaccount = (DataAccess.Entities.AccountEntity)temppayments;
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

        

        private void CreateThreadPoolTimer(TimeSpan timeSpan)
        {
            poolTimer = ThreadPoolTimer.CreatePeriodicTimer(async (source) =>
            {
                UserMessage = CreateUserMessage(GetResourceString("CortanaContinuationMessage"), GetResourceString("CortanaContinuationMessage"));
                await this.voiceCommandServiceConnection.ReportProgressAsync(VoiceCommandResponse.CreateResponse(UserMessage));
            },
    timeSpan,
    (source) =>
    {

    });
        }


        private async Task<object> DeserializeAsync(string paymenttypes)
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

        private async Task Save(object sender,string type)
        {
            switch (type)
            {
                case "payment":
                    await paymentService.SavePayments((Payment)sender);
                    break;
                case "account":
                    await accountService.SaveAccount((Account)sender);
                    break;
                case "reccuring":
                    await paymentService.SavePayments((Payment)sender);
                        break;
                   default:
                    break;
            }

        }
        private string GetResourceString(string resourcekey)
        {
            ResourceLoader keyValuePairs = ResourceLoader.GetForViewIndependentUse();
            return keyValuePairs.GetString(resourcekey);
        }
    }
}
