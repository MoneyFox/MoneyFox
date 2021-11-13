namespace MoneyFox.Application.Common.Messages
{
    public class CategorySelectedMessage
    {
        public CategorySelectedMessage(int categoryId)
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; set; }
    }
}
