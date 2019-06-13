using MoneyFox.ServiceLayer.ViewModels.DesignTime;

namespace MoneyFox.Presentation
{
    public static class DesignTimeViewModelLocator
    {
        static DesignTimeAboutViewModel aboutVm;
        static DesignTimeAccountListViewModel accountListVm;

        public static DesignTimeAboutViewModel AboutVm => aboutVm ?? (aboutVm = new DesignTimeAboutViewModel());
        public static DesignTimeAccountListViewModel AccountListVm => accountListVm ?? (accountListVm = new DesignTimeAccountListViewModel());
    }
}
