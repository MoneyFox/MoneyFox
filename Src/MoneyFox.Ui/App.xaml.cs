namespace MoneyFox.Ui;

using System.Globalization;
using Common.Exceptions;
using Core.ApplicationCore.UseCases.DbBackup;
using Core.Commands.Payments.ClearPayments;
using Core.Commands.Payments.CreateRecurringPayments;
using Core.Common.Facades;
using Core.Common.Helpers;
using Core.Common.Interfaces;
using Infrastructure.Adapters;
using InversionOfControl;
using MediatR;
using Serilog;
using ViewModels;

public partial class App
{
    private bool isRunning;

    public App()
    {
        var settingsFacade = new SettingsFacade(new SettingsAdapter());

        // TODO: use setting?
        CultureHelper.CurrentCulture = new(CultureInfo.CurrentCulture.Name);
        InitializeComponent();
        SetupServices();

        ResourceDictionary = new Dictionary<string, ResourceDictionary>();
        foreach (var dictionary in Resources.MergedDictionaries)
        {
            string key = dictionary.Source.OriginalString.Split(';').First().Split('/').Last().Split('.').First(); // Alternatively If you are good in Regex you can use that as well
            ResourceDictionary.Add(key, dictionary);
        }

        MainPage = DeviceInfo.Current.Idiom == DeviceIdiom.Desktop
                   || DeviceInfo.Current.Idiom == DeviceIdiom.Tablet
                   || DeviceInfo.Current.Idiom == DeviceIdiom.TV
            ? new AppShellDesktop()
            : new AppShell();

        if (!settingsFacade.IsSetupCompleted)
        {
            Shell.Current.GoToAsync(Routes.WelcomeViewRoute).Wait();
        }
    }

    public static Dictionary<string,ResourceDictionary> ResourceDictionary { get; set; }

    public static Action<IServiceCollection>? AddPlatformServicesAction { get; set; }

    private static IServiceProvider? ServiceProvider { get; set; }

    internal static TViewModel GetViewModel<TViewModel>() where TViewModel : BaseViewModel
    {
        return ServiceProvider?.GetService<TViewModel>() ?? throw new ResolveViewModelException<TViewModel>();
    }

    protected override void OnStart()
    {
        StartupTasksAsync().ConfigureAwait(false);
    }

    protected override void OnResume()
    {
        StartupTasksAsync().ConfigureAwait(false);
    }

    private static void SetupServices()
    {
        var services = new ServiceCollection();
        AddPlatformServicesAction?.Invoke(services);
        new MoneyFoxConfig().Register(services);
        ServiceProvider = services.BuildServiceProvider();
        ServiceProvider.GetService<IAppDbContext>()?.Migratedb();
    }

    private async Task StartupTasksAsync()
    {
        // Don't execute this again when already running
        if (isRunning)
        {
            return;
        }

        if (ServiceProvider == null)
        {
            return;
        }

        isRunning = true;
        var settingsFacade = ServiceProvider.GetService<ISettingsFacade>() ?? throw new ResolveDependencyException<ISettingsFacade>();
        var mediator = ServiceProvider.GetService<IMediator>() ?? throw new ResolveDependencyException<IMediator>();
        try
        {
            if (settingsFacade.IsBackupAutoUploadEnabled && settingsFacade.IsLoggedInToBackupService)
            {
                var backupService = ServiceProvider.GetService<IBackupService>() ?? throw new ResolveDependencyException<IBackupService>();
                await backupService.RestoreBackupAsync();
            }

            _ = await mediator.Send(new ClearPaymentsCommand());
            _ = await mediator.Send(new CreateRecurringPaymentsCommand());
            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
        }
        catch (Exception ex)
        {
            Log.Fatal(exception: ex, messageTemplate: "Error during startup");
        }
        finally
        {
            isRunning = false;
        }
    }
}
