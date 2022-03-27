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

        public string Direction { get; set; }
        public string DirCnt { get; set; }
        public string HosTagNumber { get; set; }
        public string HostType { get; set; }
        public string DuctUsage { get; set; }
        public string DuctSize { get; set; }
        public string DuctColor { get; set; }

        public string WhichDucts { get; set; }
    }
}
