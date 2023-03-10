namespace MoneyFox.Ui.Views.Accounts.AccountList
{
    using MoneyFox.Ui.Resources.Strings;

    public class AccountGroup : List<AccountListItemViewModel>
    {
        private readonly bool isExcluded;
        public AccountGroup(bool isExcluded, List<AccountListItemViewModel> accountItems) : base(accountItems)
        {
            this.isExcluded = isExcluded;
        }

        public string Title => isExcluded ? Translations.ExcludedAccountsHeader : Translations.IncludedAccountsHeader;
    }
}
