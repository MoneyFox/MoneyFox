using System.Globalization;
using MoneyFox.Business.Helpers;
using MoneyFox.Foundation.Resources;

namespace MoneyFox.Business.ViewModels
{
    public class DesignTimeCategorySettingsViewModel : ICategorySettingsViewModel
    {
        public DesignTimeCategorySettingsViewModel()
        {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }

        public LocalizedResources Resources { get; }
        public ICategoryListViewModel CategoryListViewModel { get; }
        public ICategoryGroupListViewModel CategoryGroupListViewModel { get; }
    }
}
