using MvvmCross.Droid.Shared.Caching;

namespace MoneyFox.Droid.Activities.Caching
{
    internal class FragmentCacheConfigurationCustomFragmentInfo :
        FragmentCacheConfiguration<MainActivityFragmentCacheInfoFactory.SerializableCustomFragmentInfo>
    {
        private readonly MainActivityFragmentCacheInfoFactory mainActivityFragmentCacheInfoFactory;

        public FragmentCacheConfigurationCustomFragmentInfo()
        {
            mainActivityFragmentCacheInfoFactory = new MainActivityFragmentCacheInfoFactory();
        }

        public override MvxCachedFragmentInfoFactory MvxCachedFragmentInfoFactory
            => mainActivityFragmentCacheInfoFactory;
    }
}