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


namespace MoneyFox.Windows.Tasks
{
    public sealed class CortanaPaymentDate : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            throw new NotImplementedException();
        }
    }
}