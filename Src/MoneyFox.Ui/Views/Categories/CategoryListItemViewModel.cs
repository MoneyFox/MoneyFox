namespace MoneyFox.Ui.Views.Categories;

using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Common.Interfaces.Mapping;
using Domain.Aggregates.CategoryAggregate;

public class CategoryListItemViewModel : ObservableObject, IHaveCustomMapping
{
    private DateTime created;
    private int id;
    private DateTime? lastModified;
    private string name = "";
    private string? note = "";
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

    public DateTime Created
    {
        get => created;
        set => SetProperty(field: ref created, newValue: value);
    }

    public DateTime? LastModified
    {
        get => lastModified;
        set => SetProperty(field: ref lastModified, newValue: value);
    }

    public string? Note
    {
        get => note;
        set => SetProperty(field: ref note, newValue: value);
    }

    public void CreateMappings(Profile configuration)
    {
        configuration.CreateMap<Category, CategoryListItemViewModel>().ReverseMap();
    }
}
