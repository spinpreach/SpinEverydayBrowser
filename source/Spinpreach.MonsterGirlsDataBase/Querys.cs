using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Spinpreach.MonsterGirlsDataBase
{
    public class Querys
    {
        private MonsterGirlsData database;
        public Querys(MonsterGirlsData database)
        {
            this.database = database;
        }
    }
}