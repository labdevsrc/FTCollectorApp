using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class AFiberCable
    {
        public int id { get; set; }
        public string AFRKey { get; set; }
        public string CableIdDesc { get; set; }
        public string JobKey { get; set; }
        public string JobNumber { get; set; }
        public string OwnerKey { get; set; }
    }
}
