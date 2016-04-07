using System.Collections.ObjectModel;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Interfaces.ViewModels
{
    public interface ICategoryListViewModel
    {
        ObservableCollection<Category> Categories { get; set; }
        Category SelectedCategory { get; set; }
        string SearchText { get; set; }
    }
}