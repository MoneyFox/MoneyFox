using System;
using MoneyFox.Business.Tests.Mocks;
using MvvmCross.Core.Platform;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Core;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Test.Core;

namespace MoneyFox.Business.Tests.Fixtures
{
    public class MvxIocFixture : MvxIoCSupportingTest, IDisposable
    {
        protected readonly MockDispatcher MockDispatcher;

        public MvxIocFixture()
        {
            Setup();

            MockDispatcher = new MockDispatcher();
            Ioc.RegisterSingleton<IMvxViewDispatcher>(MockDispatcher);
            Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(MockDispatcher);
            Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());
            Ioc.RegisterSingleton<IMvxMessenger>(new MvxMessengerHub());
        }

        public void Dispose()
        {
            MvxSingleton.ClearAllSingletons();
            ClearAll();
        }
    }
}
