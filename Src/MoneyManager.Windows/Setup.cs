using Windows.UI.Xaml.Controls;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsCommon.Platform;

namespace MoneyManager.Windows
{
    public class Setup : MvxWindowsSetup
    {
        public Setup(Frame frame)
            : base(frame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
#if DEBUG
            if (!Insights.IsInitialized)
            {
                Insights.Initialize("e5c4ac56bb1ca47559bc8d4973d0a8c4d78c7648", Application.Context);
            }
#endif

            return new Core.App();
        }
        
    }
}