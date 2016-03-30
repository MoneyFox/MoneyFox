using System.Collections.ObjectModel;
using MoneyFox.Core.Model;

namespace MoneyFox.Core.Interfaces.ViewModels
{
    public interface ICategoryListViewModel
    {
        ObservableCollection<Category> Categories { get; set; }
        Category SelectedCategory { get; set; }
        string SearchText { get; set; }
    }
}