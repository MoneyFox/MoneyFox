namespace MoneyFox.Ui.Views.Categories;

using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Common.Interfaces.Mapping;
using Domain.Aggregates.CategoryAggregate;

public class CategoryListItemViewModel : ObservableObject, IHaveCustomMapping
{
    private int id;
    private string name = "";
    private bool requireNote;

    public int Id
    {
        get => id;
        set => SetProperty(field: ref id, newValue: value);
    }

    public string Name
    {
        get => name;
        set => SetProperty(field: ref name, newValue: value);
    }

    public bool RequireNote
    {
        get => requireNote;
        set => SetProperty(field: ref requireNote, newValue: value);
    }

    public void CreateMappings(Profile configuration)
    {
        configuration.CreateMap<Category, CategoryListItemViewModel>().ReverseMap();
    }
}
