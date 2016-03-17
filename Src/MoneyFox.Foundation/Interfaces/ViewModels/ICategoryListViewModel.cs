using System.Collections.ObjectModel;
using MoneyFox.Foundation.Model;

namespace MoneyManager.Foundation.Interfaces.ViewModels
{
    public interface ICategoryListViewModel
    {
        ObservableCollection<Category> Categories { get; set; }
        Category SelectedCategory { get; set; }
        string SearchText { get; set; }
    }
}