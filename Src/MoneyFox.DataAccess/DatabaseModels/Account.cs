using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace MoneyFox.DataAccess.DatabaseModels
{
    [Table("Accounts")]
    internal class Account
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Iban { get; set; }
        public double CurrentBalance { get; set; }
        public string Note { get; set; }
        public bool IsOverdrawn { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]
        public List<Payment> Payments { get; set; }
    }
}