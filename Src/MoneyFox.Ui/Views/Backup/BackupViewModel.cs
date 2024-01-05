namespace MoneyFox.Ui.Views.Backup;

using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Interfaces;
using Core.Common.Settings;
using Core.Features.BackupUpload;
using Core.Features.DbBackup;
using Core.Interfaces;
using Domain.Exceptions;
using MediatR;
using Resources.Strings;
using Serilog;

internal sealed class BackupViewModel(
    IMediator mediator,
    IBackupService backupService,
    IDialogService dialogService,
    IConnectivityAdapter connectivity,
    ISettingsFacade settingsFacade,
    IToastService toastService,
    IOneDriveProfileService oneDriveProfileService) : NavigableViewModel
{
    private bool backupAvailable;

    private DateTime backupLastModified;
    private bool isLoading;

    private ImageSource? profilePicture;
    private UserAccountViewModel userAccount = new();
    public UserAccountViewModel UserAccount
    {
        get => userAccount;
        set => SetProperty(field: ref userAccount, newValue: value);
    }

    public ImageSource? ProfilePicture
    {
        get => profilePicture;
        set => SetProperty(field: ref profilePicture, newValue: value);
    }

    public AsyncRelayCommand LoginCommand => new(LoginAsync);

    public AsyncRelayCommand LogoutCommand => new(LogoutAsync);

    public AsyncRelayCommand BackupCommand => new(CreateBackupAsync);

    public AsyncRelayCommand RestoreCommand => new(RestoreBackupAsync);

    public DateTime BackupLastModified
    {
        get => backupLastModified;
        set => SetProperty(field: ref backupLastModified, newValue: value);
    }

    public bool IsLoading
    {
        get => isLoading;
        set => SetProperty(field: ref isLoading, newValue: value);
    }

    public bool IsLoggedIn => settingsFacade.IsLoggedInToBackupService;
    
    public bool BackupAvailable
    {
        get => backupAvailable;
        set => SetProperty(field: ref backupAvailable, newValue: value);
    }

    public bool IsAutoBackupEnabled
    {
        get => settingsFacade.IsBackupAutoUploadEnabled;

        set
        {
            if (settingsFacade.IsBackupAutoUploadEnabled == value)
            {
                return;
            }

            settingsFacade.IsBackupAutoUploadEnabled = value;
            OnPropertyChanged();
        }
    }

    public override Task OnNavigatedAsync(object? parameter)
    {
        return LoadedAsync();
    }

    private async Task LoadedAsync()
    {
        if (!IsLoggedIn)
        {
            OnPropertyChanged(nameof(IsLoggedIn));

            return;
        }

        if (!connectivity.IsConnected)
        {
            await toastService.ShowToastAsync(Translations.NoNetworkMessage);

            return;
        }

        IsLoading = true;
        try
        {
            BackupAvailable = await backupService.IsBackupExistingAsync();
            BackupLastModified = await backupService.GetBackupDateAsync();
            var userAccountDto = await oneDriveProfileService.GetUserAccountAsync();
            UserAccount.Name = userAccountDto.Name;
            UserAccount.Email = userAccountDto.Email;
            var profilePictureStream = await oneDriveProfileService.GetProfilePictureAsync();
            ProfilePicture = ImageSource.FromStream(() => profilePictureStream);
        }
        catch (BackupAuthenticationFailedException ex)
        {
            Log.Error(exception: ex, messageTemplate: "Issue during Login process");
            await LogoutAsync();
            await toastService.ShowToastAsync(Translations.ErrorMessageAuthenticationFailed);
        }
        catch (NetworkConnectionException)
        {
            await toastService.ShowToastAsync(Translations.NoNetworkMessage);
        }
        catch (Exception ex)
        {
            if (ex.StackTrace == "4f37.717b")
            {
                await LogoutAsync();
                await toastService.ShowToastAsync(Translations.ErrorMessageAuthenticationFailed);
            }

            Log.Error(exception: ex, messageTemplate: "Issue on loading backup view");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LoginAsync()
    {
        if (!connectivity.IsConnected)
        {
            await toastService.ShowToastAsync(Translations.NoNetworkMessage);
        }

        try
        {
            IsLoading = true;
            await backupService.LoginAsync();
            var userAccountDto = await oneDriveProfileService.GetUserAccountAsync();
            UserAccount.Name = userAccountDto.Name;
            UserAccount.Email = userAccountDto.Email;
        }
        catch (BackupOperationCanceledException)
        {
            await toastService.ShowToastAsync(Translations.LoginCanceledMessage);
        }
        catch (NetworkConnectionException)
        {
            await toastService.ShowToastAsync(Translations.NoNetworkMessage);
        }
        catch (Exception ex)
        {
            Log.Error(exception: ex, messageTemplate: "Login Failed");
            await toastService.ShowToastAsync(Translations.LoginFailedTitle);
        }
        finally
        {
            IsLoading = false;
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
        catch (BackupOperationCanceledException)
        {
            await toastService.ShowToastAsync(Translations.LogoutCanceledMessage);
        }
        catch (NetworkConnectionException)
        {
            await toastService.ShowToastAsync(Translations.NoNetworkMessage);
        }
        catch (Exception ex)
        {
            Log.Error(exception: ex, messageTemplate: "Logout Failed");
            await toastService.ShowToastAsync(Translations.GeneralErrorTitle);
        }

        // ReSharper disable once ExplicitCallerInfoArgument
        OnPropertyChanged(nameof(IsLoggedIn));
    }

    private async Task CreateBackupAsync()
    {
        if (!await ShowOverwriteBackupInfoAsync())
        {
            return;
        }

        await dialogService.ShowLoadingDialogAsync();
        try
        {
            var result = await mediator.Send(new UploadBackup.Command());
            await ShowUploadResult(result);
        }
        catch (BackupOperationCanceledException)
        {
            await toastService.ShowToastAsync(Translations.UploadBackupCanceledMessage);
        }
        catch (NetworkConnectionException)
        {
            await toastService.ShowToastAsync(Translations.NoNetworkMessage);
        }
        catch (Exception ex)
        {
            Log.Error(exception: ex, messageTemplate: "Create Backup failed");
            await toastService.ShowToastAsync(Translations.BackupFailedTitle);
        }

        await dialogService.HideLoadingDialogAsync();
    }

    private async Task ShowUploadResult(UploadBackup.UploadResult uploadResult)
    {
        switch (uploadResult)
        {
            case UploadBackup.UploadResult.Successful:
                await toastService.ShowToastAsync(Translations.BackupCreatedMessage);
                BackupLastModified = DateTime.Now;

                break;
            case UploadBackup.UploadResult.Skipped:
                await toastService.ShowToastAsync(Translations.BackupUploadSkippedMessage);

                break;
        }
    }

    private async Task RestoreBackupAsync()
    {
        if (!await ShowOverwriteDataInfoAsync())
        {
            return;
        }

        var backupDate = await backupService.GetBackupDateAsync();
        if (settingsFacade.LastDatabaseUpdate <= backupDate || await ShowForceOverrideConfirmationAsync())
        {
            await dialogService.ShowLoadingDialogAsync();
            try
            {
                await backupService.RestoreBackupAsync(BackupMode.Manual);
                await toastService.ShowToastAsync(Translations.BackupRestoredMessage);
            }
            catch (BackupOperationCanceledException)
            {
                await toastService.ShowToastAsync(Translations.RestoreBackupCanceledMessage);
            }
            catch (Exception ex)
            {
                Log.Error(exception: ex, messageTemplate: "Restore Backup failed");
                await toastService.ShowToastAsync(Translations.BackupFailedTitle);
            }
            finally
            {
                await dialogService.HideLoadingDialogAsync();
            }
        }
        else
        {
            Log.Information("Restore Backup canceled by the user due to newer local data");
        }
    }

    private async Task<bool> ShowOverwriteBackupInfoAsync()
    {
        return await dialogService.ShowConfirmMessageAsync(
            title: Translations.OverwriteTitle,
            message: Translations.OverwriteBackupMessage,
            positiveButtonText: Translations.YesLabel,
            negativeButtonText: Translations.NoLabel);
    }

    private async Task<bool> ShowOverwriteDataInfoAsync()
    {
        return await dialogService.ShowConfirmMessageAsync(
            title: Translations.OverwriteTitle,
            message: Translations.OverwriteDataMessage,
            positiveButtonText: Translations.YesLabel,
            negativeButtonText: Translations.NoLabel);
    }

    private async Task<bool> ShowForceOverrideConfirmationAsync()
    {
        return await dialogService.ShowConfirmMessageAsync(
            title: Translations.ForceOverrideBackupTitle,
            message: Translations.ForceOverrideBackupMessage,
            positiveButtonText: Translations.YesLabel,
            negativeButtonText: Translations.NoLabel);
    }
}
