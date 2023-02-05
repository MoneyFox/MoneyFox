namespace MoneyFox.Ui.Views.OverflowMenu;

using System.Collections.Immutable;
using About;
using Backup;
using Budget;
using Categories;
using CommunityToolkit.Mvvm.Input;
using Core.Interfaces;
using JetBrains.Annotations;
using Resources.Strings;
using Settings;

internal sealed record OverflowItemViewModel(string IconGlyph, string Name, OverflowMenuItemType Type);

[UsedImplicitly]
internal sealed class OverflowMenuViewModel : BasePageViewModel
{
    private readonly INavigationService navigationService;

    public OverflowMenuViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }

    public AsyncRelayCommand<OverflowItemViewModel> GoToSelectedItemCommand => new(async s => await GoToSelectedItem(s.Type));

    public static IReadOnlyList<OverflowItemViewModel> OverflowEntries
        => ImmutableList.Create<OverflowItemViewModel>(
            new(IconGlyph: IconFont.PiggyBankOutline, Name: Translations.BudgetsTitle, Type: OverflowMenuItemType.Budgets),
            new(IconGlyph: IconFont.TagOutline, Name: Translations.CategoriesTitle, Type: OverflowMenuItemType.Categories),
            new(IconGlyph: IconFont.CloudUploadOutline, Name: Translations.BackupTitle, Type: OverflowMenuItemType.Backup),
            new(IconGlyph: IconFont.CogOutline, Name: Translations.SettingsTitle, Type: OverflowMenuItemType.Settings),
            new(IconGlyph: IconFont.InformationOutline, Name: Translations.AboutTitle, Type: OverflowMenuItemType.About));

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
