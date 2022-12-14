namespace MoneyFox.Ui.Views.Categories.ModifyCategory;

using CommunityToolkit.Mvvm.ComponentModel;

internal class CategoryViewModel : ObservableObject
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
        set => SetProperty(field: ref id, newValue: value);
    }

    public required string Name
    {
        get => name;
        set => SetProperty(field: ref name, newValue: value);
    }

    public required string? Note
    {
        get => note;
        set => SetProperty(field: ref note, newValue: value);
    }

    public required bool RequireNote
    {
        get => requireNote;
        set => SetProperty(field: ref requireNote, newValue: value);
    }

    public required DateTime Created
    {
        get => created;
        set => SetProperty(field: ref created, newValue: value);
    }

    public required DateTime? LastModified
    {
        get => lastModified;
        set => SetProperty(field: ref lastModified, newValue: value);
    }
}
