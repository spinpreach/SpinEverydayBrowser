using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Spinpreach.MonsterGirlsDataBase.Tables.Transactions;

namespace Spinpreach.MonsterGirlsDataBase.Tables
{
    public class Table
    {
        public Master master { get; set; } = new Master();
        public Transaction transaction { get; set; } = new Transaction();
        public TimeSpan ts = new TimeSpan(0);

        public class Master
        {
        }

        public class Transaction
        {
            //public Castlekeep castlekeep { get; set; } = new Castlekeep();
            //public Party party { get; set; } = new Party();
            //public Resource resource { get; set; } = new Resource();
        }
    }
}