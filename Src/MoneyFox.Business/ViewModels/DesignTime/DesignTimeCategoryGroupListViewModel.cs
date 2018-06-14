using System.Collections.ObjectModel;
using System.Globalization;
using MoneyFox.Business.Helpers;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Resources;
using MvvmCross.Commands;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeCategoryGroupListViewModel : ICategoryGroupListViewModel
    {
        public DesignTimeCategoryGroupListViewModel()
        {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }

        /// <inheritdoc />
        public LocalizedResources Resources { get; }
        public MvxAsyncCommand CreateNewGroupCommand { get; }
        public ObservableCollection<AlphaGroupListGroup<CategoryViewModel>> CategoryGroupList { get; set; }
        public bool IsGroupListEmpty { get; }
    }
}
