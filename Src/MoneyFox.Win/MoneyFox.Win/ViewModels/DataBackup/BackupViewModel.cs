namespace MoneyFox.Win.ViewModels.DataBackup;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core._Pending_.Common.Facades;
using Core._Pending_.Common.Interfaces;
using Core._Pending_.DbBackup;
using Core._Pending_.Exceptions;
using Core.Interfaces;
using Core.Resources;
using Microsoft.AppCenter.Crashes;
using NLog;
using System;
using System.Threading.Tasks;

public class BackupViewModel : ObservableObject, IBackupViewModel
{
    private readonly Logger logger = LogManager.GetCurrentClassLogger();

    private readonly IBackupService backupService;
    private readonly IConnectivityAdapter connectivity;
    private readonly IDialogService dialogService;
    private readonly ISettingsFacade settingsFacade;
    private readonly IToastService toastService;
    private bool backupAvailable;
    private UserAccount userAccount;

    private DateTime backupLastModified;
    private bool isLoadingBackupAvailability;

    public BackupViewModel(
        IBackupService backupService,
        IDialogService dialogService,
        IConnectivityAdapter connectivity,
        ISettingsFacade settingsFacade,
        IToastService toastService)
    {
        this.backupService = backupService;
        this.dialogService = dialogService;
        this.connectivity = connectivity;
        this.settingsFacade = settingsFacade;
        this.toastService = toastService;

        userAccount = new UserAccount();
    }

    /// <inheritdoc />
    public AsyncRelayCommand InitializeCommand => new(async () => await InitializeAsync());

    /// <inheritdoc />
    public AsyncRelayCommand LoginCommand => new(async () => await LoginAsync());

    /// <inheritdoc />
    public AsyncRelayCommand LogoutCommand => new(async () => await LogoutAsync());

    /// <inheritdoc />
    public AsyncRelayCommand BackupCommand => new(async () => await CreateBackupAsync());

    /// <inheritdoc />
    public AsyncRelayCommand RestoreCommand => new(async () => await RestoreBackupAsync());

    /// <summary>
    ///     The Date when the backup was modified the last time.
    /// </summary>
    public DateTime BackupLastModified
    {
        get => backupLastModified;
        private set
        {
            if(backupLastModified == value)
            {
                return;
            }

            backupLastModified = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Indicator that the app is checking if backups available.
    /// </summary>
    public bool IsLoadingBackupAvailability
    {
        get => isLoadingBackupAvailability;
        private set
        {
            if(isLoadingBackupAvailability == value)
            {
                return;
            }

            isLoadingBackupAvailability = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Indicator that the user logged in to the backup service.
    /// </summary>
    public bool IsLoggedIn => settingsFacade.IsLoggedInToBackupService;

    /// <summary>
    ///     Indicates if a backup is available for restore.
    /// </summary>
    public bool BackupAvailable
    {
        get => backupAvailable;
        private set
        {
            if(backupAvailable == value)
            {
                return;
            }

            backupAvailable = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc />
    public bool IsAutoBackupEnabled
    {
        get => settingsFacade.IsBackupAutouploadEnabled;
        set
        {
            if(settingsFacade.IsBackupAutouploadEnabled == value)
            {
                return;
            }

            settingsFacade.IsBackupAutouploadEnabled = value;
            OnPropertyChanged();
        }
    }

    public UserAccount UserAccount
    {
        get => userAccount;
        set
        {
            if(userAccount == value)
            {
                return;
            }

            userAccount = value;
            OnPropertyChanged();
        }
    }

    private async Task InitializeAsync() => await LoadedAsync();

    private async Task LoadedAsync()
    {
        if(!IsLoggedIn)
        {
            OnPropertyChanged(nameof(IsLoggedIn));
            return;
        }

        if(!connectivity.IsConnected)
        {
            await dialogService.ShowMessageAsync(Strings.NoNetworkTitle, Strings.NoNetworkMessage);
        }

        IsLoadingBackupAvailability = true;
        try
        {
            BackupAvailable = await backupService.IsBackupExistingAsync();
            BackupLastModified = await backupService.GetBackupDateAsync();
            if(backupService.UserAccount != null)
            {
                UserAccount = backupService.UserAccount.GetUserAccount();
            }
        }
        catch(BackupAuthenticationFailedException ex)
        {
            logger.Error(ex, "Issue during Login process.");
            await backupService.LogoutAsync();
            await dialogService.ShowMessageAsync(
                Strings.AuthenticationFailedTitle,
                Strings.ErrorMessageAuthenticationFailed);
        }
        catch(Exception ex)
        {
            if(ex.StackTrace == "4f37.717b")
            {
                await backupService.LogoutAsync();
                await dialogService.ShowMessageAsync(
                    Strings.AuthenticationFailedTitle,
                    Strings.ErrorMessageAuthenticationFailed);
            }

            logger.Error(ex, "Issue on loading backup view.");
        }

        IsLoadingBackupAvailability = false;
    }

    private async Task LoginAsync()
    {
        if(!connectivity.IsConnected)
        {
            logger.Info("Tried to log in, but device isn't connected to the internet.");
            await dialogService.ShowMessageAsync(Strings.NoNetworkTitle, Strings.NoNetworkMessage);
        }

        try
        {
            await backupService.LoginAsync();
            UserAccount = backupService.UserAccount.GetUserAccount();
        }
        catch(BackupOperationCanceledException)
        {
            await dialogService.ShowMessageAsync(Strings.CanceledTitle, Strings.LoginCanceledMessage);
        }
        catch(Exception ex)
        {
            logger.Error(ex, "Login Failed.");
            await dialogService.ShowMessageAsync(
                Strings.LoginFailedTitle,
                string.Format(Strings.UnknownErrorMessage, ex.Message));
            Crashes.TrackError(ex);
        }

        OnPropertyChanged(nameof(IsLoggedIn));
        await LoadedAsync();
    }

    private async Task LogoutAsync()
    {
        try
        {
            await backupService.LogoutAsync();
        }
        catch(BackupOperationCanceledException)
        {
            await dialogService.ShowMessageAsync(Strings.CanceledTitle, Strings.LogoutCanceledMessage);
        }
        catch(Exception ex)
        {
            logger.Error(ex, "Logout Failed.");
            await dialogService.ShowMessageAsync(Strings.GeneralErrorTitle, ex.Message);
            Crashes.TrackError(ex);
        }

        // ReSharper disable once ExplicitCallerInfoArgument
        OnPropertyChanged(nameof(IsLoggedIn));
    }

    private async Task CreateBackupAsync()
    {
        if(!await ShowOverwriteBackupInfoAsync())
        {
            return;
        }

        await dialogService.ShowLoadingDialogAsync();

        try
        {
            await backupService.UploadBackupAsync(BackupMode.Manual);
            await toastService.ShowToastAsync(Strings.BackupCreatedMessage);

            BackupLastModified = DateTime.Now;
        }
        catch(BackupOperationCanceledException)
        {
            await dialogService.ShowMessageAsync(Strings.CanceledTitle, Strings.UploadBackupCanceledMessage);
        }
        catch(Exception ex)
        {
            logger.Error(ex, "Create Backup failed.");
            await dialogService.ShowMessageAsync(Strings.BackupFailedTitle, ex.Message);
            Crashes.TrackError(ex);
        }

        await dialogService.HideLoadingDialogAsync();
    }

    private async Task RestoreBackupAsync()
    {
        if(!await ShowOverwriteDataInfoAsync())
        {
            return;
        }

        DateTime backupDate = await backupService.GetBackupDateAsync();
        if(settingsFacade.LastDatabaseUpdate <= backupDate || await ShowForceOverrideConfirmationAsync())
        {
            await dialogService.ShowLoadingDialogAsync();

            try
            {
                await backupService.RestoreBackupAsync(BackupMode.Manual);
                await toastService.ShowToastAsync(Strings.BackupRestoredMessage);
            }
            catch(BackupOperationCanceledException)
            {
                logger.Info("Restoring the backup was canceled by the user.");
                await dialogService.ShowMessageAsync(Strings.CanceledTitle, Strings.RestoreBackupCanceledMessage);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Restore Backup failed.");
                await dialogService.ShowMessageAsync(Strings.BackupFailedTitle, ex.Message);
                Crashes.TrackError(ex);
            }
        }
        else
        {
            logger.Info("Restore Backup canceled by the user due to newer local data.");
        }

        await dialogService.HideLoadingDialogAsync();
    }

    private async Task<bool> ShowOverwriteBackupInfoAsync()
        => await dialogService.ShowConfirmMessageAsync(
            Strings.OverwriteTitle,
            Strings.OverwriteBackupMessage,
            Strings.YesLabel,
            Strings.NoLabel);

    private async Task<bool> ShowOverwriteDataInfoAsync()
        => await dialogService.ShowConfirmMessageAsync(
            Strings.OverwriteTitle,
            Strings.OverwriteDataMessage,
            Strings.YesLabel,
            Strings.NoLabel);

    private async Task<bool> ShowForceOverrideConfirmationAsync()
        => await dialogService.ShowConfirmMessageAsync(
            Strings.ForceOverrideBackupTitle,
            Strings.ForceOverrideBackupMessage,
            Strings.YesLabel,
            Strings.NoLabel);
}