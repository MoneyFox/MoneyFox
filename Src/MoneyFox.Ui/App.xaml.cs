namespace MoneyFox.Ui;

using System.Globalization;
using Common.Exceptions;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Facades;
using Core.Common.Helpers;
using Core.Common.Interfaces;
using Core.Features._Legacy_.Payments.ClearPayments;
using Core.Features._Legacy_.Payments.CreateRecurringPayments;
using Core.Features.DbBackup;
using Core.Interfaces;
using Infrastructure.Adapters;
using InversionOfControl;
using MediatR;
using Messages;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Views;

public partial class App
{
    private const string IS_CATEGORY_CLEANUP_EXECUTED_KEY_NAME = "IsCategoryCleanupExecuted";
    private bool isRunning;

    public App()
    {
        var settingsAdapter = new SettingsAdapter();
        var settingsFacade = new SettingsFacade(settingsAdapter);

        // TODO: use setting?
        CultureHelper.CurrentCulture = new(CultureInfo.CurrentCulture.Name);
        InitializeComponent();
        SetupServices();
        FillResourceDictionary();
        MainPage = DeviceInfo.Current.Idiom == DeviceIdiom.Desktop
                   || DeviceInfo.Current.Idiom == DeviceIdiom.Tablet
                   || DeviceInfo.Current.Idiom == DeviceIdiom.TV
            ? new AppShellDesktop()
            : new AppShell();

        if (settingsFacade.IsSetupCompleted is false)
        {
            Shell.Current.GoToAsync(Routes.WelcomeViewRoute).Wait();
        }

        FixCorruptPayments(settingsAdapter);
    }

    public static Dictionary<string, ResourceDictionary> ResourceDictionary { get; } = new();

    public static Action<IServiceCollection>? AddPlatformServicesAction { get; set; }

    private static IServiceProvider? ServiceProvider { get; set; }

    /// <summary>
    ///     This removes the link from payments to categories that no longer exists.
    ///     https://github.com/MoneyFox/MoneyFox/issues/2717
    ///     This can be removed in a future release.
    /// </summary>
    private static void FixCorruptPayments(ISettingsAdapter settingsAdapter)
    {
        try
        {
            if (settingsAdapter.GetValue(key: IS_CATEGORY_CLEANUP_EXECUTED_KEY_NAME, defaultValue: false) is false)
            {
                if (ServiceProvider?.GetService<IAppDbContext>() != null)
                {
                    var dbContext = ServiceProvider.GetService<IAppDbContext>();
                    var categoryIds = dbContext!.Categories.Select(c => c.Id).ToList();
                    var paymentsWithCategory = dbContext.Payments.Include(p => p.Category).Where(p => p.Category != null).ToList();
                    foreach (var payment in paymentsWithCategory.Where(payment => categoryIds.Contains(payment.Category!.Id) is false))
                    {
                        payment.RemoveCategory();
                        Analytics.TrackEvent(nameof(FixCorruptPayments));
                    }

                    dbContext.SaveChangesAsync().GetAwaiter().GetResult();
                    settingsAdapter.AddOrUpdate(key: IS_CATEGORY_CLEANUP_EXECUTED_KEY_NAME, value: true);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(exception: ex, messageTemplate: "Error while fixing payment with non existing category");
            Crashes.TrackError(ex);
        }
    }

    private void FillResourceDictionary()
    {
        foreach (var dictionary in Resources.MergedDictionaries)
        {
            var key = dictionary.Source.OriginalString.Split(';').First().Split('/').Last().Split('.').First();
            ResourceDictionary.Add(key: key, value: dictionary);
        }
    }

    internal static TViewModel GetViewModel<TViewModel>() where TViewModel : BasePageViewModel
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
        ServiceProvider.GetService<IAppDbContext>()?.MigrateDb();
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
        var messenger = ServiceProvider.GetService<IMessenger>() ?? throw new ResolveDependencyException<IMessenger>();
        try
        {
            if (settingsFacade.IsBackupAutoUploadEnabled && settingsFacade.IsLoggedInToBackupService)
            {
                var backupService = ServiceProvider.GetService<IBackupService>() ?? throw new ResolveDependencyException<IBackupService>();
                await backupService.RestoreBackupAsync();
                messenger.Send(new BackupRestoredMessage());
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
