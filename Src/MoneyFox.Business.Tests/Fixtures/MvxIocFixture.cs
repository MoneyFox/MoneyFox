using System;
using MvvmCross.Platform.Core;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Test.Core;

namespace MoneyFox.Business.Tests.Fixtures
{
    public class MvxIocFixture : MvxIoCSupportingTest, IDisposable
    {
        public MvxIocFixture()
        {
            Setup();

            Ioc.RegisterSingleton<IMvxMessenger>(new MvxMessengerHub());
        }

        public void Dispose()
        {
            MvxSingleton.ClearAllSingletons();
            ClearAll();
        }
    }
}
