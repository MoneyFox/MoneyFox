namespace MoneyFox.Ui.Views.Categories;

using AutoMapper;
using Core.Common.Interfaces.Mapping;
using Domain.Aggregates.CategoryAggregate;

public class CategoryListItemViewModel : ObservableViewModelBase, IHaveCustomMapping
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
        set => SetProperty(property: ref id, value: value);
    }

    public string Name
    {
        get => name;
        set => SetProperty(property: ref name, value: value);
    }

    public bool RequireNote
    {
        get => requireNote;
        set => SetProperty(property: ref requireNote, value: value);
    }

    public DateTime Created
    {
        get => created;
        set => SetProperty(property: ref created, value: value);
    }

    public DateTime? LastModified
    {
        get => lastModified;
        set => SetProperty(property: ref lastModified, value: value);
    }

    public string? Note
    {
        get => note;
        set => SetProperty(property: ref note, value: value);
    }

    public void CreateMappings(Profile configuration)
    {
        configuration.CreateMap<Category, CategoryListItemViewModel>().ReverseMap();
    }
}
