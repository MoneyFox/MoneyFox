#region

using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Popups;
using Xamarin;

#endregion

namespace MoneyManager.Tasks.TransactionsWp
{
    internal class BackgroundTaskLogic
    {
        private const string name = "RecurringTransactionTask";

        public static async void RegisterBackgroundTask()
        {
            try {
                if (IsTaskExisting() || !await RequestAccess()) return;

                var builder = new BackgroundTaskBuilder();
                //Task soll alle 12 Stunden laufen
                var trigger = new TimeTrigger(720, false);

                builder.Name = name;
                builder.TaskEntryPoint = typeof (TransactionTask).FullName;
                builder.SetTrigger(trigger);
                builder.Register();
            } catch (Exception ex) {
                Insights.Report(ex, ReportSeverity.Error);
            }
        }

        private static async Task<bool> RequestAccess()
        {
            var result = await BackgroundExecutionManager.RequestAccessAsync();
            if (result == BackgroundAccessStatus.Denied)
            {
                var dialog = new MessageDialog("denied");
                await dialog.ShowAsync();

                return false;
            }
            return true;
        }

        private static async void RegistrationOnCompleted(BackgroundTaskRegistration sender,
            BackgroundTaskCompletedEventArgs args)
        {
            var dialog = new MessageDialog("mööp mööp");
            await dialog.ShowAsync();
        }

        private static bool IsTaskExisting()
        {
            return BackgroundTaskRegistration.AllTasks.Any(task => task.Value.Name == name);
        }
    }
}