using GalaSoft.MvvmLight;
using MoneyFox.Application;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Ui.Shared.Commands;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.ViewModels.Settings
{
    public class RegionalSettingsViewModel : ViewModelBase, IRegionalSettingsViewModel
    {
        private readonly ISettingsFacade settingsFacade;
        private readonly IDialogService dialogService;

        public RegionalSettingsViewModel(ISettingsFacade settingsFacade,
                                         IDialogService dialogService)
        {
            this.settingsFacade = settingsFacade;
            this.dialogService = dialogService;

            AvailableCultures = new ObservableCollection<CultureInfo>();
        }

        private CultureInfo selectedCulture;

        public CultureInfo SelectedCulture
        {
            get => selectedCulture;
            set
            {
                if(selectedCulture == value)
                    return;
                selectedCulture = value;
                settingsFacade.DefaultCulture = selectedCulture.Name;
                CultureHelper.CurrentCulture = selectedCulture;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<CultureInfo> AvailableCultures { get; }

        public AsyncCommand LoadAvailableCulturesCommand => new AsyncCommand(LoadAvailableCulturesAsync);

        private async Task LoadAvailableCulturesAsync()
        {
            await dialogService.ShowLoadingDialogAsync();

            CultureInfo.GetCultures(CultureTypes.AllCultures).OrderBy(x => x.Name).ToList().ForEach(AvailableCultures.Add);
            SelectedCulture = AvailableCultures.First(x => x.Name == settingsFacade.DefaultCulture);

            await dialogService.HideLoadingDialogAsync();
        }
    }
}
