using Autofac;
using Autofac.Extras.MvvmCross;
using MoneyFox.Business;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Support.XamarinSidebar;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform.Platform;
using UIKit;

namespace MoneyFox.Ios 
{
    public class Setup : MvxIosSetup 
	{
		public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
			: base(applicationDelegate, window)
		{
		}

		public Setup(MvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
			: base(applicationDelegate, presenter)
		{
		}

		protected override IMvxTrace CreateDebugTrace()
		{
			return new DebugTrace();
		}

		protected override IMvxIosViewPresenter CreatePresenter()
		{
			return new MvxSidebarPresenter((MvxApplicationDelegate)ApplicationDelegate, Window);
		}

		protected override IMvxApplication CreateApp()
		{
			Strings.Culture = new Localize().GetCurrentCultureInfo();

			return new App();
		}

		protected override IMvxIoCProvider CreateIocProvider()
		{
			var cb = new ContainerBuilder();

            cb.RegisterModule<IosModule>();

			return new AutofacMvxIocProvider(cb.Build());
		}
    }
}