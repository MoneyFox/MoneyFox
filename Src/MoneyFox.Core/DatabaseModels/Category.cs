using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyFox.Core.DatabaseModels
{
    [Table("Categories")]
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}