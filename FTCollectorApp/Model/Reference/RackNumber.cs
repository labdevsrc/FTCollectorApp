using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class RackNumber
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string RackNumKey { get; set; }
        public string OWNER_CD { get; set; }
        public string Racknumber { get; set; }
        public string SiteId { get; set; }
        public string RackType { get; set; }
    }
}
