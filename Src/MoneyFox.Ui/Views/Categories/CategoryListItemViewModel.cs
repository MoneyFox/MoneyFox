namespace MoneyFox.Ui.Views.Categories;

using CommunityToolkit.Mvvm.ComponentModel;

public class CategoryListItemViewModel : ObservableObject
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
}
