namespace MoneyFox.Win;

using System;
using CommonServiceLocator;
using Core._Pending_.Common.Facades;
using Core.ApplicationCore.UseCases.BackupUpload;
using Core.ApplicationCore.UseCases.DbBackup;
using Core.Commands.Payments.ClearPayments;
using Core.Commands.Payments.CreateRecurringPayments;
using MediatR;
using Microsoft.UI.Windowing;
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
        RegisterOnClosing();
    }

    // This is a temporary fix until WinUI Dialogs are fixed
    public static Frame RootFrame { get; private set; }

    private void RegisterOnClosing()
    {

        // Retrieve the window handle (HWND) of the current (XAML) WinUI 3 window.
        var hWnd =
            WinRT.Interop.WindowNative.GetWindowHandle(this);

        // Retrieve the WindowId that corresponds to hWnd.
        Microsoft.UI.WindowId windowId =
            Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);

        // Lastly, retrieve the AppWindow for the current (XAML) WinUI 3 window.
        AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

        if (appWindow != null)
        {
            // You now have an AppWindow object, and you can call its methods to manipulate the window.
            // As an example, let's change the title text of the window.
            appWindow.Closing += OnClosing;
        }
    }

    private async void OnClosing(AppWindow sender, AppWindowClosingEventArgs args)
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
