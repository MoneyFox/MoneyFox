using System.Collections.Generic;
using System.Text;

namespace MoneyFox.DataLayer.Entities
{
    public class Tag
    {
        /// <summary>
        ///  EF Core Constructor
        /// </summary>
        private Tag() { }

        public Tag(string name)
        {

        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public virtual IList<PaymentTag> PaymentTags { get; set; }
    }
}
