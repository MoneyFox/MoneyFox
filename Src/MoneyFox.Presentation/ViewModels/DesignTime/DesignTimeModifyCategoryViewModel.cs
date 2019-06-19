using System.Globalization;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;
using MvvmCross.Commands;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeModifyCategoryViewModel : IModifyCategoryViewModel
    {
        public DesignTimeModifyCategoryViewModel()
        {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }

        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }
        public MvxAsyncCommand DeleteCommand { get; }
        public CategoryViewModel SelectedCategory { get; }

        public bool IsEdit { get; }
        public LocalizedResources Resources { get; }
    }
}
