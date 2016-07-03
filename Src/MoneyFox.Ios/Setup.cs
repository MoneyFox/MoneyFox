using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using MoneyFox.Shared;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;
using UIKit;

namespace MoneyFox.Ios {
    public class Setup : MvxIosSetup {

        public Setup(IMvxApplicationDelegate applicationDelegate, UIWindow window) 
            : base(applicationDelegate, window) {
        }

        public Setup(IMvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter) 
            : base(applicationDelegate, presenter) {
        }

        protected override void InitializeFirstChance() {
            base.InitializeFirstChance();

            //Mvx.RegisterType<IDialogService, DialogService>();
            //Mvx.RegisterType<IOneDriveAuthenticator, OneDriveAuthenticator>();
            Mvx.RegisterType<IProtectedData, ProtectedData>();
            //Mvx.RegisterType<INotificationService, NotificationService>();
        }

        protected override IMvxApplication CreateApp() {
            Strings.Culture = new Localize().GetCurrentCultureInfo();

            return new App();
        }

        protected override IMvxTrace CreateDebugTrace() => new DebugTrace();

    }
}