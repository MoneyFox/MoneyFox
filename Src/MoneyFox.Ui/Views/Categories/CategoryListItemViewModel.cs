namespace MoneyFox.Ui.Views.Categories;

using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
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
        set => SetProperty( ref id,   value);
    }

    public string Name
    {
        get => name;
        set => SetProperty( ref name,   value);
    }

    public bool RequireNote
    {
        get => requireNote;
        set => SetProperty( ref requireNote,   value);
    }

    public DateTime Created
    {
        get => created;
        set => SetProperty( ref created,   value);
    }

    public DateTime? LastModified
    {
        get => lastModified;
        set => SetProperty( ref lastModified,   value);
    }

    public string? Note
    {
        get => note;
        set => SetProperty( ref note,   value);
    }

    public void CreateMappings(Profile configuration)
    {
        configuration.CreateMap<Category, CategoryListItemViewModel>().ReverseMap();
    }
}
