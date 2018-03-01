using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation.Groups;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeCategoryListViewModel : ICategoryListViewModel
    {
        public ObservableCollection<AlphaGroupListGroup<CategoryViewModel>> CategoryList =>
            new ObservableCollection<AlphaGroupListGroup<CategoryViewModel>>
            {
                new AlphaGroupListGroup<CategoryViewModel>("A")
                {
                    new CategoryViewModel(new Category()) {Name = "Auto"}
                },
                new AlphaGroupListGroup<CategoryViewModel>("E")
                {
                    new CategoryViewModel(new Category()) {Name = "Einkaufen"}
                }
            };
    }
}
