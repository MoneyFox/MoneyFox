using System;
using NLog;

namespace MoneyFox.Presentation
{
    public partial class App 
    {
        public App ()
        {
            StyleHelper.Init();
            InitializeComponent();

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                LogManager.GetCurrentClassLogger().Fatal(e);
            };
        }
    }
}
