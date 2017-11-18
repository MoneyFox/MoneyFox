using Windows.UI.Xaml.Controls;
using Autofac;
using Autofac.Extras.MvvmCross;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Cheesebaron.MvxPlugins.Settings.WindowsUWP;
using MoneyFox.Business;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;
using MvvmCross.Platform.UI;
using MvvmCross.Plugins.Email;
using MvvmCross.Plugins.File;
using MvvmCross.Plugins.WebBrowser;
using PluginLoader = MvvmCross.Plugins.Messenger.PluginLoader;
using MvvmCross.Platform.IoC;
using MvvmCross.Plugins.Email.Uwp;
using MvvmCross.Plugins.File.Uwp;
using MvvmCross.Plugins.Visibility.Uwp;
using MvvmCross.Plugins.WebBrowser.Uwp;
using MvvmCross.Uwp.Platform;

namespace MoneyFox.Windows
{
    public class Setup : MvxWindowsSetup
    {
        public Setup(Frame frame)
            : base(frame)
        {
        }

        /// <inheritdoc />
        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            base.LoadPlugins(pluginManager);
            pluginManager.EnsurePluginLoaded<PluginLoader>();

            //We have to do this here, since the loading via bootloader won't work for UWP projects
            Mvx.RegisterType<IMvxComposeEmailTask, MvxComposeEmailTask>();
            Mvx.RegisterType<IMvxWebBrowserTask, MvxWebBrowserTask>();
            Mvx.RegisterType<IMvxFileStore, MvxWindowsCommonFileStore>();
            Mvx.RegisterType<ISettings, WindowsUwpSettings>();
            Mvx.RegisterType<IMvxNativeVisibility, MvxWinRTVisibility>();
        }

        /// <inheritdoc />
        protected override IMvxIoCProvider CreateIocProvider()
        {
            var cb = new ContainerBuilder();

            cb.RegisterModule<BusinessModule>();
            cb.RegisterModule<WindowsModule>();

            return new AutofacMvxIocProvider(cb.Build());
        }

        /// <inheritdoc />
        protected override IMvxApplication CreateApp() => new MoneyFox.Business.App();
    }
}