namespace MoneyFox.Ui.Platforms.Windows.Src;

using System.Security.Cryptography;
using global::Windows.Storage;
using Microsoft.Identity.Client;

internal static class TokenCacheHelper
{
    /// <summary>
    ///     Path to the token cache
    /// </summary>
    private static readonly string CacheFilePath = ApplicationData.Current.LocalFolder.Path + ".msalcache.bin";

    private static readonly object FileLock = new();

    private static void BeforeAccessNotification(TokenCacheNotificationArgs args)
    {
        lock (FileLock)
        {
            args.TokenCache.DeserializeMsalV3(
                File.Exists(CacheFilePath)
                    ? ProtectedData.Unprotect(encryptedData: File.ReadAllBytes(CacheFilePath), optionalEntropy: null, scope: DataProtectionScope.CurrentUser)
                    : null);
        }
    }

    private static void AfterAccessNotification(TokenCacheNotificationArgs args)
    {
        // if the access operation resulted in a cache update
        if (args.HasStateChanged)
        {
            lock (FileLock)
            {
                // reflect changes in the persistent store
                File.WriteAllBytes(
                    path: CacheFilePath,
                    bytes: ProtectedData.Protect(userData: args.TokenCache.SerializeMsalV3(), optionalEntropy: null, scope: DataProtectionScope.CurrentUser));
            }
        }
    }

    internal static void EnableSerialization(ITokenCache tokenCache)
    {
        tokenCache.SetBeforeAccess(BeforeAccessNotification);
        tokenCache.SetAfterAccess(AfterAccessNotification);
    }
}
