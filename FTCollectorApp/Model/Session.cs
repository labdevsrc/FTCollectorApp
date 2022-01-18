using System;
using System.Collections.Generic;
using System.Text;

namespace FTCollectorApp.Model
{
    public static class Session
    {
        public static int uid { get; set; }
        public static string jobnum { get; set; }

        //gps_sts = 1 when gpscheckedval = DeviceGPS, gpscheckedval=ExternalGPS
        //otherwise  gps_sts = 0 
        public static string gps_sts { get; set; }
        public static string manual_latti { get; set; }
        public static string manual_longi { get; set; }
        public static string lattitude2 { get; set; }
        public static string longitude2 { get; set; }
        public static  string lunchinsts { get; set; }

        public static int event_type { get; set; }

        /* 1	employee login
        2	job verified
        3	crew assembled
        4	equipment checked out
        5	vehicle inspected
        6	left for job site
        7	arrived at job site
        8	Left Job Site
        9	Arrived Back at Yard
        10	Equipment Return
        12	Employee Log Out
        13	Lunch Out
        14	Lunch In
        15	Clock In
        16	Clock Out
        17	Employee Added to Job
        18	Employee Left Job   */
    }
}
