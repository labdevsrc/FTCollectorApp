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


        string Notes, SiteType;
        string InstalledAt, Manufactured;

        public BuildingSitePage(string minorType, string tagNumber)
        {
            InitializeComponent();
            BindingContext = new DropDownViewModel();

            MajorMinorType = $"Building - {minorType}";
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<CodeSiteType>();
                var CodeSiteTable = conn.Table<CodeSiteType>().ToList();
                SiteType = CodeSiteTable.Where(a => a.MajorType == "Building" && a.MinorType == minorType).Select(g=>g.CodeKey).First().ToString();
            }

            for (int i = 0; i < 100; i++)
            {
                DotDistrict.Add(i.ToString());
            }

            YesNo.Add("N");
            YesNo.Add("Y");

            TagNumber = tagNumber;
            entryTagNum.Text = tagNumber;
            pickerLaneClosure.ItemsSource = YesNo;
            pickerDotDisctrict.ItemsSource = DotDistrict;
            pickerHasPowerDisconnect.ItemsSource = YesNo;
            pickerElectSiteKey.ItemsSource = DotDistrict;
            picker3rdpComms.ItemsSource = YesNo;

            pRackCount.ItemsSource = DotDistrict;
            pKeyCode.ItemsSource = DotDistrict;

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

            //IsBusy = true;
            entrySiteType.Text = MajorMinorType;
            entryTagNum.Text = TagNumber;
            ownerName.Text = Session.OwnerName;

            //buildingClass.ItemsSource = BuildingTypeList;
            buildingClass.SelectedIndexChanged += OnItemSelectedIndexChange;

            //pIntersection.ItemsSource = IntersectionList;
            pIntersection.SelectedIndexChanged += OnItemSelectedIndexChange;

            //pRoadway.ItemsSource = RoadwayList;
            pRoadway.SelectedIndexChanged += OnItemSelectedIndexChange;
            pDirTravel.SelectedIndexChanged += OnItemSelectedIndexChange;
            pOrientation.SelectedIndexChanged += OnItemSelectedIndexChange;
            pMaterial.SelectedIndexChanged += OnItemSelectedIndexChange;
            pMounting.SelectedIndexChanged += OnItemSelectedIndexChange;


            pFilterType.SelectedIndexChanged += OnItemSelectedIndexChange;
            pFilterSize.SelectedIndexChanged += OnItemSelectedIndexChange;
            
            // Building didn't have Model & Manufacturer
            //pModel.SelectedIndexChanged += OnItemSelectedIndexChange;
            //pManufacturer.SelectedIndexChanged += OnItemSelectedIndexChange;

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
            pRackCount.SelectedIndexChanged += OnItemSelectedIndexChange;
            pKeyCode.SelectedIndexChanged += OnItemSelectedIndexChange;


            dateManufactured.DateSelected += OnDateSelected;
            dateInstalled.DateSelected += OnDateSelected;
            //IsBusy = false;
            InstalledAt = DateTime.Now.ToString("yyyy-MM-dd");
            Manufactured = DateTime.Now.ToString("yyyy-MM-dd");
        }

        int IsBucketTruck = 0;
        int DirectionTravel = 0, IsLaneClosure = 0, DotDistrictCnt = 0, IsHasPowerDisconnect = 0, 
            IsHaveSunShield = 0, IsHasGround = 0, IsHasKey =0, ElectSiteKeyCnt = 0, Is3rdComms = 0;
        int KeyTypeSelected = 0;
        int IsSiteClearZone, RackCountSelected =0 , KeyCodeSelected =0;
        string buildingClassiKeySelected, IntersectionSelected, RoadwaySelected, TravelDirSelected, MaterialCodeKeySelected;
        string MountingSelected, FilterTypeSelected, FilterSizeKeySelected, OrientationSelected;

        private void OnItemSelectedIndexChange(object sender, EventArgs e)
        {
            IsHaveSunShield =  pHaveSunShield.SelectedIndex == -1 ? 0 : pHaveSunShield.SelectedIndex;
            IsHasGround = pHasGround.SelectedIndex == -1 ? 0 : pHasGround.SelectedIndex;
            IsHasKey = pHasKey.SelectedIndex == -1 ? 0 : pHasKey.SelectedIndex;
            KeyTypeSelected = pKeyType.SelectedIndex == -1 ? 0 : pKeyType.SelectedIndex;
            //DirectionTravel = pDirectionTravel.SelectedIndex == -1 ? 0 : pDirectionTravel.SelectedIndex;
            IsBucketTruck =    pBucketTruck.SelectedIndex == -1 ? 0 : pBucketTruck.SelectedIndex;
            IsSiteClearZone = pIsSiteClearZone.SelectedIndex == -1 ? 0 : pIsSiteClearZone.SelectedIndex;
            IsLaneClosure = pickerLaneClosure.SelectedIndex == -1 ? 0 : pickerLaneClosure.SelectedIndex;
            DotDistrictCnt = pickerDotDisctrict.SelectedIndex == -1 ? 0 : pickerDotDisctrict.SelectedIndex;
            IsHasPowerDisconnect = pickerHasPowerDisconnect.SelectedIndex == -1 ? 0 : pickerHasPowerDisconnect.SelectedIndex;
            ElectSiteKeyCnt = pickerElectSiteKey.SelectedIndex == -1 ? 0 : pickerElectSiteKey.SelectedIndex;
            Is3rdComms = picker3rdpComms.SelectedIndex == -1 ? 0 : picker3rdpComms.SelectedIndex;
            RackCountSelected = pRackCount.SelectedIndex == -1 ? 0 : pRackCount.SelectedIndex;
            KeyCodeSelected = pKeyCode.SelectedIndex == -1 ? 0 : pKeyCode.SelectedIndex;

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
                OrientationSelected = selected.OrientationHV;
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
                FilterTypeSelected = selected.FilterTypeKey;
            }
        }

        private void btnCamera(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CameraViewPage());
        }


        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            InstalledAt = dateInstalled.Date.ToString("yyyy-MM-dd");
            Manufactured = dateManufactured.Date.ToString("yyyy-MM-dd");
        }


        List<KeyValuePair<string, string>> keyvaluepair()
        {


            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),  // 2
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // 2
                new KeyValuePair<string, string>("accuracy", Session.accuracy), //3
                new KeyValuePair<string, string>("altitude", Session.altitude),  //4
                new KeyValuePair<string, string>("oid", Session.ownerkey), //1
                //new KeyValuePair<string, string>("owner", Session.ownerkey), //5
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 6
                new KeyValuePair<string, string>("jobnum",Session.jobnum), //  7 

                new KeyValuePair<string, string>("tag",TagNumber), //8
                new KeyValuePair<string, string>("site2", entrySiteName.Text),  /// site_id
                new KeyValuePair<string, string>("type2", SiteType),  /// code_site_type.key
                new KeyValuePair<string, string>("sitname2", entrySiteName.Text),


                new KeyValuePair<string, string>("manufacturer", ""),  // manufacturer , for Cabinet, pull box
                new KeyValuePair<string, string>("manufactured_date", Manufactured),
                new KeyValuePair<string, string>("model", ""), /// model name, Building : x,  Cabinet/Pull Box : o
                new KeyValuePair<string, string>("pic2", ""),
                new KeyValuePair<string, string>("otag", ""),
                new KeyValuePair<string, string>("roadway", RoadwaySelected),
                new KeyValuePair<string, string>("pid", ""),
                new KeyValuePair<string, string>("loct", ""),
                new KeyValuePair<string, string>("staddr", entryStreetAddr.Text), // site_address
                new KeyValuePair<string, string>("pscode", entryPostalCode.Text),

                new KeyValuePair<string, string>("btype", buildingClassiKeySelected),
                new KeyValuePair<string, string>("orientation", OrientationSelected),

                new KeyValuePair<string, string>("laneclosure", IsLaneClosure == 1 ? "1":"0"),
                new KeyValuePair<string, string>("dotdis",  DotDistrictCnt.ToString()),
                new KeyValuePair<string, string>("powr", IsHasPowerDisconnect == 1 ? "1":"0"),
                new KeyValuePair<string, string>("elecsite", ElectSiteKeyCnt.ToString()),
                new KeyValuePair<string, string>("comm", Is3rdComms == 1 ? "1":"0"),
                new KeyValuePair<string, string>("commprovider", commsProvide.Text),
                new KeyValuePair<string, string>("sitaddr", entryStreetAddr.Text), // site_street_addres
                new KeyValuePair<string, string>("udsowner", ""),

                new KeyValuePair<string, string>("rs2", "L"),

                new KeyValuePair<string, string>("height2", entryHeight.Text),
                new KeyValuePair<string, string>("depth2", entryDepth.Text),
                new KeyValuePair<string, string>("width2", entryWidth.Text),
                new KeyValuePair<string, string>("CLEAR_ZONE_IND2", IsSiteClearZone  == 1 ? "1":"0"),

                new KeyValuePair<string, string>("intersect2", IntersectionSelected),
                new KeyValuePair<string, string>("material2", MaterialCodeKeySelected),
                new KeyValuePair<string, string>("mounting2", MountingSelected),
                new KeyValuePair<string, string>("offilter2", FilterTypeSelected),
                new KeyValuePair<string, string>("fltrsize2", FilterSizeKeySelected),
                new KeyValuePair<string, string>("sunshield2", IsHaveSunShield == 1 ? "1":"0"),
                new KeyValuePair<string, string>("installed2", InstalledAt),
                new KeyValuePair<string, string>("comment2", Notes), // Notes, pr description

                new KeyValuePair<string, string>("etc2", ""),
                new KeyValuePair<string, string>("fosc2", ""),
                new KeyValuePair<string, string>("vault2", ""),
                new KeyValuePair<string, string>("trlane2", ""),
                new KeyValuePair<string, string>("bucket2", IsBucketTruck == 1 ? "1":"0"),
                new KeyValuePair<string, string>("serialno", entrySerial.Text),
                new KeyValuePair<string, string>("key", ""),
                new KeyValuePair<string, string>("ktype", KeyTypeSelected.ToString()),
                new KeyValuePair<string, string>("ground", IsHasGround == 1 ? "1":"0"),
                new KeyValuePair<string, string>("traveldir", TravelDirSelected),
                new KeyValuePair<string, string>("owner_key", Session.ownerkey),
                new KeyValuePair<string, string>("owner_county", Session.countycode),
                new KeyValuePair<string, string>("jobkey", Session.jobkey),

                new KeyValuePair<string, string>("gps_offset_latitude", ""),
                new KeyValuePair<string, string>("gps_offset_longitude", ""),
                new KeyValuePair<string, string>("LATITUDE", Session.lattitude2),
                new KeyValuePair<string, string>("LONGITUDE", Session.longitude2),


                new KeyValuePair<string, string>("plansheet","0"),
                new KeyValuePair<string, string>("psitem","0"),
                new KeyValuePair<string, string>("stage", Session.stage),
            };


            return keyValues;

        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            var KVPair = keyvaluepair();
            await CloudDBService.PostSaveBuilding(KVPair);
            btnRecDucts.IsEnabled = true;
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

        private async void btnRecDucts_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DuctPage());
        }



    }
}