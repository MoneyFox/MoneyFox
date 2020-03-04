using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyFox.Domain.Entities
{
    public class Category
    {
        //used by EF Core
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private Category()
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
        }

        public Category(string name, string note = "")
        {
            CreationTime = DateTime.Now;
            Payments = new List<Payment>();

            UpdateData(name, note);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required] public string Name { get; private set; }

        public string? Note { get;set; }

        public DateTime ModificationDate { get; private set; }

        public DateTime CreationTime { get; private set; }

        public List<Payment> Payments { get; private set; }

        public void UpdateData(string name, string note = "")
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            Note = note;
            ModificationDate = DateTime.Now;
        }
    }
}
