namespace MoneyFox.Win;

using Microsoft.Extensions.Configuration;
using Windows.ApplicationModel;

internal partial class AppConfig
{
    private readonly IConfigurationRoot configurationRoot;

    public AppConfig()
    {
        IConfigurationBuilder? builder = new ConfigurationBuilder()
            .SetBasePath(Package.Current.InstalledLocation.Path)
            .AddJsonFile("appsettings.json", false);

        configurationRoot = builder.Build();
    }

    public AppCenterConfig AppCenter => GetSection<AppCenterConfig>(nameof(AppCenter));

    private T GetSection<T>(string key) => configurationRoot.GetSection(key).Get<T>();
}