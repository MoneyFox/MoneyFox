namespace MoneyFox.Ui.Views.OverflowMenu;

using System.Collections.Immutable;
using About;
using Backup;
using Budget;
using Categories;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using Resources.Strings;
using Settings;

public sealed record OverflowItemViewModel(string IconGlyph, string Name, OverflowMenuItemType Type);

[UsedImplicitly]
public sealed class OverflowMenuViewModel(INavigationService navigationService) : NavigableViewModel
{
    public AsyncRelayCommand<OverflowItemViewModel> GoToSelectedItemCommand => new(s => GoToSelectedItem(s.Type));

    public static IReadOnlyList<OverflowItemViewModel> OverflowEntries
        => ImmutableList.Create<OverflowItemViewModel>(
            new(IconGlyph: IconFont.PiggyBankOutline, Name: Translations.BudgetsTitle, Type: OverflowMenuItemType.Budgets),
            new(IconGlyph: IconFont.TagOutline, Name: Translations.CategoriesTitle, Type: OverflowMenuItemType.Categories),
            new(IconGlyph: IconFont.CloudUploadOutline, Name: Translations.BackupTitle, Type: OverflowMenuItemType.Backup),
            new(IconGlyph: IconFont.CogOutline, Name: Translations.SettingsTitle, Type: OverflowMenuItemType.Settings),
            new(IconGlyph: IconFont.InformationOutline, Name: Translations.AboutTitle, Type: OverflowMenuItemType.About));

    private Task GoToSelectedItem(OverflowMenuItemType menuType)
    {
        return menuType switch
        {
            OverflowMenuItemType.Budgets => navigationService.GoTo<BudgetListViewModel>(),
            OverflowMenuItemType.Categories => navigationService.GoTo<CategoryListViewModel>(),
            OverflowMenuItemType.Backup => navigationService.GoTo<BackupViewModel>(),
            OverflowMenuItemType.Settings => navigationService.GoTo<SettingsViewModel>(),
            OverflowMenuItemType.About => navigationService.GoTo<AboutViewModel>(),
            _ => Task.CompletedTask
        };
    }
}
