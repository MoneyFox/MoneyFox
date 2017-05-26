using System.ComponentModel.DataAnnotations;

namespace MoneyFox.DataAccess.EntityOld
{
    internal class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Notes { get; set; }
    }
}
