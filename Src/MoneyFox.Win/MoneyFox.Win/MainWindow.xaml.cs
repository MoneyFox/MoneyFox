namespace MoneyFox.Win;

using System;
using CommonServiceLocator;
using Core._Pending_.Common.Facades;
using Core.ApplicationCore.UseCases.BackupUpload;
using Core.ApplicationCore.UseCases.DbBackup;
using Core.Commands.Payments.ClearPayments;
using Core.Commands.Payments.CreateRecurringPayments;
using MediatR;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Pages;
using Serilog;

public sealed partial class MainWindow : Window
{
    private bool isRunning;

    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
        RootFrame = ShellFrame;
        RootFrame.Navigate(sourcePageType: typeof(ShellPage), parameter: null);
        Closed += OnClosed;
    }

    // This is a temporary fix until WinUI Dialogs are fixed
    public static Frame RootFrame { get; private set; }

    private async void OnClosed(object sender, WindowEventArgs args)
    {
        // Don't execute this again when already running
        if (isRunning)
        {
            return;
        }

        isRunning = true;
        var settingsFacade = ServiceLocator.Current.GetInstance<ISettingsFacade>();
        var mediator = ServiceLocator.Current.GetInstance<IMediator>();
        try
        {
            if (settingsFacade.IsBackupAutoUploadEnabled && settingsFacade.IsLoggedInToBackupService)
            {
                var backupService = ServiceLocator.Current.GetInstance<IBackupService>();
                await backupService.RestoreBackupAsync();
            }

            await mediator.Send(new ClearPaymentsCommand());
            await mediator.Send(new CreateRecurringPaymentsCommand());
            await mediator.Send(new UploadBackup.Command());
        }
        catch (Exception ex)
        {
            Log.Fatal(exception: ex, messageTemplate: "Error during startup");
        }
        finally
        {
            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            isRunning = false;
        }
    }
}
