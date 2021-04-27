using CommonServiceLocator;
using MediatR;
using Microsoft.AppCenter.Crashes;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Payments.Commands.ClearPayments;
using MoneyFox.Application.Payments.Commands.CreateRecurringPayments;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MoneyFox.Uwp.Services
{
    public class StartupTasksService
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected StartupTasksService() { }

        public static async Task StartupAsync()
        {
            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            try
            {
                IMediator mediator = ServiceLocator.Current.GetInstance<IMediator>();
                if(!settingsFacade.IsBackupAutouploadEnabled || !settingsFacade.IsLoggedInToBackupService)
                {
                    await mediator.Send(new ClearPaymentsCommand());
                    await mediator.Send(new CreateRecurringPaymentsCommand());
                    return;
                }

                IBackupService backupService = ServiceLocator.Current.GetInstance<IBackupService>();
                await backupService.RestoreBackupAsync();

                await mediator.Send(new ClearPaymentsCommand());
                await mediator.Send(new CreateRecurringPaymentsCommand());

                logger.Info("Backup synced.");
            }
            catch(Exception ex)
            {
                logger.Fatal(ex);
                Crashes.TrackError(ex, new Dictionary<string, string> { { "Context", "Startup." } });
            }
            finally
            {
                settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            }
        }
    }
}
