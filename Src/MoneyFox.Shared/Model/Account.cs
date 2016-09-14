using MoneyFox.Shared.Manager;
using PropertyChanged;
using SQLite.Net.Attributes;

using System;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using System.Collections.ObjectModel;
using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Repositories;
using System.Linq;
using SQLite.Net;
using MoneyFox.Shared.Constants;
using MvvmCross.Plugins.File;
using MvvmCross.Plugins.Sqlite;
using SQLite.Net.Async;


namespace MoneyFox.Shared.Model
{
    [ImplementPropertyChanged]
    [Table("Accounts")]
    public class Account
    {
       public Account()
        {
            testy = new EndOfMonthManager(CurrentBalance, Id);

        }


        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Iban { get; set; }
        public double CurrentBalance { get; set; }
            
        public string Note { get; set; }


        public EndOfMonthManager testy;
        public string final
        {
            get
            {
                return final;
            }
            set
            {
                 final = testy.returnValue();
            }
        }
      
    }
}