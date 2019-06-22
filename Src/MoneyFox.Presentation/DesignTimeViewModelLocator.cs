using MoneyFox.Presentation.ViewModels;
using MoneyFox.Presentation.ViewModels.DesignTime;

namespace MoneyFox.Presentation
{
    public static class DesignTimeViewModelLocator
    {
        static ShellViewModel SHELL_VM;
        static DesignTimeAboutViewModel ABOUT_VM;
        static DesignTimeAccountListViewModel ACCOUNT_LIST_VM;

        public static ShellViewModel ShellVm => SHELL_VM ?? (SHELL_VM = new ShellViewModel(null));
        public static DesignTimeAboutViewModel AboutVm => ABOUT_VM ?? (ABOUT_VM = new DesignTimeAboutViewModel());
        public static DesignTimeAccountListViewModel AccountListVm => ACCOUNT_LIST_VM ?? (ACCOUNT_LIST_VM = new DesignTimeAccountListViewModel());
    }
}
