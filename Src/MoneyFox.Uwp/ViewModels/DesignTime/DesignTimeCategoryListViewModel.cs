﻿using CommunityToolkit.Mvvm.Input;
using MoneyFox.Uwp.Groups;
using MoneyFox.Uwp.ViewModels.Categories;
using System.Collections.ObjectModel;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeCategoryListViewModel : ICategoryListViewModel
    {
        public ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> CategoryList
            => new ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>>
            {
                new AlphaGroupListGroupCollection<CategoryViewModel>("A") {new CategoryViewModel {Name = "Auto"}},
                new AlphaGroupListGroupCollection<CategoryViewModel>("E")
                {
                    new CategoryViewModel {Name = "Einkaufen"}
                }
            };

        public RelayCommand AppearingCommand { get; } = null!;

        public RelayCommand<CategoryViewModel> ItemClickCommand { get; } = null!;

        public AsyncRelayCommand<string> SearchCommand { get; } = null!;

        public CategoryViewModel SelectedCategory { get; set; } = null!;

        public string SearchText { get; set; } = "";

        public bool IsCategoriesEmpty => false;
    }
}