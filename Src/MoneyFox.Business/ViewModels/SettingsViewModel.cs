using System.Threading.Tasks;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Models;
using MoneyFox.Foundation.Resources;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace MoneyFox.Business.ViewModels
{
    public interface ISettingsViewModel : IBaseViewModel
    {
        /// <summary>
        ///     Contains all available Settingsitems.
        /// </summary>
        MvxObservableCollection<SettingsSelectorType> SettingsList { get; }

        /// <summary>
        ///     Navigate to a concrete settings page.
        ///     Used in Xamarin Forms.
        /// </summary>
        MvxAsyncCommand<SettingsSelectorType> GoToSettingCommand { get; }

        /// <summary>
        ///     View Model for the Background job part.
        /// </summary>
        ISettingsBackgroundJobViewModel BackgroundJobViewModel { get; }

        ISettingsPersonalizationViewModel PersonalizationViewModel { get; }
        ISettingsSecurityViewModel SettingsSecurityViewModel { get; }
    }

    /// <summary>
    ///     ViewModel for the settings view.
    /// </summary>
    public class SettingsViewModel : BaseNavigationViewModel, ISettingsViewModel
    {
        private readonly IMvxNavigationService navigationService;

        public SettingsViewModel(IMvxNavigationService navigationService,
                                 IAboutViewModel aboutViewModel, 
                                 ISettingsBackgroundJobViewModel settingsBackgroundJobViewModel,
                                 ISettingsPersonalizationViewModel settingsPersonalizationViewModel,
                                 ISettingsSecurityViewModel settingsSecurityViewModel,
                                 IMvxLogProvider logProvider) : base(logProvider, navigationService)
        {
            this.navigationService = navigationService;

            AboutViewModel = aboutViewModel;
            BackgroundJobViewModel = settingsBackgroundJobViewModel;
            PersonalizationViewModel = settingsPersonalizationViewModel;
            SettingsSecurityViewModel = settingsSecurityViewModel;
        }

        public IAboutViewModel AboutViewModel { get; }

        /// <inheritdoc />
        public MvxObservableCollection<SettingsSelectorType> SettingsList => new MvxObservableCollection<SettingsSelectorType>
        {
            new SettingsSelectorType
            {
                Name = Strings.CategoriesLabel,
                Description = Strings.CategoriesSettingsDescription,
                Type = SettingsType.Categories
            },
            new SettingsSelectorType
            {
                Name = Strings.BackgroundJobLabel,
                Description = Strings.BackgroundJobSettingDescription,
                Type = SettingsType.BackgroundJob
            },
            new SettingsSelectorType
            {
                Name = Strings.BackupLabel,
                Description = Strings.BackupSettingsDescription,
                Type = SettingsType.Backup
            },
            new SettingsSelectorType
            {
                Name = Strings.AboutLabel,
                Description = Strings.AboutSettingsDescription,
                Type = SettingsType.About
            }
        };

        /// <inheritdoc />
        public MvxAsyncCommand<SettingsSelectorType> GoToSettingCommand => new MvxAsyncCommand<SettingsSelectorType>(GoToSettings);

        public ISettingsBackgroundJobViewModel BackgroundJobViewModel { get; set; }
        public ISettingsPersonalizationViewModel PersonalizationViewModel { get; set; }
        public ISettingsSecurityViewModel SettingsSecurityViewModel { get; set; }

        private async Task GoToSettings(SettingsSelectorType item)
        {
            switch (item.Type)
            {
                case SettingsType.Categories:
                    await navigationService.Navigate<CategoryListViewModel>();
                    break;

                case SettingsType.BackgroundJob:
                    await navigationService.Navigate<SettingsBackgroundJobViewModel>();
                    break;

                case SettingsType.Backup:
                    await navigationService.Navigate<BackupViewModel>();
                    break;

                case SettingsType.About:
                    await navigationService.Navigate<AboutViewModel>();
                    break;
            }
        }
    }
}