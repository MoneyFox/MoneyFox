namespace MoneyFox.Ui.Views.Accounts.AccountList;

using Core.Common.Settings;
using Infrastructure.Adapters;
using Resources.Strings;

public class AccountGroup : List<AccountListItemViewModel>
{
    private readonly bool isExcluded;
    private readonly ISettingsFacade settingsFacade;

    public AccountGroup(bool isExcluded, List<AccountListItemViewModel> accountItems) : base(accountItems)
    {
        this.isExcluded = isExcluded;
        settingsFacade = new SettingsFacade(new SettingsAdapter());
    }

    public string Title => isExcluded ? Translations.ExcludedAccountsHeader : Translations.IncludedAccountsHeader;

    public string TotalString => $"{this.Sum(a => a.CurrentBalance)} {settingsFacade.DefaultCurrency}";
}
