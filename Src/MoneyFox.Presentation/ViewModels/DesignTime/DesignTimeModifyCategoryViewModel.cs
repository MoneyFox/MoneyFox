using System.Globalization;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Commands;
using MoneyFox.ServiceLayer.Utilities;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeModifyCategoryViewModel : IModifyCategoryViewModel
    {
        public DesignTimeModifyCategoryViewModel()
        {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }

        public AsyncCommand SaveCommand { get; }
        public AsyncCommand CancelCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public CategoryViewModel SelectedCategory { get; }

        public bool IsEdit { get; }
        public LocalizedResources Resources { get; }
    }
}
