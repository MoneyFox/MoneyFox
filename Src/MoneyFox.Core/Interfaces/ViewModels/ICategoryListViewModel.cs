using System.Collections.ObjectModel;
using MoneyFox.Core.DatabaseModels;

namespace MoneyFox.Core.Interfaces.ViewModels
{
    public interface ICategoryListViewModel
    {
        ObservableCollection<Category> Categories { get; set; }
        Category SelectedCategory { get; set; }
        string SearchText { get; set; }
    }
}