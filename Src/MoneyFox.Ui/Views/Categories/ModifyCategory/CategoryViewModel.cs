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
        set => SetProperty(ref id, value);
    }

    public required string Name
    {
        get => name;
        set => SetProperty(ref name, value);
    }

    public required string? Note
    {
        get => note;
        set => SetProperty(ref note, value);
    }

    public required bool RequireNote
    {
        get => requireNote;
        set => SetProperty(ref requireNote, value);
    }

    public required DateTime Created
    {
        get => created;
        set => SetProperty(ref created, value);
    }

    public required DateTime? LastModified
    {
        get => lastModified;
        set => SetProperty(ref lastModified, value);
    }
}
