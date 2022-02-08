using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.Reference
{
    public class Owner
    {
        [PrimaryKey,AutoIncrement]
        public int id { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }


    }
}
