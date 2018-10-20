using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Media.Capture;
using Windows.ApplicationModel.VoiceCommands;
using MoneyFox.Foundation.Resources;
using System.Xml.Linq;
using MoneyFox.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;

namespace MoneyFox.Windows
{
    public static class CortanaFunctions
    {
        static MediaCapture mc;
        static ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;
        public static async Task EnableMicrophone()
        {
            try
            {

                 mc = new MediaCapture();
                await mc.InitializeAsync();
                                        
            }
            catch (Exception e)
            {
                await Logging(e.ToString());
            }
        }
        public static async Task IntializeCortana()
        {
            try
            {
                await EnableMicrophone();

            }
            catch (Exception e)
            {

                await Logging(e.ToString());
            }
            try
                {

                    await CreateVCDfilesAndInstallAsync();

                }
                catch (Exception e)
                {
                await Logging(e.ToString());
                }
        }
        public static async Task CreateVCDfilesAndInstallAsync()
        {
            XNamespace xds = "http://schemas.microsoft.com/voicecommands/1.2";
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile dynamciallycreatedfile = await storageFolder.CreateFileAsync("createaccount.xml", CreationCollisionOption.OpenIfExists);

            XDocument xd = new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement(xds + "VoiceCommands"));
            XElement CommandSet = new XElement(xds+"CommandSet");
            CommandSet.SetAttributeValue(XNamespace.Xml + "lang",System.Globalization.CultureInfo.CurrentCulture.ToString());
            CommandSet.SetAttributeValue("Name","MoneyFox_" + System.Globalization.CultureInfo.CurrentCulture.ToString());
            XElement CommandPrefix = new XElement(xds+ "CommandPrefix");
            CommandPrefix.SetValue($"{Strings.CortanaVoiceCommandCommandPrefix}");
            XElement Example = new XElement(xds+"Example");
            Example.SetValue($"{Strings.CortanaVoiceCommandCommandPrefixExample}");
            CommandSet.Add(CommandPrefix);
            CommandSet.Add(Example);
            xd.Root.Add(CommandSet);
            XElement Createaccount = new XElement(xds+"Command");
            XElement CreateaccountExample = new XElement(xds+"Example");
            XElement Createaccountlistenfor = new XElement(xds+"ListenFor");
            XElement Createaccountfeedback = new XElement(xds+"Feedback");
            XElement Targetcreateaccountservice = new XElement(xds+"VoiceCommandService");
            Targetcreateaccountservice.SetAttributeValue("Target", "CortanaCreateAccount");
            Createaccount.SetAttributeValue("Name", "create-account");
            CreateaccountExample.SetValue($"{Strings.CortanaVoiceCommandCreateAccountExample}");
            Createaccountlistenfor.SetValue($"{Strings.CortanaVoiceCommandCreateAccountListenFor}");
            Createaccountfeedback.SetValue($"{Strings.CortanaVoiceCommandCreateAccountFeedback}");
            Createaccount.Add(CreateaccountExample);
            Createaccount.Add(Createaccountlistenfor);
            Createaccount.Add(Createaccountfeedback);
            Createaccount.Add(Targetcreateaccountservice);
            CommandSet.Add(Createaccount);

            XElement accountname = new XElement(xds+"Command");
            XElement accountnameExample = new XElement(xds+"Example");
            XElement accountnamelistenfor = new XElement(xds+"ListenFor");
            XElement accountnamefeedback = new XElement(xds+"Feedback");
            accountname.SetAttributeValue("Name", "account-name");
            XElement Targetaccountnameservice = new XElement(xds + "VoiceCommandService");
            Targetaccountnameservice.SetAttributeValue("Target", "CortanaAccountName");
            accountnameExample.SetValue($"{Strings.CortanaVoiceCommandAccountNameExample}");
            accountnamelistenfor.SetValue($"{Strings.CortanaVoiceCommandAccountNameListenFor}");
            accountnamefeedback.SetValue($"{Strings.CortanaVoiceCommandAccountNameFeedback}");
            accountname.Add(accountnameExample);
            accountname.Add(accountnamelistenfor);
            accountname.Add(accountnamefeedback);
            accountname.Add(Targetaccountnameservice);
            CommandSet.Add(accountname);

        
            XElement accountamount = new XElement(xds+"Command");
            XElement accountamountExample = new XElement(xds+"Example");
            XElement accountamountlistenfor = new XElement(xds+"ListenFor");
            XElement accountamountfeedback = new XElement(xds+"Feedback");
            XElement Targetaccountamountservice = new XElement(xds + "VoiceCommandService");
            Targetaccountamountservice.SetAttributeValue("Target", "CortanaAccountAmount");
            accountamount.SetAttributeValue("Name", "account-amount");
            accountamountExample.SetValue($"{Strings.CortanaVoiceCommandAccountAmountExample}");
            accountamountlistenfor.SetValue($"{Strings.CortanaVoiceCommandAccountAmountListenFor}");
            accountamountfeedback.SetValue($"{Strings.CortanaVoiceCommandAccountAmountFeedback}");
            accountamount.Add(accountamountExample);
            accountamount.Add(accountamountlistenfor);
            accountamount.Add(accountamountfeedback);
            accountamount.Add(Targetaccountamountservice);
            CommandSet.Add(accountamount);

            
            XElement createpayment = new XElement(xds+"Command");
            XElement createpaymentExample = new XElement(xds+"Example");
            XElement createpaymentlistenfor = new XElement(xds+"ListenFor");
            XElement createpaymentfeedback = new XElement(xds+"Feedback");
            createpayment.SetAttributeValue("Name", "create-payment");
            createpaymentExample.SetValue($"{Strings.CortanaVoiceCommandCreatePaymentExample}");
            XElement Targetcreatepaymentservice = new XElement(xds + "VoiceCommandService");
            Targetcreatepaymentservice.SetAttributeValue("Target", "CortanaCreatePayment");
            createpaymentlistenfor.SetValue($"{Strings.CortanaVoiceCommandCreatePaymentListenFor}");
            createpaymentfeedback.SetValue($"{Strings.CortanaVoiceCommandCreatePaymentFeedback}");
            createpayment.Add(createpaymentExample);
            createpayment.Add(createpaymentlistenfor);
            createpayment.Add(createpaymentfeedback);
            createpayment.Add(Targetcreatepaymentservice);
            CommandSet.Add(createpayment);
            

            XElement paymentamount = new XElement(xds+"Command");
            XElement paymentamountExample = new XElement(xds+"Example");
            XElement paymentamountlistenfor = new XElement(xds+"ListenFor");
            XElement paymentamountfeedback = new XElement(xds+"Feedback");
            paymentamount.SetAttributeValue("Name", "payment-amount");
            paymentamountExample.SetValue($"{Strings.CortanaVoiceCommandPaymentAmountExample}");
            XElement Targetpaymentamountservice = new XElement(xds + "VoiceCommandService");
            Targetpaymentamountservice.SetAttributeValue("Target", "CortanaPaymentAmount");
            paymentamountlistenfor.SetValue($"{Strings.CortanaVoiceCommandPaymentAmountListenFor}");
            paymentamountfeedback.SetValue($"{Strings.CortanaVoiceCommandPaymentAmountFeedback}");
            paymentamount.Add(paymentamountExample);
            paymentamount.Add(paymentamountlistenfor);
            paymentamount.Add(paymentamountfeedback);
            paymentamount.Add(Targetpaymentamountservice);
            CommandSet.Add(paymentamount);
           

            XElement paymentdate = new XElement(xds+"Command");
            XElement paymentdateExample = new XElement(xds+"Example");
            XElement paymentdatelistenfor = new XElement(xds+"ListenFor");
            XElement paymentdatefeedback = new XElement(xds+"Feedback");
            paymentdate.SetAttributeValue("Name", "payment-date");
            paymentdateExample.SetValue($"{Strings.CortanaVoiceCommandPaymentDateExample}");
            XElement Targetpaymentdateservice = new XElement(xds + "VoiceCommandService");
            Targetpaymentdateservice.SetAttributeValue("Target", "CortanaPaymentDate");
            paymentdatelistenfor.SetValue($"{Strings.CortanaVoiceCommandPaymentDateListenFor}");
            paymentdatefeedback.SetValue($"{Strings.CortanaVoiceCommandPaymentDateFeedback}");
            paymentdate.Add(paymentdateExample);
            paymentdate.Add(paymentdatelistenfor);
            paymentdate.Add(paymentdatefeedback);
            paymentdate.Add(Targetpaymentdateservice);
            CommandSet.Add(paymentdate);
 
            
            XElement dollars = new XElement(xds+"PhraseTopic");
            dollars.SetAttributeValue("Scenario", "Natural Language");
            dollars.SetAttributeValue("Label", "amount");
            XElement dollarssubject = new XElement(xds+"Subject");
            dollarssubject.SetValue("Natural Language");
            dollars.Add(dollarssubject);
            CommandSet.Add(dollars);
      
            
            XElement dates = new XElement(xds+"PhraseTopic");
            dates.SetAttributeValue("Scenario", "Natural Language");
            dates.SetAttributeValue("Label", "date");
            XElement datessubject = new XElement(xds+"Subject");
            datessubject.SetValue("Natural Language");
            dates.Add(datessubject);
            CommandSet.Add(dates);
           
            XElement accountnametopic = new XElement(xds+"PhraseTopic");
            accountnametopic.SetAttributeValue("Scenario", "Natural Language");
            accountnametopic.SetAttributeValue("Label", "name");
            XElement accountnamesubject = new XElement(xds+"Subject");
            accountnamesubject.SetValue("Natural Language");
            accountnametopic.Add(accountnamesubject);
            CommandSet.Add(accountnametopic);
           
            xd.Save(dynamciallycreatedfile.Path);
            dynamciallycreatedfile = await StorageFile.GetFileFromPathAsync(dynamciallycreatedfile.Path);
            try
            {
                await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(dynamciallycreatedfile);
            }
            catch (Exception e)
            {
                await Logging(e.ToString());
            }
       
        }
        public static VoiceCommandContentTile CreateTile(string Title, string Text)
        {
            VoiceCommandContentTile vcct = new VoiceCommandContentTile
            {
                Title = Title ?? Strings.ApplicationTitle,
                ContentTileType = VoiceCommandContentTileType.TitleWithText,
                TextLine1 = Text ?? null,
            };
            return vcct;
        }
        public static string SemanticInterpretation(string interpretationKey, VoiceCommand command)
        {
            return command.SpeechRecognitionResult.SemanticInterpretation.Properties[interpretationKey].ToString();

        }
        public static VoiceCommandUserMessage CreateUserMessage(string spokenmessage, string displaymessage)
        {
            VoiceCommandUserMessage vcum = new VoiceCommandUserMessage()
            {
                DisplayMessage = displaymessage ?? GetResourceString("CortanaUserMessageDefaultErrorMessage"),
                SpokenMessage = spokenmessage ?? GetResourceString("CortanaUserMessageDefaultErrorMessage")
            };

            return vcum;
        }
        public static object DeserializeAsync(string paymenttypes)
        {
            List<PropertyInfo> tempinfo = new List<PropertyInfo>();
            object te = new object();
            switch (paymenttypes)
             {

                case "payment":
                    {
                        var tes = (PaymentEntity)te;

                        tempinfo = tes.GetType().GetProperties().ToList();
                        foreach (PropertyInfo x in tempinfo)
                        {
                            var z = tes.GetType().GetProperty(x.Name);
                            z.SetValue(tes, LocalSettings.Containers["payment"].Values[x.Name]);
                        }
                        return tes;
                    }
                case "recurring":
                    {
                        var tes=(RecurringPaymentEntity)te;
                        tempinfo = tes.GetType().GetProperties().ToList();
                        foreach (PropertyInfo x in tempinfo)
                        {
                            var z = tes.GetType().GetProperty(x.Name);
                            z.SetValue(tes, LocalSettings.Containers["recurring"].Values[x.Name]);
                        }
                        return tes;
                    }
                case "account":
                    {
                        var tes = (AccountEntity)te;
                        tempinfo = tes.GetType().GetProperties().ToList();
                        foreach (PropertyInfo x in tempinfo)
                        {
                            var z = tes.GetType().GetProperty(x.Name);
                            z.SetValue(tes, LocalSettings.Containers["account"].Values[x.Name]);
                        }
                        return tes;
                    }
                default:
                    return null;
            }
        }
        public static string GetResourceString(string resourcekey)
        {
            System.Resources.ResourceManager keyValuePairs = Strings.ResourceManager;
            return keyValuePairs.GetString(resourcekey);
        }
        public static async Task Logging(string texttolog)
        {
            StorageFolder stors = ApplicationData.Current.LocalFolder;
            await FileIO.AppendTextAsync(await stors.CreateFileAsync("logfile.txt", CreationCollisionOption.OpenIfExists), texttolog);
            await FileIO.AppendTextAsync(await stors.CreateFileAsync("logfile.txt", CreationCollisionOption.OpenIfExists), Environment.NewLine);
        }
        public static void SerializeAsync(object temppayments, string paymenttypes)
        {
            //StorageFolder storageFolder = ApplicationData.Current.LocalCacheFolder;
            //StorageFile contentFile = await storageFolder.CreateFileAsync("contnent.json", CreationCollisionOption.ReplaceExisting);
            List<PropertyInfo> tempinfo = new List<PropertyInfo>();
            switch (paymenttypes)
            {

                case "payment":
                    { 
                    LocalSettings.CreateContainer("payment", ApplicationDataCreateDisposition.Always);
                    LocalSettings.Containers["payment"].Values["type"] = paymenttypes;
                    var temppayment = (PaymentEntity)temppayments;
                    tempinfo = temppayment.GetType().GetProperties().ToList();

                    foreach (PropertyInfo x in tempinfo)
                    {
                        LocalSettings.Containers["payment"].Values[x.Name] = x.GetValue(temppayment);

                    }
                    }
                    break;
                case "recurring":
                    {
                        LocalSettings.CreateContainer("recurring", ApplicationDataCreateDisposition.Always);
                        LocalSettings.Containers["recurring"].Values["type"] = paymenttypes;
                        var temppayment = (RecurringPaymentEntity)temppayments;
                        tempinfo = temppayment.GetType().GetProperties().ToList();

                        foreach (PropertyInfo x in tempinfo)
                        {
                            LocalSettings.Containers["recurring"].Values[x.Name] = x.GetValue(temppayment);

                        }
                    }
                    break;
                case "account":
                    {
                        LocalSettings.CreateContainer("account", ApplicationDataCreateDisposition.Always);
                        LocalSettings.Containers["account"].Values["type"] = paymenttypes;
                        var tempaccount = (AccountEntity)temppayments;
                        tempinfo = tempaccount.GetType().GetProperties().ToList();
                        foreach (PropertyInfo x in tempinfo)
                        {
                            LocalSettings.Containers["account"].Values[x.Name] = x.GetValue(tempaccount);
                        }
                    }
                    break;
                default:
                    return;
            }
            //Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            //foreach (PropertyInfo x in tempinfo)
            //{
            //    keyValuePairs.Add(x.Name, x.GetValue(temppayments).ToString());
            //    LocalSettings.Values[x.Name] = x.GetValue(temppayments).ToString();
            //}

            //string json = JsonConvert.SerializeObject(keyValuePairs);
            //await FileIO.WriteTextAsync(contentFile, json);
        }
        public static void Updatestepfile(string stringtoenter)
        {
            LocalSettings.Values["step"] = stringtoenter;
        }
        public static string ReadStepFile()
        {
            return (string)LocalSettings.Values["step"];
        }
        
    }
}

