namespace MoneyFox.Ui.Views.Statistics.CategoryProgression;

using CommunityToolkit.Mvvm.ComponentModel;

public class SelectedCategoryViewModel : ObservableViewModelBase
{
    private int id;
    private string name = "";

    public required int Id
    {
        get => id;
        set => SetProperty( ref id,   value);
    }

    public required string Name
    {
        get => name;
        set => SetProperty( ref name,   value);
    }
}
