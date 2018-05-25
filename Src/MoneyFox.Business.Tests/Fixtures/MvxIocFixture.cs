using System;
using MvvmCross.Base;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Tests;

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
