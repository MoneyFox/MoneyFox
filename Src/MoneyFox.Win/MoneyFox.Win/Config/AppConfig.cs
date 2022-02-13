using Microsoft.Extensions.Configuration;
using Windows.ApplicationModel;

namespace MoneyFox.Win;

internal partial class AppConfig
{
    private readonly IConfigurationRoot configurationRoot;

    public AppConfig()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Package.Current.InstalledLocation.Path)
            .AddJsonFile("appsettings.json", optional: false);

        configurationRoot = builder.Build();
    }

    public AppCenterConfig AppCenter => GetSection<AppCenterConfig>(nameof(AppCenter));

    private T GetSection<T>(string key) => configurationRoot.GetSection(key).Get<T>();

}
