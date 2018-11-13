#if !DEBUG
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using PCLAppConfig;
#endif

namespace MoneyFox
{
	public partial class App
    {
		public App ()
        {
            StyleHelper.Init();
            InitializeComponent();
#if !DEBUG
            AppCenter.Start($"ios={ConfigurationManager.AppSettings["IosAppcenterSecret"]};" +
                            $"uwp={ConfigurationManager.AppSettings["WindowsAppcenterSecret"]};" +
                            $"android={ConfigurationManager.AppSettings["AndroidAppcenterSecret"]}",
                            typeof(Analytics), typeof(Crashes));
#endif
        }
    }
}
