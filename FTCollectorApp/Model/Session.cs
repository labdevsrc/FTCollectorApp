﻿using FTCollectorApp.Model.AWS;
using FTCollectorApp.Model.Reference;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FTCollectorApp.Model
{
    public static class Session
    {
        public static int uid { get; set; }
        public static string jobnum { get; set; }
        public static string jobkey { get; set; }
        //gps_sts = 1 when gpscheckedval = DeviceGPS, gpscheckedval=ExternalGPS
        //otherwise  gps_sts = 0 
        public static string gps_sts { get; set; }
        public static string manual_latti { get; set; }
        public static string manual_longi { get; set; }

        public static string live_lattitude { get; set; }
        public static string live_longitude { get; set; }

        public static string lattitude2 { get; set; }
        public static string longitude2 { get; set; }
        public static string altitude { get; set; }
        public static string accuracy { get; set; }
        public static  string lunchinsts { get; set; }

        public static string crew_leader { get; set; }

        public static string event_type { get; set; }

        public static string stage { get; set; }
        public static ArrayList sessioncrew { get; set; }
        public static string countycode { get; set; }
        public static string ownerkey { get; set; }
        public static string OwnerName { get; set; }
        public static string ownerCD { get; set; }
        public static int crewCnt { get; set; }

        public static string JobShowAll { get; set; }
        public static string lattitude_offset { get; set; }
        public static string longitude_offset { get; set; }

        public static string gps_offset_bearing { get; set; }
        public static string gps_offset_distance { get; set; }

        public static string current_page { get; set; }
        public static string tag_number { get; set; }
        public static string site_key { get; set; }
        public static string site_type_key { get; set; }
        public static int SITE_PAGE_COUNT { get; set; }
        public static string Result { get; set; }
        public static string RowId { get; set; }

        // tracing params at page Duct Trace
        public static string duct_from { get; set; }
        public static string site_from { get; set; }
        public static string show_all { get; set; }
        public static string? GpsPointMaxIdx { get; set; }
        public static string? LocpointnumberEnd { get; set; }
        public static string? LocpointnumberStart { get; set; }
        public static AFiberCable? Cable1 { get; set; }
        public static AFiberCable? Cable2 { get; set; }
        public static AFiberCable? Cable3 { get; set; }
        public static AFiberCable? Cable4 { get; set; }
        public static ConduitsGroup? FromDuct { get; set; }
        public static ConduitsGroup? ToDuct { get; set; }

        public static List<UnSyncTaskList?> TaskPendingList { get; set; }

        public static int MAX_DIR_CNT { get; set; } = 0;
        // 1: site input
        // 2 : Building/Cabinet/Pole/ input
        // 3

        public const string EventLogin = "1";
        public const string JOB_VERIFIED = "2";
        public const string CrewAssembled = "3";
        public const string LunchOut = "13";
        public const string LunchIn = "14";
        public const string ClockIn = "15";


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
