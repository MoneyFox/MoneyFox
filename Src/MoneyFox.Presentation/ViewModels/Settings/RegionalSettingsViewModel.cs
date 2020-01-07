using GalaSoft.MvvmLight;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Presentation.Commands;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyFox.Presentation.ViewModels.Settings
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
                selectedCulture = value;
                settingsFacade.DefaultCulture = selectedCulture.Name;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<CultureInfo> AvailableCultures { get; }

        public AsyncCommand LoadAvailableCulturesCommand => new AsyncCommand(LoadAvailableCulturesAsync);

        private async Task LoadAvailableCulturesAsync()
        {
            await dialogService.ShowLoadingDialogAsync();

            CultureInfo.GetCultures(CultureTypes.AllCultures).ToList().ForEach(AvailableCultures.Add);
            SelectedCulture = AvailableCultures.First(x => x.Name == settingsFacade.DefaultCulture);

            await dialogService.HideLoadingDialogAsync();
        }
    }
}
