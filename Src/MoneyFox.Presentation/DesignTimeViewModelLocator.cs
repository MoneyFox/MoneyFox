using MoneyFox.Presentation.ViewModels;
using MoneyFox.ServiceLayer.ViewModels.DesignTime;

namespace MoneyFox.Presentation
{
    public static class DesignTimeViewModelLocator
    {
        static MainViewModel mainVm;
        static DesignTimeAboutViewModel aboutVm;
        static DesignTimeAccountListViewModel accountListVm;

        public static MainViewModel MainVm => mainVm ?? (mainVm = new MainViewModel(null));
        public static DesignTimeAboutViewModel AboutVm => aboutVm ?? (aboutVm = new DesignTimeAboutViewModel());
        public static DesignTimeAccountListViewModel AccountListVm => accountListVm ?? (accountListVm = new DesignTimeAccountListViewModel());
    }
}
