namespace MoneyFox.Ui.Controls.CategorySelection;

using CommunityToolkit.Mvvm.Messaging.Messages;

public class CategorySelectedMessage : ValueChangedMessage<int>
{
    public CategorySelectedMessage(int selectedCategoryId) : base(selectedCategoryId) { }
}
