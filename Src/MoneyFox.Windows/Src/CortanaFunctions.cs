using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Media.Capture;
using Windows.UI.Popups;
using Windows.ApplicationModel.VoiceCommands;
using MoneyFox.Foundation.Resources;
using System.Xml.Linq;


namespace MoneyFox.Windows
{
    public static class CortanaFunctions
    {
        static MediaCapture mc;
        static ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;
        static async Task EnableMicrophone()
        {
            try
            {
                 mc = new MediaCapture();
                await mc.InitializeAsync();
                    
            }
            catch (Exception e)
            {
                await LogErrors(e);
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

               await LogErrors(e);
            }
            try
            {
                
                await CreateVCDfileAndInstallAsync();

            }
            catch (Exception e)
            {
                await LogErrors(e);
            }





        }
        static async Task CreateVCDfileAndInstallAsync()
        {
            XNamespace xds = "http://schemas.microsoft.com/voicecommands/1.2";
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile dynamciallycreatedfile = await storageFolder.CreateFileAsync("voicecommandsdefinition.xml", CreationCollisionOption.OpenIfExists);
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
            XElement VoiceCommandService = new XElement(xds+"VoiceCommandService");
            VoiceCommandService.SetAttributeValue("Target", "MoneyFoxCortanaIntegration");
            Createaccount.SetAttributeValue("Name", "create-account");
            CreateaccountExample.SetValue($"{Strings.CortanaVoiceCommandCreateAccountExample}");
            Createaccountlistenfor.SetValue($"{Strings.CortanaVoiceCommandCreateAccountListenFor}");
            Createaccountfeedback.SetValue($"{Strings.CortanaVoiceCommandCreateAccountFeedback}");
            Createaccount.Add(CreateaccountExample);
            Createaccount.Add(Createaccountlistenfor);
            Createaccount.Add(Createaccountfeedback);
            Createaccount.Add(VoiceCommandService);
            CommandSet.Add(Createaccount);
           
            XElement accountname = new XElement(xds+"Command");
            XElement accountnameExample = new XElement(xds+"Example");
            XElement accountnamelistenfor = new XElement(xds+"ListenFor");
            XElement accountnamefeedback = new XElement(xds+"Feedback");
            accountname.SetAttributeValue("Name", "account-name");
            accountnameExample.SetValue($"{Strings.CortanaVoiceCommandAccountNameExample}");
            accountnamelistenfor.SetValue($"{Strings.CortanaVoiceCommandAccountNameListenFor}");
            accountnamefeedback.SetValue($"{Strings.CortanaVoiceCommandAccountNameFeedback}");
            accountname.Add(accountnameExample);
            accountname.Add(accountnamelistenfor);
            accountname.Add(accountnamefeedback);
            accountname.Add(VoiceCommandService);
            CommandSet.Add(accountname);

            XElement accountamount = new XElement(xds+"Command");
            XElement accountamountExample = new XElement(xds+"Example");
            XElement accountamountlistenfor = new XElement(xds+"ListenFor");
            XElement accountamountfeedback = new XElement(xds+"Feedback");
            accountamount.SetAttributeValue("Name", "account-amount");
            accountamountExample.SetValue($"{Strings.CortanaVoiceCommandAccountAmountExample}");
            accountamountlistenfor.SetValue($"{Strings.CortanaVoiceCommandAccountAmountListenFor}");
            accountamountfeedback.SetValue($"{Strings.CortanaVoiceCommandAccountAmountFeedback}");
            accountamount.Add(accountamountExample);
            accountamount.Add(accountamountlistenfor);
            accountamount.Add(accountamountfeedback);
            accountamount.Add(VoiceCommandService);
            CommandSet.Add(accountamount);

            XElement createpayment = new XElement(xds+"Command");
            XElement createpaymentExample = new XElement(xds+"Example");
            XElement createpaymentlistenfor = new XElement(xds+"ListenFor");
            XElement createpaymentfeedback = new XElement(xds+"Feedback");
            createpayment.SetAttributeValue("Name", "create-payment");
            createpaymentExample.SetValue($"{Strings.CortanaVoiceCommandCreatePaymentExample}");
            createpaymentlistenfor.SetValue($"{Strings.CortanaVoiceCommandCreatePaymentListenFor}");
            createpaymentfeedback.SetValue($"{Strings.CortanaVoiceCommandCreatePaymentFeedback}");
            createpayment.Add(createpaymentExample);
            createpayment.Add(createpaymentlistenfor);
            createpayment.Add(createpaymentfeedback);
            createpayment.Add(VoiceCommandService);
            CommandSet.Add(createpayment);
           
            XElement paymentamount = new XElement(xds+"Command");
            XElement paymentamountExample = new XElement(xds+"Example");
            XElement paymentamountlistenfor = new XElement(xds+"ListenFor");
            XElement paymentamountfeedback = new XElement(xds+"Feedback");
            paymentamount.SetAttributeValue("Name", "payment-amount");
            paymentamountExample.SetValue($"{Strings.CortanaVoiceCommandPaymentAmountExample}");
            paymentamountlistenfor.SetValue($"{Strings.CortanaVoiceCommandPaymentAmountListenFor}");
            paymentamountfeedback.SetValue($"{Strings.CortanaVoiceCommandPaymentAmountFeedback}");
            paymentamount.Add(paymentamountExample);
            paymentamount.Add(paymentamountlistenfor);
            paymentamount.Add(paymentamountfeedback);
            paymentamount.Add(VoiceCommandService);
            CommandSet.Add(paymentamount);

            XElement paymentdate = new XElement(xds+"Command");
            XElement paymentdateExample = new XElement(xds+"Example");
            XElement paymentdatelistenfor = new XElement(xds+"ListenFor");
            XElement paymentdatefeedback = new XElement(xds+"Feedback");
            paymentdate.SetAttributeValue("Name", "payment-date");
            paymentdateExample.SetValue($"{Strings.CortanaVoiceCommandPaymentDateExample}");
            
            paymentdatelistenfor.SetValue($"{Strings.CortanaVoiceCommandPaymentDateListenFor}");
            paymentdatefeedback.SetValue($"{Strings.CortanaVoiceCommandPaymentDateFeedback}");
            paymentdate.Add(paymentdateExample);
            paymentdate.Add(paymentdatelistenfor);
            paymentdate.Add(paymentdatefeedback);
            paymentdate.Add(VoiceCommandService);
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
                await LogErrors(e);
               
            }
        
          
        }
        static async Task LogErrors(Exception e)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalCacheFolder;
            StorageFile file = await storageFolder.CreateFileAsync("errors.txt", CreationCollisionOption.OpenIfExists);
            await FileIO.AppendTextAsync(file, e.ToString());
            await FileIO.AppendTextAsync(file, Environment.NewLine);
        }
    }
}
