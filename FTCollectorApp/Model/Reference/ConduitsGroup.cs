using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.Reference
{
    public class ConduitsGroup
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string ConduitKey { get; set; }
        public string HosTagNumber { get; set; }
    }
}
