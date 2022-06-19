namespace MoneyFox.Win.ViewModels.Settings;

using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Common;
using Core.Common.Facades;
using Core.Common.Helpers;
using Core.Common.Interfaces;

internal class SettingsViewModel : BaseViewModel, ISettingsViewModel
{
    private readonly ISettingsFacade settingsFacade;
    private readonly IDialogService dialogService;

    private CultureInfo selectedCulture = CultureHelper.CurrentCulture;

    public SettingsViewModel(ISettingsFacade settingsFacade, IDialogService dialogService)
    {
        this.settingsFacade = settingsFacade;
        this.dialogService = dialogService;
        AvailableCultures = new();
    }

    public async Task InitializeAsync()
    {
        await LoadAvailableCulturesAsync();
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

    private async Task LoadAvailableCulturesAsync()
    {
        await dialogService.ShowLoadingDialogAsync();
        CultureInfo.GetCultures(CultureTypes.AllCultures).OrderBy(x => x.Name).ToList().ForEach(AvailableCultures.Add);
        SelectedCulture = AvailableCultures.First(x => x.Name == settingsFacade.DefaultCulture);
        await dialogService.HideLoadingDialogAsync();
    }
}
