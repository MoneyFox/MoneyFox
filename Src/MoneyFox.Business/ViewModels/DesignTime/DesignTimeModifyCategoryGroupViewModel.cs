using System.Globalization;
using MoneyFox.Business.Helpers;
using MoneyFox.Foundation.Resources;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeModifyCategoryGroupViewModel : IModifyCategoryGroupViewModel
    {
        public DesignTimeModifyCategoryGroupViewModel()
        {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }

        public LocalizedResources Resources { get; }

        public bool IsEdit { get; set; }
        public string Title { get; set; } = "Edit Group";
    }
}
