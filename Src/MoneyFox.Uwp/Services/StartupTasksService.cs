using CommonServiceLocator;
using MediatR;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Payments.Commands.ClearPayments;
using MoneyFox.Application.Payments.Commands.CreateRecurringPayments;
using NLog;
using System;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.Services
{
    public class StartupTasksService
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static async Task StartupAsync()
        {
            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            var mediator = ServiceLocator.Current.GetInstance<IMediator>();
            if(!settingsFacade.IsBackupAutouploadEnabled || !settingsFacade.IsLoggedInToBackupService)
            {
                await mediator.Send(new ClearPaymentsCommand());
                await mediator.Send(new CreateRecurringPaymentsCommand());
                return;
            }

            try
            {
                var backupService = ServiceLocator.Current.GetInstance<IBackupService>();
                await backupService.RestoreBackupAsync();

                await mediator.Send(new ClearPaymentsCommand());
                await mediator.Send(new CreateRecurringPaymentsCommand());

                logger.Info("Backup synced.");
            }
            catch(Exception ex)
            {
                logger.Fatal(ex);
                throw;
            }
            finally
            {
                settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            }
        }
    }
}
