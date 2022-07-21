using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.AWS
{
    public class UnSyncTaskList
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string targetTable { get; set; }
        public string rowCount { get; set; }
        public string ajaxTarget { get; set; }
        public string taskName { get; set; }
        public string status { get; set; } = "UNSYNC";
    }
}
