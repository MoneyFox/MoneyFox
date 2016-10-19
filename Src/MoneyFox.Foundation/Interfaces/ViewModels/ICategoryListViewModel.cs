using System.Collections.ObjectModel;
using MoneyFox.Foundation.DataModels;

namespace MoneyFox.Shared.Interfaces.ViewModels
{
    public interface ICategoryListViewModel
    {
        ObservableCollection<CategoryViewModel> Categories { get; set; }
        CategoryViewModel SelectedCategory { get; set; }
        string SearchText { get; set; }
    }
}