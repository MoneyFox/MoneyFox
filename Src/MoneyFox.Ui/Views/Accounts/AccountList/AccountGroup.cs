namespace MoneyFox.Ui.Views.Accounts.AccountList;

using MoneyFox.Core.Common.Settings;
using MoneyFox.Ui.Infrastructure.Adapters;
using Resources.Strings;

public class AccountGroup : List<AccountListItemViewModel>
{
    private ISettingsFacade settingsFacade;
    private readonly bool isExcluded;

    public AccountGroup(bool isExcluded, List<AccountListItemViewModel> accountItems) : base(accountItems)
    {
        this.isExcluded = isExcluded;

        settingsFacade = new SettingsFacade(new SettingsAdapter());
    }

    public string Title => isExcluded ? Translations.ExcludedAccountsHeader : Translations.IncludedAccountsHeader;

    public string TotalString => $"{this.Sum(a => a.CurrentBalance)} {settingsFacade.DefaultCurrency}";
}
