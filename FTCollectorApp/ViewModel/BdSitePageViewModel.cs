﻿using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web;

namespace FTCollectorApp.ViewModel
{
    public class BdSitePageViewModel
    {

        public ObservableCollection<CodeSiteType> CodeSite
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CodeSiteType>();
                    var table = conn.Table<CodeSiteType>().ToList();
                    return new ObservableCollection<CodeSiteType>(table);
                }
            }
        }

        public ObservableCollection<BuildingType> BuildingTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<BuildingType>();
                    var bdClassiTable = conn.Table<BuildingType>().ToList();
                    return new ObservableCollection<BuildingType>(bdClassiTable);
                }
            }
        }
        public ObservableCollection<Mounting> MountingTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Mounting>();
                    var mountingTable = conn.Table<Mounting>().ToList();
                    return new ObservableCollection<Mounting>(mountingTable);
                }
            }
        }
        public ObservableCollection<InterSectionRoad> IntersectionList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<InterSectionRoad>();
                    var intersectionTable = conn.Table<InterSectionRoad>().ToList();
                    var data = intersectionTable.Where(a => a.OWNER_CD == Session.ownerCD).ToList();
                    return new ObservableCollection<InterSectionRoad>(data);
                }
            }
        }
        public ObservableCollection<Roadway> RoadwayList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Roadway>();
                    var rwTable = conn.Table<Roadway>().ToList();
                    var table = rwTable.Where(a => a.RoadOwnerKey == Session.ownerkey).ToList();
                    return new ObservableCollection<Roadway>(table);
                }
            }
        }
        public ObservableCollection<CompassDirection> TravelDirectionList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CompassDirection>();
                    var data = conn.Table<CompassDirection>().ToList();
                    return new ObservableCollection<CompassDirection>(data);
                }
            }
        }

        public ObservableCollection<MaterialCode> MaterialCodeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<MaterialCode>();
                    var table = conn.Table<MaterialCode>().ToList();
                    foreach (var col in table)
                    {
                        col.CodeDescription = HttpUtility.HtmlDecode(col.CodeDescription); // should use for escape char "
                    }
                    return new ObservableCollection<MaterialCode>(table);
                }
            }
        }


        public ObservableCollection<FilterType> FilterTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<FilterType>();
                    var table = conn.Table<FilterType>().ToList();
                    foreach (var col in table)
                    {
                        col.FilterTypeDesc = HttpUtility.HtmlDecode(col.FilterTypeDesc); // should use for escape char "
                    }
                    return new ObservableCollection<FilterType>(table);
                }
            }
        }

        public ObservableCollection<FilterSize> FilterSizeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<FilterSize>();
                    var table = conn.Table<FilterSize>().ToList();
                    foreach (var col in table)
                    {
                        col.data = HttpUtility.HtmlDecode(col.data); // should use for escape char "
                    }
                    return new ObservableCollection<FilterSize>(table);
                }
            }
        }

        public ObservableCollection<Tracewaretag> TracewiretagList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Tracewaretag>();
                    var rwTable = conn.Table<Tracewaretag>().ToList();
                    var table = rwTable.Where(a => a.SiteOwnerKey == Session.ownerkey).ToList();
                    return new ObservableCollection<Tracewaretag>(table);
                }
            }
        }

        public ObservableCollection<Orientation> OrientationList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Orientation>();
                    var table = conn.Table<Orientation>().ToList();
                    return new ObservableCollection<Orientation>(table);
                }
            }
        }

        //// Cabinet Page : 
        ///  - have Manufacturer List and Model List
        ///  - didn't have Filter Type and Filter Size
        public ObservableCollection<Manufacturer> ManufacturerList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Manufacturer>();
                    var table = conn.Table<Manufacturer>().ToList();
                    foreach (var col in table)
                    {
                        col.ManufName = HttpUtility.HtmlDecode(col.ManufName); // should use for escape char "
                    }
                    return new ObservableCollection<Manufacturer>(table);
                }
            }
        }

        public ObservableCollection<DevType> ModelList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<DevType>();
                    var table = conn.Table<DevType>().ToList();
                    foreach (var col in table)
                    {
                        col.DevTypeDesc = HttpUtility.HtmlDecode(col.DevTypeDesc); // should use for escape char "
                    }
                    return new ObservableCollection<DevType>(table);
                }
            }
        }

    }
}
