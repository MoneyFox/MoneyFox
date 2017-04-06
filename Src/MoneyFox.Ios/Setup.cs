using Autofac;
using Autofac.Extras.MvvmCross;
using MoneyFox.Business;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Support.JASidePanels;
using MvvmCross.iOS.Support.SidePanels;
using MvvmCross.iOS.Support.XamarinSidebar;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform.Platform;

namespace MoneyFox.Ios 
{
    public class Setup : MvxIosSetup 
	{

        public Setup(MvxApplicationDelegate appDelegate, IMvxIosViewPresenter presenter)
			: base(appDelegate, presenter){}

        protected override IMvxIoCProvider CreateIocProvider()
        {
            var cb = new ContainerBuilder();

            cb.RegisterModule<BusinessModule>();
            cb.RegisterModule<IosModule>();

            return new AutofacMvxIocProvider(cb.Build());
        }

        protected override void InitializeFirstChance() 
		{
            base.InitializeFirstChance();
        }

		protected override IMvxIosViewPresenter CreatePresenter()
		{
			return new MvxSidePanelsPresenter((MvxApplicationDelegate)ApplicationDelegate, Window);
		}

        protected override IMvxApplication CreateApp() 
		{
            Strings.Culture = new Localize().GetCurrentCultureInfo();

            return new App();
        }

        protected override IMvxTrace CreateDebugTrace() => new DebugTrace();

    }
}