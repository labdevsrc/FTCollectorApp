using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.View.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuildingSitePage : ContentPage
    {
        string MajorMinorType;
        string TagNumber;
        List<string> DotDistrict = new List<string>();
        List<string> YesNo = new List<string>();

        List<string>MountingTypes = new List<string>();



        // key list 
        List<string> roadWayKeyList = new List<string>();
        List<string> mountingKeyList = new List<string>();
        List<string> dirTravelKeyList = new List<string>();
        List<string> buildingClassiKeyList = new List<string>();

        public BuildingSitePage(string majorType, string minorType, string tagNumber)
        {
            InitializeComponent();
            MajorMinorType = $"{majorType} - {minorType}";

            for (int i = 0; i < 100; i++)
            {
                DotDistrict.Add(i.ToString());
            }

            YesNo.Add("N");
            YesNo.Add("Y");

            pickerDotDisctrict.ItemsSource = DotDistrict;
            pickerElectSiteKey.ItemsSource = DotDistrict;
            pickerHasPowerDisconnect.ItemsSource = YesNo;
            picker3rdpComms.ItemsSource = YesNo;
            pickerLaneClosure.ItemsSource = YesNo;
            buildingClass.ItemsSource = getBuildingClassiList();
            pIntersection.ItemsSource = getIntersectionList();
            pRoadway.ItemsSource = getRoadwayList();

            pDirTravel.ItemsSource = getDirection();
            pOrientation.ItemsSource = getDirection();


            pMaterial.ItemsSource = getMaterialCodeList();

            pFilterType.ItemsSource = getFilterType();
            pFilterSize.ItemsSource = getFilterSize();


            pHaveSunShield.ItemsSource = YesNo;
            pHasGround.ItemsSource = YesNo;
            pHasKey.ItemsSource = YesNo;
            pKeyType.ItemsSource = DotDistrict;
            pDirectionTravel.ItemsSource = DotDistrict;
            pIsSiteClearZone.ItemsSource = YesNo;
            pBucketTruck.ItemsSource = YesNo;

            pMounting.ItemsSource = getMountingTypeList();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            IsBusy = true;
            entrySiteType.Text = MajorMinorType;
            entryTagNum.Text = TagNumber;
            ownerName.Text = Session.OwnerName;

            IsBusy = false;


        }

        private void btnCamera(object sender, EventArgs e)
        {

            Navigation.PushAsync(new CameraViewPage());
        }



        List<string> getBuildingClassiList()
        {
            List<string> buildingClassiList = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<BuildingType>();
                var bdClassiTable = conn.Table<BuildingType>().ToList();
                foreach (var col in bdClassiTable)
                {
                    buildingClassiList.Add(col.TYPE_DESC);
                    buildingClassiKeyList.Add(col.BuildingTypeKey);
                }
                return buildingClassiList;
            }
            /*buildingClassification.Add("City Facility");
            buildingClassification.Add("County Facility");
            buildingClassification.Add("Other Agency Facility");
            buildingClassification.Add("Private Partner Facility");
            buildingClassification.Add("Shelter – Aboveground");
            buildingClassification.Add("Shelter – Underground");
            buildingClassification.Add("Utility Company Facility");

            return buildingClassification;*/
        }

        List<string> getRoadwayList()
        {
            List<string> roadWayList = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Roadway>();
                var roadwayTable= conn.Table<Roadway>().ToList();

                conn.CreateTable<OwnerRoadway>();
                var ownerRoadwayTable = conn.Table<OwnerRoadway>().ToList();

                // get roadway with owner = Session.owner
                var RoadwayList1 = ownerRoadwayTable.Where(a => a.OR_Owner == Session.ownerkey).Select(b => b.OR_Roadway).ToList();
                
                roadWayList.Add("----Select-----");
                foreach (string col in RoadwayList1)
                {
                    roadWayList.Add(roadwayTable.Where(a => a.RoadwayKey == col.ToString()).Select(b => b.RoadwayName).First());
                    //roadWayKeyList.Add(col.RoadwayKey);
                }
                return roadWayList;
            }
            /*roadWay.Add("Crosstown Parkway");
            roadWay.Add("US 1");
            roadWay.Add("SE Floesta Drive");
            roadWay.Add("SE Sandia Drive");
            roadWay.Add("SE Manth Lane");
            roadWay.Add("I95");
            roadWay.Add("SW Rosser Blvd");
            roadWay.Add("SW Gatlin  Blvd");
            roadWay.Add("SW Savona  Blvd");
            roadWay.Add("SW Tradition Pkwy");
            return roadWay;*/
        }


        List<string> getIntersectionList()
        {
            List<string> intersectionList = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<InterSectionRoad>();
                var intersectionTable = conn.Table<InterSectionRoad>().ToList();

                var data = intersectionTable.Where(a => a.OWNER_CD == Session.ownerCD).Select(a => a.IntersectionName).ToList();

                return data;
            }
            /*roadWay.Add("Crosstown Parkway");
            roadWay.Add("US 1");
            roadWay.Add("SE Floesta Drive");
            roadWay.Add("SE Sandia Drive");
            roadWay.Add("SE Manth Lane");
            roadWay.Add("I95");
            roadWay.Add("SW Rosser Blvd");
            roadWay.Add("SW Gatlin  Blvd");
            roadWay.Add("SW Savona  Blvd");
            roadWay.Add("SW Tradition Pkwy");
            return roadWay;*/
        }


        List<string> getDirection()
        {
            List<string> dirTravelList = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Direction>();
                var directionTable = conn.Table<Direction>().ToList();
                dirTravelList.Add("----Select----");
                foreach (var col in directionTable)
                {
                    dirTravelList.Add(col.DirDesc);
                    dirTravelKeyList.Add(col.DirKey);
                }
                return dirTravelList;
            }
            /*dirTravel.Add("North");
            dirTravel.Add("North East");
            dirTravel.Add("East");
            dirTravel.Add("South East");
            dirTravel.Add("South");
            dirTravel.Add("South West");
            dirTravel.Add("West");
            dirTravel.Add("North West");
            dirTravel.Add("Median");
            dirTravel.Add("On Ramp");
            dirTravel.Add("Unknown");

            return dirTravel;*/
        }

        List<string> getMaterialCodeList()
        {
            List<string> materialCodeList = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<MaterialCode>();
                var matCodeTable= conn.Table<MaterialCode>().ToList();
                foreach (var col in matCodeTable)
                {
                    materialCodeList.Add(col.CodeDescription);
                }
                //Mountings = new ObservableCollection<Mounting>(mountingTable);
                return MountingTypes;
            }
            /*material.Add("Composite");
            material.Add("Concrete");
            material.Add("Fiberglass");
            material.Add("Galvanized Steel");
            material.Add("Plastic");
            material.Add("Cast Iron");
            material.Add("Polymer");
            material.Add("Metal");
            material.Add("CONC_W/METAL_LID");
            material.Add("Steel");
            material.Add("Aluminium");

            return material;*/
        }

        List<string> getMountingTypeList()
        {
            List<string> mountingTypeList = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Mounting>();
                var mountingTable = conn.Table<Mounting>().ToList();
                foreach (var col in mountingTable)
                {
                    mountingTypeList.Add(col.MountingType);
                }
                //Mountings = new ObservableCollection<Mounting>(mountingTable);
                return mountingTypeList;
            }

            /*List<string> mounting = new List<string>();
            mounting.Add("Butterfly");
            mounting.Add("In Pavement");
            mounting.Add("Under Pavement");
            mounting.Add("Bridge");
            mounting.Add("Cantilever");
            mounting.Add("Concrete Pad");
            mounting.Add("Overhead Span");
            mounting.Add("Pier");
            mounting.Add("Pole");
            mounting.Add("Strut");
            mounting.Add("Wall");
            mounting.Add("Inside of Cabinet");
            mounting.Add("Outside of Cabinet");
            mounting.Add("Unistrut");
            mounting.Add("Building");
            mounting.Add("Pedistal");
            mounting.Add("Messenger Wire");
            mounting.Add("Mast Arm");
            mounting.Add("Street Light");
            mounting.Add("In Ground");

            return mounting;*/
        }

        List<string> getSide()
        {
            List<string> sideList = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Site>();
                var Sites = conn.Table<Site>().ToList();
                var TagNumbers = Sites.Where(a => (a.CreatedBy == Session.uid.ToString()) 
                && (a.JobNumber == Session.jobnum) 
                && (a.OWNER_CD == Session.ownerCD)).Select(g => g.TagNumber).ToList();

                return TagNumbers;
            }
        }

        List<string> getCables2()
        {
            List<string> Cables = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<AFiberCable>();
                var a_fiber_cables = conn.Table<AFiberCable>().ToList();
                var CableIdListbyJobNum = a_fiber_cables.Where(a => a.JobNumber == Session.jobnum).Select(g => g.CableIdKey).ToList();

                return CableIdListbyJobNum;
            }
        }

        List<string> getCables1()
        {
            List<string> Cables = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<AFiberCable>();
                var a_fiber_cables = conn.Table<AFiberCable>().ToList();

                var CableIdListbyOwnerKey = a_fiber_cables.Where(a => a.OwnerKey == Session.ownerkey).Select(g => g.CableIdKey).ToList();
                return CableIdListbyOwnerKey;
            }
        }

        List<string> getTracewaretag()
        {
            List<string> TraceWareTagList = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Tracewaretag>();
                var Sites = conn.Table<Tracewaretag>().ToList();
                var TagNumbers = Sites.Where(a => a.SiteOwnerKey == Session.ownerkey).Select(b => b.SiteTagNumber).ToList();

                return TraceWareTagList;
            }
        }

        List<string> getFilterType()
        {
            List<string> filterTypeList = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<FilterType>();
                var ftTypeTable = conn.Table<FilterType>().ToList();
                filterTypeList.Add("----Select----");
                foreach (var col in ftTypeTable)
                {
                    filterTypeList.Add(col.FilterTypeDesc);
                }
                //Mountings = new ObservableCollection<Mounting>(mountingTable);
                return filterTypeList;
            }
            /*filterType.Add("Fabric");
            filterType.Add("Metal");
            filterType.Add("Paper");
            filterType.Add("Fiberglass");
            filterType.Add("Sponge");
            filterType.Add("NONE");
            filterType.Add("Unknown");

            return filterType;*/
        }

        List<string> getFilterSize()
        {
            List<string> filterSizeList = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<FilterSize>();
                var ftTypeTable = conn.Table<FilterSize>().ToList();
                filterSizeList.Add("---Select---");
                foreach (var col in ftTypeTable)
                {
                    var temp = HttpUtility.HtmlDecode(col.data);
                    filterSizeList.Add(temp);
                }
                //Mountings = new ObservableCollection<Mounting>(mountingTable);
                return filterSizeList;
            }
            /*filterSize.Add("12\" x 12\" x 1\"");
            filterSize.Add("12\" x 14\" x 1\"");
            filterSize.Add("12\" x 16\" x 1\"");
            filterSize.Add("12\" x 18\" x 1\"");
            filterSize.Add("12\" x 25\" x 1\"");

            return filterSize;*/
        }

        private void btnCamera2(object sender, EventArgs e)
        {

        }
    }
}