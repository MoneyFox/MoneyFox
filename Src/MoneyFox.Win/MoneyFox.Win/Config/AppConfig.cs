namespace MoneyFox.Win.Config;

using Windows.ApplicationModel;
using Microsoft.Extensions.Configuration;

internal partial class AppConfig
{
    private readonly IConfigurationRoot configurationRoot;

    public AppConfig()
    {
        var builder = new ConfigurationBuilder().SetBasePath(Package.Current.InstalledLocation.Path).AddJsonFile(path: "appsettings.json", optional: false);
        configurationRoot = builder.Build();
    }

    public AppCenterConfig AppCenter => GetSection<AppCenterConfig>(nameof(AppCenter));

    private T GetSection<T>(string key)
    {
        return configurationRoot.GetSection(key).Get<T>();
    }
}
