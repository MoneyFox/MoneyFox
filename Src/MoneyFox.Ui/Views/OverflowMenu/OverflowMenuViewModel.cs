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

internal sealed record OverflowItemViewModel(string IconGlyph, string Name, OverflowMenuItemType Type);

[UsedImplicitly]
internal sealed class OverflowMenuViewModel(INavigationService service) : BasePageViewModel
{
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
                await service.GoTo<BudgetListViewModel>();

                break;
            case OverflowMenuItemType.Categories:
                await service.GoTo<CategoryListViewModel>();

                break;
            case OverflowMenuItemType.Backup:
                await service.GoTo<BackupViewModel>();

                break;
            case OverflowMenuItemType.Settings:
                await service.GoTo<SettingsViewModel>();

                break;
            case OverflowMenuItemType.About:
                await service.GoTo<AboutViewModel>();

                break;
        }
    }
}
