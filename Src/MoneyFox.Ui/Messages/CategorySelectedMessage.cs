namespace MoneyFox.Ui.Messages;

using CommunityToolkit.Mvvm.Messaging.Messages;

public class CategorySelectedMessage : ValueChangedMessage<CategorySelectedDataSet>
{
    public CategorySelectedMessage(CategorySelectedDataSet dataSet) : base(dataSet) { }
}

public class CategorySelectedDataSet
{
    public CategorySelectedDataSet(int categoryId, string name)
    {
        CategoryId = categoryId;
        Name = name;
    }

    public int CategoryId { get; }

    public string Name { get; }
}
