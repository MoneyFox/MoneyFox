namespace MoneyFox.Ui.Views.Categories.ModifyCategory;

public class CategoryViewModel : ObservableViewModelBase
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
        set => SetProperty(property: ref id, value: value);
    }

    public required string Name
    {
        get => name;
        set => SetProperty(property: ref name, value: value);
    }

    public required string? Note
    {
        get => note;
        set => SetProperty(property: ref note, value: value);
    }

    public required bool RequireNote
    {
        get => requireNote;
        set => SetProperty(property: ref requireNote, value: value);
    }

    public required DateTime Created
    {
        get => created;
        set => SetProperty(property: ref created, value: value);
    }

    public required DateTime? LastModified
    {
        get => lastModified;
        set => SetProperty(property: ref lastModified, value: value);
    }
}
