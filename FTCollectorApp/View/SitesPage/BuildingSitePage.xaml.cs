using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Service;
using FTCollectorApp.Utils;
using FTCollectorApp.View.Utils;
using FTCollectorApp.ViewModel;
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


        string Notes;
        string InstalledAt, Manufactured;

        public BuildingSitePage(string majorType, string minorType, string tagNumber)
        {
            InitializeComponent();
            BindingContext = new BdSitePageViewModel();


            MajorMinorType = $"{majorType} - {minorType}";

            for (int i = 0; i < 100; i++)
            {
                DotDistrict.Add(i.ToString());
            }

            YesNo.Add("N");
            YesNo.Add("Y");

            TagNumber = tagNumber;
            entryTagNum.Text = tagNumber;
            pickerDotDisctrict.ItemsSource = DotDistrict;
            pickerElectSiteKey.ItemsSource = DotDistrict;
            pickerHasPowerDisconnect.ItemsSource = YesNo;
            picker3rdpComms.ItemsSource = YesNo;
            pickerLaneClosure.ItemsSource = YesNo;

            pHaveSunShield.ItemsSource = YesNo;
            pHasGround.ItemsSource = YesNo;
            pHasKey.ItemsSource = YesNo;
            pKeyType.ItemsSource = DotDistrict;
            pIsSiteClearZone.ItemsSource = YesNo;
            pBucketTruck.ItemsSource = YesNo;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            IsBusy = true;
            entrySiteType.Text = MajorMinorType;
            entryTagNum.Text = TagNumber;
            ownerName.Text = Session.OwnerName;

            //buildingClass.ItemsSource = BuildingTypeList;
            buildingClass.SelectedIndexChanged += OnItemSelectedIndexChange;

            //pIntersection.ItemsSource = IntersectionList;
            pIntersection.SelectedIndexChanged += OnItemSelectedIndexChange;

            //pRoadway.ItemsSource = RoadwayList;
            pRoadway.SelectedIndexChanged += OnItemSelectedIndexChange;

            //pDirTravel.ItemsSource = TravelDirectionList;
            pDirTravel.SelectedIndexChanged += OnItemSelectedIndexChange;

            //pOrientation.SelectedIndexChanged += OnItemSelectedIndexChange;
            pMaterial.SelectedIndexChanged += OnItemSelectedIndexChange;

            //pMounting.ItemsSource = MountingTypeList;
            pMounting.SelectedIndexChanged += OnItemSelectedIndexChange;


            pFilterType.SelectedIndexChanged += OnItemSelectedIndexChange;
            pFilterSize.SelectedIndexChanged += OnItemSelectedIndexChange;


            Notes = editorNotes.Text;

            pHaveSunShield.SelectedIndexChanged += OnItemSelectedIndexChange;
            pHasGround.SelectedIndexChanged += OnItemSelectedIndexChange;
            pHasKey.SelectedIndexChanged += OnItemSelectedIndexChange;
            pKeyType.SelectedIndexChanged += OnItemSelectedIndexChange;
            //pDirectionTravel.SelectedIndexChanged += OnItemSelectedIndexChange;
            pBucketTruck.SelectedIndexChanged += OnItemSelectedIndexChange;
            pIsSiteClearZone.SelectedIndexChanged += OnItemSelectedIndexChange;
            pickerLaneClosure.SelectedIndexChanged += OnItemSelectedIndexChange;
            pickerDotDisctrict.SelectedIndexChanged += OnItemSelectedIndexChange;
            pickerHasPowerDisconnect.SelectedIndexChanged += OnItemSelectedIndexChange;
            pickerElectSiteKey.SelectedIndexChanged += OnItemSelectedIndexChange;
            picker3rdpComms.SelectedIndexChanged += OnItemSelectedIndexChange;

            dateManufactured.DateSelected += OnDateSelected;
            dateInstalled.DateSelected += OnDateSelected;


            IsBusy = false;


        }

        int IsBucketTruck = 0;
        int DirectionTravel = 0, IsLaneClosure = 0, DotDistrictCnt = 0, IsHasPowerDisconnect = 0, 
            IsHaveSunShield = 0, IsHasGround = 0, IsHasKey =0, ElectSiteKeyCnt = 0, Is3rdComms = 0;
        int KeyType;
        int IsSiteClearZone;
        string buildingClassiKeySelected, IntersectionSelected, RoadwaySelected, TravelDirSelected, Orientation, MaterialCodeKeySelected;
        string MountingSelected, FilterTypeSelected, FilterSizeKeySelected, OrientationSelected;

        private void OnItemSelectedIndexChange(object sender, EventArgs e)
        {
            IsHaveSunShield =  pHaveSunShield.SelectedIndex == -1 ? 0 : pHaveSunShield.SelectedIndex;
            IsHasGround = pHasGround.SelectedIndex == -1 ? 0 : pHasGround.SelectedIndex;
            IsHasKey = pHasKey.SelectedIndex == -1 ? 0 : pHasKey.SelectedIndex;
            KeyType = pKeyType.SelectedIndex == -1 ? 0 : pKeyType.SelectedIndex;
            //DirectionTravel = pDirectionTravel.SelectedIndex == -1 ? 0 : pDirectionTravel.SelectedIndex;
            IsBucketTruck =    pBucketTruck.SelectedIndex == -1 ? 0 : pBucketTruck.SelectedIndex;
            IsSiteClearZone = pIsSiteClearZone.SelectedIndex == -1 ? 0 : pIsSiteClearZone.SelectedIndex;
            IsLaneClosure = pickerLaneClosure.SelectedIndex == -1 ? 0 : pickerLaneClosure.SelectedIndex;
            DotDistrictCnt = pickerDotDisctrict.SelectedIndex == -1 ? 0 : pickerDotDisctrict.SelectedIndex;
            IsHasPowerDisconnect = pickerHasPowerDisconnect.SelectedIndex == -1 ? 0 : pickerHasPowerDisconnect.SelectedIndex;
            ElectSiteKeyCnt = pickerElectSiteKey.SelectedIndex == -1 ? 0 : pickerElectSiteKey.SelectedIndex;
            Is3rdComms = picker3rdpComms.SelectedIndex == -1 ? 0 : picker3rdpComms.SelectedIndex;
            IsSiteClearZone = pIsSiteClearZone.SelectedIndex == -1 ? 0 : pIsSiteClearZone.SelectedIndex;


            if (buildingClass.SelectedIndex != -1)
            {
                var selected = buildingClass.SelectedItem as BuildingType;
                buildingClassiKeySelected = selected.BuildingTypeKey;
            }
            if (pMounting.SelectedIndex != -1)
            {
                var selected = pMounting.SelectedItem as Mounting;
                MountingSelected = selected.MountingKey;  /// object reference not set to instance
            }

            if (pIntersection.SelectedIndex != -1)
            {
                var selected = pIntersection.SelectedItem as InterSectionRoad;
                IntersectionSelected = selected.IntersectionKey;
            }
            if (pRoadway.SelectedIndex != -1)
            {
                var selected = pRoadway.SelectedItem as Roadway;
                RoadwaySelected = selected.RoadwayKey;
            }
            if (pDirTravel.SelectedIndex != -1)
            {
                var selected = pDirTravel.SelectedItem as CompassDirection;
                TravelDirSelected = selected.ITSFM;
            }

            if (pOrientation.SelectedIndex != -1)
            {
                var selected = pOrientation.SelectedItem as Orientation;
                OrientationSelected = selected.OrientationDetail;
            }

            ///////////////////////////////////////////////////////////////////

            if (pMaterial.SelectedIndex != -1)
            {
                var selected = pMaterial.SelectedItem as MaterialCode;
                MaterialCodeKeySelected = selected.MaterialKey;
            }
            if (pFilterSize.SelectedIndex != -1)
            {
                var selected = pFilterSize.SelectedItem as FilterSize;
                FilterSizeKeySelected = selected.FtSizeKey;
            }

            if (pFilterType.SelectedIndex != -1)
            {
                var selected = pFilterType.SelectedItem as FilterType;
                FilterTypeSelected = selected.FilterTypeDesc;
            }
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
                //buildingClassiKeyList.Clear();
                foreach (var col in bdClassiTable)
                {
                    buildingClassiList.Add(col.TYPE_DESC);
                    //buildingClassiKeyList.Add(col.BuildingTypeKey);
                }
                return buildingClassiList;
            }
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
                
                //roadWayList.Add("----Select-----");
                foreach (string col in RoadwayList1)
                {
                    roadWayList.Add(roadwayTable.Where(a => a.RoadwayKey == col.ToString()).Select(b => b.RoadwayName).First());
                    //roadWayKeyList.Add(col.RoadwayKey);
                }
                return roadWayList;
            }
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
        }


        List<string> getDirection()
        {
            List<string> dirTravelList = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Direction>();
                var directionTable = conn.Table<Direction>().ToList();
                //dirTravelList.Add("----Select----");
                foreach (var col in directionTable)
                {
                    dirTravelList.Add(col.DirDesc);
                    //dirTravelKeyList.Add(col.DirKey);
                }
                return dirTravelList;
            }
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
                return materialCodeList;
            }
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
                foreach (var col in ftTypeTable)
                {
                    filterTypeList.Add(col.FilterTypeDesc);
                }
                return filterTypeList;
            }
        }

        List<string> getFilterSize()
        {
            List<string> filterSizeList = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<FilterSize>();
                var ftTypeTable = conn.Table<FilterSize>().ToList();
                //filterSizeList.Add("---Select---");
                foreach (var col in ftTypeTable)
                {
                    var temp = HttpUtility.HtmlDecode(col.data);
                    filterSizeList.Add(temp);
                }
                return filterSizeList;
            }
        }

        private void btnCamera2(object sender, EventArgs e)
        {

        }

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            InstalledAt = dateInstalled.Date.ToString("MM-dd-yyyy");
            Manufactured = dateManufactured.Date.ToString("MM-dd-yyyy");
        }


        List<KeyValuePair<string, string>> keyvaluepair()
        {


            var keyValues = new List<KeyValuePair<string, string>>{
                //new KeyValuePair<string, string>("jobnum",Session.jobnum),
                new KeyValuePair<string, string>("jno",Session.jobnum), //  7 
                new KeyValuePair<string, string>("uid", Session.uid.ToString()), //1
                new KeyValuePair<string, string>("tag",TagNumber), //8
                //new KeyValuePair<string, string>("typecode",typecode),
                new KeyValuePair<string, string>("plansheet","0"),
                new KeyValuePair<string, string>("psitem","0"),

                //new KeyValuePair<string, string>("gps_offset_latitude", offsetLat),
                //new KeyValuePair<string, string>("gps_offset_longitude", offsetLon),
                new KeyValuePair<string, string>("LATITUDE", Session.lattitude2),
                new KeyValuePair<string, string>("LONGITUDE", Session.longitude2),
                new KeyValuePair<string, string>("altitude", Session.altitude),  //4
                new KeyValuePair<string, string>("accuracy", Session.accuracy), //3

                //new KeyValuePair<string, string>("evtype", Session.event_type),
                
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // 2
                new KeyValuePair<string, string>("owner", Session.ownerkey), //5
                new KeyValuePair<string, string>("user", Session.uid.ToString()),
                new KeyValuePair<string, string>("stage", Session.stage),
                //new KeyValuePair<string, string>("gpstime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new KeyValuePair<string, string>("ownerCD", Session.ownerCD), // 6
                new KeyValuePair<string, string>("owner_key", Session.ownerkey),
                new KeyValuePair<string, string>("jobkey", Session.jobkey),
                //new KeyValuePair<string, string>("createdfrm", "field collection"),
                new KeyValuePair<string, string>("usercounty", Session.countycode),
                //new KeyValuePair<string, string>("ajaxname", Constants.CreateSiteTableUrl),



                new KeyValuePair<string, string>("serialno", entrySerial.Text),
                new KeyValuePair<string, string>("notes", Notes),
                new KeyValuePair<string, string>("roadway", RoadwaySelected),
                new KeyValuePair<string, string>("intersect2", IntersectionSelected),
                new KeyValuePair<string, string>("notes", Notes),
                new KeyValuePair<string, string>("sunshield2", IsHaveSunShield == 1 ? "1":"0"),
                new KeyValuePair<string, string>("ground", IsHasGround == 1 ? "1":"0"),


                new KeyValuePair<string, string>("bucket2", IsBucketTruck == 1 ? "1":"0"),
                new KeyValuePair<string, string>("laneclosure", IsLaneClosure == 1 ? "1":"0"),
                new KeyValuePair<string, string>("dotdis",  DotDistrictCnt.ToString()),


                new KeyValuePair<string, string>("mounting1", MountingSelected),
                new KeyValuePair<string, string>("gps_offset_longitude", ""),
                new KeyValuePair<string, string>("gps_offset_latitude", ""),
                new KeyValuePair<string, string>("traveldir", TravelDirSelected),
                new KeyValuePair<string, string>("installed2", InstalledAt),
                new KeyValuePair<string, string>("mfd2", Manufactured),

                new KeyValuePair<string, string>("pscode", entryPostalCode.Text),
                new KeyValuePair<string, string>("staddr", entryStreetAddr.Text),
                new KeyValuePair<string, string>("site2", entrySiteName.Text),
                new KeyValuePair<string, string>("udsowner", UDSOwner.Text),
                new KeyValuePair<string, string>("btype", buildingClassiKeySelected),
                new KeyValuePair<string, string>("elecsite", ElectSiteKeyCnt.ToString()),
                new KeyValuePair<string, string>("comm", Is3rdComms == 1 ? "1":"0"),
                new KeyValuePair<string, string>("commprovider", commsProvide.Text),
                new KeyValuePair<string, string>("key", ""),
                new KeyValuePair<string, string>("ktype", KeyType.ToString()),


                new KeyValuePair<string, string>("height2", entryHeight.Text),
                new KeyValuePair<string, string>("depth2", entryDepth.Text),
                new KeyValuePair<string, string>("width2", entryWidth.Text),
                new KeyValuePair<string, string>("CLEAR_ZONE_IND2", IsSiteClearZone  == 1 ? "1":"0"),
                new KeyValuePair<string, string>("etc2", ""),
                new KeyValuePair<string, string>("fosc2", ""),
                new KeyValuePair<string, string>("vault2", ""),
                new KeyValuePair<string, string>("trlane2", ""),

                new KeyValuePair<string, string>("orientation", ""),
                new KeyValuePair<string, string>("mod2", ""),
                new KeyValuePair<string, string>("pic2", ""),
                new KeyValuePair<string, string>("otag", ""),
                new KeyValuePair<string, string>("fltrsize2", FilterSizeKeySelected),
                
            };


            return keyValues;

        }


        /// selected item event 
        /*private void OnItemSelected(object sender, ItemSelectedEventArgs e)
        {
            if(buildingClass.SelectedIndex != -1)
                buildingClassiKeySelected = buildingClassiKeyList[buildingClass.SelectedIndex];
            if (pIntersection.SelectedIndex != -1)
                IntersectionSelected = IntersectionKeyList[pIntersection.SelectedIndex];
            if(pRoadway.SelectedIndex != -1)
                RoadwaySelected = roadWayKeyList[ pRoadway.SelectedIndex];
            if (pDirTravel.SelectedIndex != -1)
                DirTraval = dirTravelKeyList[pDirTravel.SelectedIndex];
            //OrientationSelected = OrientationList[pOrientation.SelectedIndex];
            if (pMaterial.SelectedIndex != -1)
                MaterialCodeKeySelected = materialCodeKeyList[pMaterial.SelectedIndex];
            if (pMounting.SelectedIndex != -1)
                MountingSelected = mountingKeyList[pMounting.SelectedIndex];
            if (pFilterType.SelectedIndex != -1)
                FilterTypeSelected = filterTypeKeyList[pFilterType.SelectedIndex];
            if (pRoadway.SelectedIndex != -1)
                FilterSizeKeySelected =  filterSizeKeyList[pFilterSize.SelectedIndex];

            if (pickerLaneClosure.SelectedIndex != -1)
                IsLaneClosure = pickerLaneClosure.SelectedIndex;
            if (pickerDotDisctrict.SelectedIndex != -1)
                DotDistrictCnt = pickerDotDisctrict.SelectedIndex;
            if (pickerHasPowerDisconnect.SelectedIndex != -1)
                IsHasPowerDisconnect = pickerHasPowerDisconnect.SelectedIndex;
            if (pDirectionTravel.SelectedIndex != -1)
                DirectionTravel = pDirectionTravel.SelectedIndex;
            if (pBucketTruck.SelectedIndex != -1)
                IsBucketTruck = pBucketTruck.SelectedIndex;
            if (pIsSiteClearZone.SelectedIndex != -1)
                IsSiteClearZone = pIsSiteClearZone.SelectedIndex;

            Console.WriteLine($"IsLaneClosure {IsLaneClosure}, DotDistrictCnt {DotDistrictCnt}, IsHasPowerDisconnect{IsHasPowerDisconnect}");
            Console.WriteLine($"DirectionTravel {DirectionTravel},IsBucketTruck {IsBucketTruck}, IsSiteClearZone{IsSiteClearZone}"  );

        }*/



        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            var KVPair = keyvaluepair();
            await CloudDBService.PostSaveBuilding(KVPair);
        }

        private void btnActive_Clicked(object sender, EventArgs e)
        {

        }

        private void btnRecRacks_Clicked(object sender, EventArgs e)
        {

        }

        private void btnTracer_Clicked(object sender, EventArgs e)
        {

        }

        private void btnFiber_Clicked(object sender, EventArgs e)
        {

        }

        private void btnRecDucts_Clicked(object sender, EventArgs e)
        {

        }



    }
}