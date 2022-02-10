using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Service;
using FTCollectorApp.View.Utils;
using FTCollectorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CabinetSitePage : ContentPage
    {
        string MajorMinorType;
        string TagNumber;
        List<string> DotDistrict = new List<string>();
        List<string> YesNo = new List<string>();


        string Notes;
        string InstalledAt, Manufactured;

        public CabinetSitePage(string minorType, string tagNumber)
        {
            InitializeComponent();
            BindingContext = new BdSitePageViewModel();


            MajorMinorType = $"Cabinet - {minorType}";

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
            pDirectionTravel.ItemsSource = DotDistrict;
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

            buildingClass.SelectedIndexChanged += OnItemSelectedIndexChange;
            pIntersection.SelectedIndexChanged += OnItemSelectedIndexChange;
            pRoadway.SelectedIndexChanged += OnItemSelectedIndexChange;
            pDirTravel.SelectedIndexChanged += OnItemSelectedIndexChange;

            //pOrientation.SelectedIndexChanged += OnItemSelectedIndexChange;
            pMaterial.SelectedIndexChanged += OnItemSelectedIndexChange;
            pMounting.SelectedIndexChanged += OnItemSelectedIndexChange;
            pModel.SelectedIndexChanged += OnItemSelectedIndexChange;
            pManufacturer.SelectedIndexChanged += OnItemSelectedIndexChange;

            Notes = editorNotes.Text;

            pHaveSunShield.SelectedIndexChanged += OnItemSelectedIndexChange;
            pHasGround.SelectedIndexChanged += OnItemSelectedIndexChange;
            pHasKey.SelectedIndexChanged += OnItemSelectedIndexChange;
            pKeyType.SelectedIndexChanged += OnItemSelectedIndexChange;
            pDirectionTravel.SelectedIndexChanged += OnItemSelectedIndexChange;
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
            IsHaveSunShield = 0, IsHasGround = 0, IsHasKey = 0, ElectSiteKeyCnt = 0, Is3rdComms = 0;
        int KeyType;
        int IsSiteClearZone;
        string buildingClassiKeySelected, IntersectionSelected, RoadwaySelected, TravelDirSelected, Orientation, MaterialCodeKeySelected;
        string MountingSelected, FilterTypeSelected, FilterSizeKeySelected, OrientationSelected;
        string ManufacturerKeySelected, ModelKeySelected;
        private void OnItemSelectedIndexChange(object sender, EventArgs e)
        {
            IsHaveSunShield = pHaveSunShield.SelectedIndex == -1 ? 0 : pHaveSunShield.SelectedIndex;
            IsHasGround = pHasGround.SelectedIndex == -1 ? 0 : pHasGround.SelectedIndex;
            IsHasKey = pHasKey.SelectedIndex == -1 ? 0 : pHasKey.SelectedIndex;
            KeyType = pKeyType.SelectedIndex == -1 ? 0 : pKeyType.SelectedIndex;
            DirectionTravel = pDirectionTravel.SelectedIndex == -1 ? 0 : pDirectionTravel.SelectedIndex;
            IsBucketTruck = pBucketTruck.SelectedIndex == -1 ? 0 : pBucketTruck.SelectedIndex;
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

            if (pManufacturer.SelectedIndex != -1)
            {
                var selected = pManufacturer.SelectedItem as Manufacturer;
                ManufacturerKeySelected = selected.ManufKey;
            }

            if (pModel.SelectedIndex != -1)
            {
                var selected = pModel.SelectedItem as DevType;
                ModelKeySelected = selected.DevTypeDesc;
            }

        }

        private void btnCamera(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CameraViewPage());
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
                new KeyValuePair<string, string>("type1","1"),  // 1: Building 
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