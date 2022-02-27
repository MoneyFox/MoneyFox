namespace MoneyFox.Core._Pending_.Common.Messages
{
    using CommunityToolkit.Mvvm.Messaging.Messages;

    public class CategorySelectedMessage : ValueChangedMessage<int>
    {
        public CategorySelectedMessage(int categoryId) : base(categoryId)
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; set; }
    }
}