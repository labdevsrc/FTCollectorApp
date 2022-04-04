﻿using FTCollectorApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using FTCollectorApp.ViewModel;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.View.Utils;
using FTCollectorApp.Service;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PullBoxSitePage : ContentPage
    {
        string MajorMinorType;
        string TagNumber;
        List<string> DotDistrict = new List<string>();
        List<string> YesNo = new List<string>();


        string Notes, SiteType;
        string InstalledAt, Manufactured;

        public PullBoxSitePage(string minorType, string tagNumber)
        {
            InitializeComponent();
            BindingContext = new DropDownViewModel();


            MajorMinorType = $"Pull Box - {minorType}";
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<CodeSiteType>();
                var CodeSiteTable = conn.Table<CodeSiteType>().ToList();
                SiteType = CodeSiteTable.Where(a => a.MajorType == "Pull Box" && a.MinorType == minorType).Select(g => g.CodeKey).First().ToString();
            }

            for (int i = 0; i < 100; i++)
            {
                DotDistrict.Add(i.ToString());
            }

            YesNo.Add("N");
            YesNo.Add("Y");

            TagNumber = tagNumber;
            Session.tag_number = TagNumber;
            entryTagNum.Text = tagNumber;
            //pickerDotDisctrict.ItemsSource = DotDistrict;
            pickerElectSiteKey.ItemsSource = DotDistrict;
            pickerHasPowerDisconnect.ItemsSource = YesNo;
            picker3rdpComms.ItemsSource = YesNo;
            pickerLaneClosure.ItemsSource = YesNo;


            //pHaveSunShield.ItemsSource = YesNo;
            pHasGround.ItemsSource = YesNo;
            pHasKey.ItemsSource = YesNo;
            //pKeyType.ItemsSource = DotDistrict;
            //pDirectionTravel.ItemsSource = DotDistrict;
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

            //buildingClass.SelectedIndexChanged += OnItemSelectedIndexChange;
            pIntersection.SelectedIndexChanged += OnItemSelectedIndexChange;
            pRoadway.SelectedIndexChanged += OnItemSelectedIndexChange;
            pDirTravel.SelectedIndexChanged += OnItemSelectedIndexChange;

            pOrientation.SelectedIndexChanged += OnItemSelectedIndexChange;
            pMaterial.SelectedIndexChanged += OnItemSelectedIndexChange;
            //pMounting.SelectedIndexChanged += OnItemSelectedIndexChange;

            // Cabinet didn't have Filter type and Filter size dropdown
            //pFilterType.SelectedIndexChanged += OnItemSelectedIndexChange; 
            //pFilterSize.SelectedIndexChanged += OnItemSelectedIndexChange;
            pModel.SelectedIndexChanged += OnItemSelectedIndexChange;
            pManufacturer.SelectedIndexChanged += OnItemSelectedIndexChange;

            Notes = editorNotes.Text;

            //pHaveSunShield.SelectedIndexChanged += OnItemSelectedIndexChange;
            pHasGround.SelectedIndexChanged += OnItemSelectedIndexChange;
            pHasKey.SelectedIndexChanged += OnItemSelectedIndexChange;
            //pKeyType.SelectedIndexChanged += OnItemSelectedIndexChange;
            //pDirectionTravel.SelectedIndexChanged += OnItemSelectedIndexChange;
            pBucketTruck.SelectedIndexChanged += OnItemSelectedIndexChange;
            pIsSiteClearZone.SelectedIndexChanged += OnItemSelectedIndexChange;
            pickerLaneClosure.SelectedIndexChanged += OnItemSelectedIndexChange;
            //pickerDotDisctrict.SelectedIndexChanged += OnItemSelectedIndexChange;
            pickerHasPowerDisconnect.SelectedIndexChanged += OnItemSelectedIndexChange;
            pickerElectSiteKey.SelectedIndexChanged += OnItemSelectedIndexChange;
            picker3rdpComms.SelectedIndexChanged += OnItemSelectedIndexChange;

            //dateManufactured.DateSelected += OnDateSelected;
            //dateInstalled.DateSelected += OnDateSelected;
            InstalledAt = DateTime.Now.ToString("yyyy-MM-dd");
            Manufactured = DateTime.Now.ToString("yyyy-MM-dd");

            IsBusy = false;


        }

        int IsBucketTruck = 0;
        int DirectionTravel = 0, IsLaneClosure = 0, DotDistrictCnt = 0, IsHasPowerDisconnect = 0,
            IsHaveSunShield = 0, IsHasGround = 0, IsHasKey = 0, ElectSiteKeyCnt = 0, Is3rdComms = 0;
        int KeyType;



        int IsSiteClearZone;


        int KeyTypeSelected = 0;
        string buildingClassiKeySelected, IntersectionSelected, RoadwaySelected, TravelDirSelected, Orientation, MaterialCodeKeySelected;
        string MountingSelected, FilterTypeSelected, FilterSizeKeySelected, OrientationSelected;
        string ManufacturerKeySelected, ModelKeySelected;
        private void OnItemSelectedIndexChange(object sender, EventArgs e)
        {
            //IsHaveSunShield = pHaveSunShield.SelectedIndex == -1 ? 0 : pHaveSunShield.SelectedIndex;
            IsHasGround = pHasGround.SelectedIndex == -1 ? 0 : pHasGround.SelectedIndex;
            IsHasKey = pHasKey.SelectedIndex == -1 ? 0 : pHasKey.SelectedIndex;
            //KeyTypeSelected = pKeyType.SelectedIndex == -1 ? 0 : pKeyType.SelectedIndex;
            //KeyType = pKeyType.SelectedIndex == -1 ? 0 : pKeyType.SelectedIndex;
            // DirectionTravel = pDirectionTravel.SelectedIndex == -1 ? 0 : pDirectionTravel.SelectedIndex;
            IsBucketTruck = pBucketTruck.SelectedIndex == -1 ? 0 : pBucketTruck.SelectedIndex;
            IsSiteClearZone = pIsSiteClearZone.SelectedIndex == -1 ? 0 : pIsSiteClearZone.SelectedIndex;
            IsLaneClosure = pickerLaneClosure.SelectedIndex == -1 ? 0 : pickerLaneClosure.SelectedIndex;
            //DotDistrictCnt = pickerDotDisctrict.SelectedIndex == -1 ? 0 : pickerDotDisctrict.SelectedIndex;
            IsHasPowerDisconnect = pickerHasPowerDisconnect.SelectedIndex == -1 ? 0 : pickerHasPowerDisconnect.SelectedIndex;
            ElectSiteKeyCnt = pickerElectSiteKey.SelectedIndex == -1 ? 0 : pickerElectSiteKey.SelectedIndex;
            Is3rdComms = picker3rdpComms.SelectedIndex == -1 ? 0 : picker3rdpComms.SelectedIndex;
            IsSiteClearZone = pIsSiteClearZone.SelectedIndex == -1 ? 0 : pIsSiteClearZone.SelectedIndex;


            /*if (buildingClass.SelectedIndex != -1)
            {
                var selected = buildingClass.SelectedItem as BuildingType;
                buildingClassiKeySelected = selected.BuildingTypeKey;
            }
            if (pMounting.SelectedIndex != -1)
            {
                var selected = pMounting.SelectedItem as Mounting;
                MountingSelected = selected.MountingKey;  /// object reference not set to instance
            }*/

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

            if (pManufacturer.SelectedIndex != -1)
            {
                var selected = pManufacturer.SelectedItem as Manufacturer;
                ManufacturerKeySelected = selected.ManufKey;
            }

            if (pModel.SelectedIndex != -1)
            {
                var selected = pModel.SelectedItem as DevType;
                ModelKeySelected = selected.DevTypeKey;
            }

        }

        private void btnCamera(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CameraViewPage());
        }


        private void btnCamera2(object sender, EventArgs e)
        {

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


                new KeyValuePair<string, string>("mfr2", ""),  // manufacturer , for Cabinet, pull box
                new KeyValuePair<string, string>("mfd2", Manufactured),
                new KeyValuePair<string, string>("mod2", ModelKeySelected), /// model name, Building : x,  Cabinet/Pull Box : o
                new KeyValuePair<string, string>("pic2", ""),
                new KeyValuePair<string, string>("otag", ""),
                new KeyValuePair<string, string>("roadway", RoadwaySelected),
                new KeyValuePair<string, string>("pid", ""),
                new KeyValuePair<string, string>("loct", ""),
                new KeyValuePair<string, string>("staddr", entryStreetAddr.Text), // site_address
                new KeyValuePair<string, string>("pscode", ""),  //This Pull box page didn't have postcode patam

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
                new KeyValuePair<string, string>("serialno", ""),
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



        private async void OnClicked(object sender, EventArgs e)
        {
            var KVPair = keyvaluepair();
            var result = await CloudDBService.PostSaveBuilding(KVPair);
            if (result.Equals("OK"))
            {
                await DisplayAlert("Success", "Uploading Data Done", "OK");
            }
            else
            {
                await DisplayAlert("Warning", result, "OK");
            }
            btnRecDucts.IsEnabled = true;
            btnRecRacks.IsEnabled = true;

        }


        private void btnActive_Clicked(object sender, EventArgs e)
        {

        }

        private async void btnRecRacks_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RacksPage());
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

