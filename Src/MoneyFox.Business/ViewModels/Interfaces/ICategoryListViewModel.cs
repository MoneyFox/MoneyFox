using System.Collections.ObjectModel;

namespace MoneyFox.Business.ViewModels.Interfaces
{
    public interface ICategoryListViewModel
    {
        ObservableCollection<CategoryViewModel> Categories { get; set; }
        CategoryViewModel SelectedCategory { get; set; }
        string SearchText { get; set; }
    }
}