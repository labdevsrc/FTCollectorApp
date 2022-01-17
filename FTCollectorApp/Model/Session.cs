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
    }
}
