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

        set
        {
            if (id == value)
            {
                return;
            }

            id = value;
            OnPropertyChanged();
        }
    }

    public string Name
    {
        get => name;

        set
        {
            if (name == value)
            {
                return;
            }

            name = value;
            OnPropertyChanged();
        }
    }

    public bool RequireNote
    {
        get => requireNote;

        set
        {
            if (requireNote == value)
            {
                return;
            }

            requireNote = value;
            OnPropertyChanged();
        }
    }

    public DateTime Created
    {
        get => created;

        set
        {
            if (created == value)
            {
                return;
            }

            created = value;
            OnPropertyChanged();
        }
    }

    public DateTime? LastModified
    {
        get => lastModified;

        set
        {
            if (lastModified == value)
            {
                return;
            }

            lastModified = value;
            OnPropertyChanged();
        }
    }

    public string? Note
    {
        get => note;

        set
        {
            if (note == value)
            {
                return;
            }

            note = value;
            OnPropertyChanged();
        }
    }

    public void CreateMappings(Profile configuration)
    {
        configuration.CreateMap<Category, CategoryListItemViewModel>().ReverseMap();
    }
}
