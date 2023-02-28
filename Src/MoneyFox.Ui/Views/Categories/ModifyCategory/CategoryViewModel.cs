namespace MoneyFox.Ui.Views.Categories.ModifyCategory;

using CommunityToolkit.Mvvm.ComponentModel;

public class CategoryViewModel : ObservableObject
{
    private DateTime created;
    private int id;
    private DateTime? lastModified;
    private string name = "";
    private string? note;
    private bool requireNote;

    public required int Id
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

    public required string Name
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

    public required string? Note
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

    public required bool RequireNote
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

    public required DateTime Created
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

    public required DateTime? LastModified
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
}
