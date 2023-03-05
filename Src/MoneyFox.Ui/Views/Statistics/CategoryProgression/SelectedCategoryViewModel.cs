namespace MoneyFox.Ui.Views.Statistics.CategoryProgression;

public class SelectedCategoryViewModel : ObservableViewModelBase
{
    private int id;
    private string name = "";

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
}
