using Autofac;
using Autofac.Extras.MvvmCross;
using MoneyFox.Foundation.Resources;
using MoneyFox.iOS;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;
using MvvmCross.Forms.iOS;
using MvvmCross.Forms.Platform;
using MvvmCross.iOS.Platform;
using UIKit;

namespace MoneyFox.Droid
{
    public class Setup : MvxFormsIosSetup
    {
        public Setup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        /// <inheritdoc />
        protected override IMvxIoCProvider CreateIocProvider()
        {
            var cb = new ContainerBuilder();
            cb.RegisterModule<IosModule>();
            return new AutofacMvxIocProvider(cb.Build());
        }

        protected override IMvxApplication CreateApp()
        {
            return new MoneyFox.CoreApp();
        }

        protected override MvxFormsApplication CreateFormsApplication()
        {
            return new MoneyFox.App();
        }
    }
}