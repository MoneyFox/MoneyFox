using MoneyFox.Ui.ViewModels;

namespace MoneyFox.Ui.Views.OverflowMenu;

using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Resources;
using MoneyFox.Ui.Views.Backup;
using Views.About;
using Views.Budget;
using Views.Categories;
using Views.Settings;

[UsedImplicitly]
internal sealed class OverflowMenuViewModel : BaseViewModel
{
    private readonly INavigationService navigationService;

    public OverflowMenuViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }

    public AsyncRelayCommand<OverflowItemViewModel> GoToSelectedItemCommand => new(async s => await GoToSelectedItem(s.Type));

    public List<OverflowItemViewModel> OverflowEntries
        => new()
        {
            new() { IconGlyph = "savings", Name = Strings.BudgetsTitle, Type = OverflowMenuItemType.Budgets },
            new() { IconGlyph = "label", Name = Strings.CategoriesTitle, Type = OverflowMenuItemType.Categories },
            new() { IconGlyph = "backup", Name = Strings.BackupTitle, Type = OverflowMenuItemType.Backup },
            new() { IconGlyph = "settings", Name = Strings.SettingsTitle, Type = OverflowMenuItemType.Settings },
            new() { IconGlyph = "info", Name = Strings.AboutTitle, Type = OverflowMenuItemType.About }
        };

    private async Task GoToSelectedItem(OverflowMenuItemType menuType)
    {
        switch (menuType)
        {
            case OverflowMenuItemType.Budgets:
                await navigationService.NavigateToAsync<BudgetListPage>();

                break;
            case OverflowMenuItemType.Categories:
                await navigationService.NavigateToAsync<CategoryListPage>();

                break;
            case OverflowMenuItemType.Backup:
                await navigationService.NavigateToAsync<BackupPage>();

                break;
            case OverflowMenuItemType.Settings:
                await navigationService.NavigateToAsync<SettingsPage>();

                break;
            case OverflowMenuItemType.About:
                await navigationService.NavigateToAsync<AboutPage>();

                break;
        }
    }
}
