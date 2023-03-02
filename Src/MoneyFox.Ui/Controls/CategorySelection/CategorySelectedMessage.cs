namespace MoneyFox.Ui.Controls.CategorySelection;

using CommunityToolkit.Mvvm.Messaging.Messages;

public class CategorySelectedMessage : ValueChangedMessage<CategorySelectedDataSet>
{
    public CategorySelectedMessage(CategorySelectedDataSet dataSet) : base(dataSet) { }
}

public record CategorySelectedDataSet(int CategoryId, string Name);
