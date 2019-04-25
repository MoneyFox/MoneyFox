using System.Globalization;
using System.Reactive;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;
using MvvmCross.Commands;
using ReactiveUI;

namespace MoneyFox.ServiceLayer.ViewModels.DesignTime
{
    public class DesignTimeModifyCategoryViewModel : IModifyCategoryViewModel
    {
        public DesignTimeModifyCategoryViewModel()
        {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }
        public CategoryViewModel SelectedCategory { get; }

        public bool IsEdit { get; }
        public LocalizedResources Resources { get; }
    }
}
