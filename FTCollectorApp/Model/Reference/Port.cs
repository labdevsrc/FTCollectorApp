using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.Reference
{
    public class Port
    {
        [AutoIncrement, PrimaryKey]
        public int id { get; set; }
        public string Key { get; set; }
        public string RackCount { get; set; }
    }
}
