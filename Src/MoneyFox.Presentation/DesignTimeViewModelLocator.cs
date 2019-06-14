using MoneyFox.Presentation.ViewModels;
using MoneyFox.Presentation.ViewModels.DesignTime;
using MoneyFox.ServiceLayer.ViewModels.DesignTime;

namespace MoneyFox.Presentation
{
    public static class DesignTimeViewModelLocator
    {
        static MainViewModel MAIN_VM;
        static DesignTimeAboutViewModel ABOUT_VM;
        static DesignTimeAccountListViewModel ACCOUNT_LIST_VM;

        public static MainViewModel MainVm => MAIN_VM ?? (MAIN_VM = new MainViewModel(null));
        public static DesignTimeAboutViewModel AboutVm => ABOUT_VM ?? (ABOUT_VM = new DesignTimeAboutViewModel());
        public static DesignTimeAccountListViewModel AccountListVm => ACCOUNT_LIST_VM ?? (ACCOUNT_LIST_VM = new DesignTimeAccountListViewModel());
    }
}
