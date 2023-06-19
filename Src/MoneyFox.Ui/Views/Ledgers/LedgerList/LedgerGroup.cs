namespace MoneyFox.Ui.Views.Ledgers.LedgerList;

using MoneyFox.Core.Common.Settings;
using MoneyFox.Ui.Infrastructure.Adapters;
using MoneyFox.Ui.Resources.Strings;

public class LedgerGroup : List<LedgerListItemViewModel>
{
    private readonly bool isExcluded;
    private readonly ISettingsFacade settingsFacade;

    public LedgerGroup(bool isExcluded, List<LedgerListItemViewModel> accountItems) : base(accountItems)
    {
        this.isExcluded = isExcluded;
        settingsFacade = new SettingsFacade(new SettingsAdapter());
    }

    public string Title => isExcluded ? Translations.ExcludedAccountsHeader : Translations.IncludedAccountsHeader;

    public string TotalString => $"{this.Sum(a => a.CurrentBalance)} {settingsFacade.DefaultCurrency}";
}
