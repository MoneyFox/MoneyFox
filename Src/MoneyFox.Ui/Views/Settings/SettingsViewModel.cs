namespace MoneyFox.Ui.Views.Settings;

using System.Collections.ObjectModel;
using System.Globalization;
using Core.Common.Facades;
using Core.Common.Helpers;
using Core.Common.Interfaces;
using Serilog;

internal sealed class SettingsViewModel : BaseViewModel
{
    private readonly IDialogService dialogService;
    private readonly ISettingsFacade settingsFacade;

    private CultureInfo selectedCulture = CultureHelper.CurrentCulture;

    public SettingsViewModel(ISettingsFacade settingsFacade, IDialogService dialogService)
    {
        this.settingsFacade = settingsFacade;
        this.dialogService = dialogService;
        AvailableCultures = new();
    }

    public CultureInfo SelectedCulture
    {
        get => selectedCulture;

        set
        {
            if (value == null)
            {
                return;
            }

            selectedCulture = value;
            settingsFacade.DefaultCulture = selectedCulture.Name;
            CultureHelper.CurrentCulture = selectedCulture;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<CultureInfo> AvailableCultures { get; }

    public async Task InitializeAsync()
    {
        await LoadAvailableCulturesAsync();
    }

    private async Task LoadAvailableCulturesAsync()
    {
        try
        {
            await dialogService.ShowLoadingDialogAsync();
            CultureInfo.GetCultures(CultureTypes.AllCultures).OrderBy(x => x.Name).ToList().ForEach(AvailableCultures.Add);
            SelectedCulture = AvailableCultures.First(x => x.Name == settingsFacade.DefaultCulture);
        }
        catch (Exception ex)
        {
            Log.Error(exception: ex, messageTemplate: "Failed to load Available Cultures");
        }
        finally
        {
            await dialogService.HideLoadingDialogAsync();
        }
    }
}
