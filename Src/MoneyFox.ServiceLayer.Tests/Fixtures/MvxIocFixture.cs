using System;
using System.Diagnostics.CodeAnalysis;
using MvvmCross.Base;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Tests;

namespace MoneyFox.ServiceLayer.Tests.Fixtures
{
    [ExcludeFromCodeCoverage]
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
