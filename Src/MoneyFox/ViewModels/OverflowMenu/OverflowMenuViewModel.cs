namespace MoneyFox.ViewModels.OverflowMenu
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using Core.Interfaces;
    using Core.Resources;
    using JetBrains.Annotations;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Views.About;
    using Views.Backup;
    using Views.Categories;
    using Views.Settings;

    [UsedImplicitly]
    public class OverflowMenuViewModel : ObservableObject
    {
        private readonly INavigationService navigationService;

        public OverflowMenuViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public AsyncRelayCommand<OverflowItem> GoToSelectedItemCommand
            => new AsyncRelayCommand<OverflowItem>(async s => await GoToSelectedItem(s.Type));

        public List<OverflowItem> OverflowEntries
            => new List<OverflowItem>
            {
                new OverflowItem { Name = Strings.CategoriesTitle, Type = OverflowMenuItemType.Categories },
                new OverflowItem { Name = Strings.BackupTitle, Type = OverflowMenuItemType.Backup },
                new OverflowItem { Name = Strings.SettingsTitle, Type = OverflowMenuItemType.Settings },
                new OverflowItem { Name = Strings.AboutTitle, Type = OverflowMenuItemType.About }
            };

        private async Task GoToSelectedItem(OverflowMenuItemType menuType)
        {
            switch(menuType)
            {
                case OverflowMenuItemType.Categories:
                    await navigationService.NavigateTo<CategoryListPage>();
                    break;
                case OverflowMenuItemType.Backup:
                    await navigationService.NavigateTo<BackupPage>();
                    break;
                case OverflowMenuItemType.Settings:
                    await navigationService.NavigateTo<SettingsPage>();
                    break;
                case OverflowMenuItemType.About:
                    await navigationService.NavigateTo<AboutPage>();
                    break;
            }
        }
    }

    public enum OverflowMenuItemType
    {
        Categories,
        Backup,
        Settings,
        About
    }

    public class OverflowItem
    {
        public string Name { get; set; } = "";
        public OverflowMenuItemType Type { get; set; }
    }
}