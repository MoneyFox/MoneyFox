using Android.Content;
using Autofac;
using Autofac.Extras.MvvmCross;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;
using MvvmCross.Forms.Droid.Platform;
using MvvmCross.Forms.Platform;

namespace MoneyFox.Droid
{
    public class Setup : MvxFormsAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {
        }

        /// <inheritdoc />
        protected override IMvxIoCProvider CreateIocProvider()
        {
            var cb = new ContainerBuilder();
            cb.RegisterModule<DroidModule>();
            return new AutofacMvxIocProvider(cb.Build());
        }

        protected override IMvxApplication CreateApp()
        {
            return new MoneyFox.CoreApp();
        }

        protected override MvxFormsApplication CreateFormsApplication()
        {
            Strings.Culture = new Src.Localize().GetCurrentCultureInfo();
            return new App();
        }
    }
}