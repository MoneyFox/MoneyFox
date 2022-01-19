using CommonServiceLocator;
using MediatR;
using Microsoft.AppCenter.Crashes;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.DbBackup;
using MoneyFox.Core.Commands.Payments.ClearPayments;
using MoneyFox.Core.Commands.Payments.CreateRecurringPayments;
using MoneyFox.Core.Interfaces;
using MoneyFox.Desktop.Infrastructure.Adapters;
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
                var mediator = ServiceLocator.Current.GetInstance<IMediator>();
                if(!settingsFacade.IsBackupAutouploadEnabled || !settingsFacade.IsLoggedInToBackupService)
                {
                    await mediator.Send(new ClearPaymentsCommand());
                    await mediator.Send(new CreateRecurringPaymentsCommand());
                    return;
                }

                var backupService = ServiceLocator.Current.GetInstance<IBackupService>();
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