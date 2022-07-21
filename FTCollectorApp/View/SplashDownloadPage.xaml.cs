using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Service;
using FTCollectorApp.Utils;
using FTCollectorApp.View.SyncPages;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using Acr.UserDialogs;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashDownloadPage : ContentPage
    {


        public SplashDownloadPage()
        {
            InitializeComponent();
            BindingContext = this;
            Session.Result = "InitializingDownload";

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            CrossConnectivity.Current.ConnectivityChanged += OnConnectivityHandler;
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (!IsBusy)  // blocks overlap display allert when accidentally, user move to other application
                {
                    bool answer = await DisplayAlert("Welcome", "Press DOWNLOAD for downloading require tables. Or choose LOGIN for LOGIN directly ", "DOWNLOAD", "LOGIN");
                    if (answer)
                        await DownloadTables();
                    else
                        Navigation.PushAsync(new LoginPage());
                        //Navigation.PushAsync(new MainPage());
                }
            }
            else
            {
                /*UserDialogs.Instance.Confirm(new ConfirmConfig
                {
                    Title = "Warning",
                    Message = "No Internet Available. Turn it ON now then retry",
                    OkText = "Retry",
                    CancelText = "Close",
                    OnAction = async (confirmed) =>
                    {
                        if (confirmed)
                            //UserDialogs.Instance.Alert(Title = "Close");
                            await DownloadTables();
                    }
                });*/
                await DisplayAlert("Warning", "No Internet Available. Turn it ON now then retry", "Close");
            }

        }

        private async void OnConnectivityHandler(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await DownloadTables();
            }
        }

        async Task DownloadTables()
        {
            IsBusy = true;
            txtLoading.Text = "Downloading...";
            try
            {
                txtLoading.Text = "Downloading end_user table";
                var contentUser = await CloudDBService.GetEndUserFromAWSMySQLTable();
                txtLoading.Text = "Downloading Job table";
                var contentJob = await CloudDBService.GetJobFromAWSMySQLTable();
                var contentCodeSiteType = await CloudDBService.GetCodeSiteTypeFromAWSMySQLTable();
                txtLoading.Text = "Downloading Site table";
                //var contentSite = await CloudDBService.GetSite();
                //var contentSite = await CloudDBService.GetSiteFromAWSMySQLTable();

                var contentCrewDefault = await CloudDBService.GetCrewDefaultFromAWSMySQLTable();

                txtLoading.Text = "Downloading manufacturer table";
                var contentManuf = await CloudDBService.GetManufacturerTable(); //manufacturer_list 
                var contentJobSumittal = await CloudDBService.GetJobSubmittalTable(); //job_submittal
                var contentKeyType = await CloudDBService.GetKeyTypeTable(); // keytype
                var equipmentType = await CloudDBService.GetEquipmentType(); //code_equipment_type
                var equipmentDetail = await CloudDBService.GetEquipmentDetail(); //equipment


                var unitOfmeasure = await CloudDBService.GetUOM(); //unit_of_measurement
                txtLoading.Text = "Downloading code_material table";
                var contentMaterialCode = await CloudDBService.GetMaterialCodeTable(); // material
                var contentMounting = await CloudDBService.GetMountingTable(); // mounting

                txtLoading.Text = "Downloading roadway table";
                var contentRoadway = await CloudDBService.GetRoadway();  // roadway
                //var contentOwnRoadway = await CloudDBService.GetOwnerRoadway();  // owner_roadway, this will be joined to roadway
                var contentElectCircuit = await CloudDBService.GetElectricCircuit(); //intersection
                txtLoading.Text = "Downloading intersection table";
                var contentIntersection = await CloudDBService.GetIntersection(); //electric

                var contentDirection = await CloudDBService.GetDirection(); //direction
                txtLoading.Text = "Downloading code_duct_size table";
                var contentDuctSize = await CloudDBService.GetDuctSize(); //dsize
                var contentDuctType = await CloudDBService.GetDuctType(); //ducttype
                var contentGroupType = await CloudDBService.GetGroupType(); //grouptype


                var contentDevType = await CloudDBService.GetDevType(); //devtype
                var contentModelDetail = await CloudDBService.GetModelDetail(); //model
                var contentRackNumber = await CloudDBService.GetRackNumber();
                var contentRackType = await CloudDBService.GetRackType(); //racktype
                txtLoading.Text = "Downloading code_fiber_sheath_type table";
                var contentSheath = await CloudDBService.GetSheath(); // sheath
                var contentReelId = await CloudDBService.GetReelId(); // reelid
                var contentOrientation = await CloudDBService.GetOrientation();  // sbto
                var contentChassis = await CloudDBService.GetChassis();  // sbto

                txtLoading.Text = "Downloading a_fiber_cable table";
                var contentAFCable = await CloudDBService.GetAFCable();  // frcable
                txtLoading.Text = "Downloading a_fiber_reel table";
                var contentCabStructure = await CloudDBService.GetCableStructure(); //cable_structure

                //var contentSide = await CloudDBService.GetSide(); //side
                var contentTraceWareTag = await CloudDBService.GetTraceWareTag(); // tracewaretag
                txtLoading.Text = "Downloading owner table";
                var contentOwner    = await CloudDBService.GetOwners(); //owners
                txtLoading.Text = "Downloading owner conduits";
                var contentConduit   = await CloudDBService.GetConduits(); // conduits

                var ductInstallType = await CloudDBService.GetDuctInstallType(); // installtype
                var fiberInstallType = await CloudDBService.GetFiberInstallType(); // installtype
                var contentDuctUsed = await CloudDBService.GetDuctUsed(); // ductused

                var contentDimension = await CloudDBService.GetDimensions();   // dimesnsions
                var contentFilterSize = await CloudDBService.GetFilterSize(); //fltrsizes
                txtLoading.Text = "Downloading  code_filter_type table";
                var contentFilterType = await CloudDBService.GetFilterType(); //fltrsizes
                var contentSpliceType = await CloudDBService.GetSpliceType();//splicetype
                var contentLaborClass  = await CloudDBService.GetLaborClass();// laborclass


                var contentTravellen = await CloudDBService.GetCompassDir(); // travellen
                txtLoading.Text = "Downloading code_site_type table";
                var contentBuildingType = await CloudDBService.GetBuildingType(); //bClassification
                var contentCableType = await CloudDBService.GetCableType(); //code_cable_type

                var codeDuctInstallType = await CloudDBService.GetDuctInstallTypes(); //code_duct_installation

                var codeColor = await CloudDBService.GetColorCode(); //code_colors

                var contentChassisType = await CloudDBService.GetChassisTypes(); //code_colors
                var contentslotBladeTray = await CloudDBService.GetBladeTableKey(); //slotbladetray

                var portType = await CloudDBService.GetCodePortType(); //code_port_type
                var portTable = await CloudDBService.GetPortTable(); //port table
                var codeLocatePoint = await CloudDBService.GetLocatePoint(); //code_locate_point

                var max_gps_point = await CloudDBService.GetMaxGpsPoint(); //gps_point

                var suspList = await CloudDBService.GetSuspendedTrace(); //gps_point

                txtLoading.Text = "Site...";
                var contentSite = await CloudDBService.GetSiteFromAWSMySQLTable();
                //Thread.Sleep(5000);
                txtLoading.Text = "Download done! Populating SQLite...";
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Site>();
                    conn.DeleteAll<Site>();
                    conn.InsertAll(contentSite);

                    txtLoading.Text = "code tables ...";

                    conn.CreateTable<SuspendedTrace>();
                    conn.DeleteAll<SuspendedTrace>();
                    conn.InsertAll(suspList);

                    conn.CreateTable<GpsPoint>();
                    conn.DeleteAll<GpsPoint>();
                    conn.InsertAll(max_gps_point);


                    conn.CreateTable<CodeLocatePoint>();
                    conn.DeleteAll<CodeLocatePoint>();
                    conn.InsertAll(codeLocatePoint);

                    conn.CreateTable<PortType>();
                    conn.DeleteAll<PortType>();
                    conn.InsertAll(portType);

                    conn.CreateTable<Ports>();
                    conn.DeleteAll<Ports>();
                    conn.InsertAll(portTable);

                    conn.CreateTable<DuctInstallType>();
                    conn.DeleteAll<DuctInstallType>();
                    conn.InsertAll(ductInstallType);


                    conn.CreateTable<DuctInstallType>();
                    conn.DeleteAll<DuctInstallType>();
                    conn.InsertAll(ductInstallType);

                    conn.CreateTable<FiberInstallType>();
                    conn.DeleteAll<FiberInstallType>();
                    conn.InsertAll(fiberInstallType);

                    conn.CreateTable<User>();
                    conn.DeleteAll<User>();
                    conn.InsertAll(contentUser);

                    conn.CreateTable<Job>();
                    conn.DeleteAll<Job>();
                    conn.InsertAll(contentJob);

                    conn.CreateTable<CodeSiteType>();
                    conn.DeleteAll<CodeSiteType>();
                    conn.InsertAll(contentCodeSiteType);

                    conn.CreateTable<Crewdefault>();
                    conn.DeleteAll<Crewdefault>();
                    conn.InsertAll(contentCrewDefault);

                    conn.CreateTable<Manufacturer>();
                    conn.DeleteAll<Manufacturer>();
                    conn.InsertAll(contentManuf);

                    conn.CreateTable<JobSubmittal>();
                    conn.DeleteAll<JobSubmittal>();
                    conn.InsertAll(contentJobSumittal);

                    conn.CreateTable<KeyType>();
                    conn.DeleteAll<KeyType>();
                    conn.InsertAll(contentKeyType);

                    conn.CreateTable<MaterialCode>();
                    conn.DeleteAll<MaterialCode>();
                    conn.InsertAll(contentMaterialCode);

                    conn.CreateTable<Mounting>();
                    conn.DeleteAll<Mounting>();
                    conn.InsertAll(contentMounting);

                    conn.CreateTable<Roadway>();
                    conn.DeleteAll<Roadway>();
                    conn.InsertAll(contentRoadway);


                    conn.CreateTable<InterSectionRoad>();
                    conn.DeleteAll<InterSectionRoad>();
                    conn.InsertAll(contentIntersection);


                    conn.CreateTable<ElectricCircuit>();
                    conn.DeleteAll<ElectricCircuit>();
                    conn.InsertAll(contentElectCircuit);

                    conn.CreateTable<Direction>();
                    conn.DeleteAll<Direction>();
                    conn.InsertAll(contentDirection);

                    conn.CreateTable<DuctSize>();
                    conn.DeleteAll<DuctSize>();
                    conn.InsertAll(contentDuctSize);

                    conn.CreateTable<DuctType>();
                    conn.DeleteAll<DuctType>();
                    conn.InsertAll(contentDuctType);

                    conn.CreateTable<GroupType>();
                    conn.DeleteAll<GroupType>();
                    conn.InsertAll(contentGroupType);


                    conn.CreateTable<DevType>();
                    conn.DeleteAll<DevType>();
                    conn.InsertAll(contentDevType);

                    conn.CreateTable<ModelDetail>();
                    conn.DeleteAll<ModelDetail>();
                    conn.InsertAll(contentModelDetail);

                    conn.CreateTable<RackNumber>();
                    conn.DeleteAll<RackNumber>();
                    conn.InsertAll(contentRackNumber);

                    conn.CreateTable<RackType>();
                    conn.DeleteAll<RackType>();
                    conn.InsertAll(contentRackType);

                    conn.CreateTable<Sheath>();
                    conn.DeleteAll<Sheath>();
                    conn.InsertAll(contentSheath);

                    conn.CreateTable<ReelId>();
                    conn.DeleteAll<ReelId>();
                    conn.InsertAll(contentReelId);

                    conn.CreateTable<Chassis>();
                    conn.DeleteAll<Chassis>();
                    conn.InsertAll(contentChassis);

                    conn.CreateTable<ChassisType>();
                    conn.DeleteAll<ChassisType>();
                    conn.InsertAll(contentChassisType);

                    conn.CreateTable<CableStructure>();
                    conn.DeleteAll<CableStructure>();
                    conn.InsertAll(contentCabStructure);

                    conn.CreateTable<Dimensions>();
                    conn.DeleteAll<Dimensions>();
                    conn.InsertAll(contentDimension);

                    conn.CreateTable<Orientation>();
                    conn.DeleteAll<Orientation>();
                    conn.InsertAll(contentOrientation);

                    conn.CreateTable<FilterSize>();
                    conn.DeleteAll<FilterSize>();
                    conn.InsertAll(contentFilterSize);

                    conn.CreateTable<FilterType>();
                    conn.DeleteAll<FilterType>();
                    conn.InsertAll(contentFilterType);

                    conn.CreateTable<SpliceType>();
                    conn.DeleteAll<SpliceType>();
                    conn.InsertAll(contentSpliceType);

                    conn.CreateTable<LaborClass>();
                    conn.DeleteAll<LaborClass>();
                    conn.InsertAll(contentLaborClass);

                    conn.CreateTable<CompassDirection>();
                    conn.DeleteAll<CompassDirection>();
                    conn.InsertAll(contentTravellen);

                    conn.CreateTable<BuildingType>();
                    conn.DeleteAll<BuildingType>();
                    conn.InsertAll(contentBuildingType);

                    conn.CreateTable<AFiberCable>();
                    conn.DeleteAll<AFiberCable>();
                    conn.InsertAll(contentAFCable);

                    conn.CreateTable<CableType>();
                    conn.DeleteAll<CableType>();
                    conn.InsertAll(contentCableType);

                    conn.CreateTable<EquipmentType>();
                    conn.DeleteAll<EquipmentType>();
                    conn.InsertAll(equipmentType);

                    conn.CreateTable<EquipmentDetailType>();
                    conn.DeleteAll<EquipmentDetailType>();
                    conn.InsertAll(equipmentDetail);

                    conn.CreateTable<UnitOfMeasure>();
                    conn.DeleteAll<UnitOfMeasure>();
                    conn.InsertAll(unitOfmeasure);

                    conn.CreateTable<DuctInstallType>();
                    conn.DeleteAll<DuctInstallType>();
                    conn.InsertAll(codeDuctInstallType);



                    conn.CreateTable<ConduitsGroup>();
                    conn.DeleteAll<ConduitsGroup>();
                    conn.InsertAll(contentConduit);


                    conn.CreateTable<ColorCode>();
                    conn.DeleteAll<ColorCode>();
                    conn.InsertAll(codeColor);

                    conn.CreateTable<SlotBladeTray>();
                    conn.DeleteAll<SlotBladeTray>();
                    conn.InsertAll(contentslotBladeTray);

                }
                txtLoading.Text = "Populating Local SQLite done!";
            }
            catch(Exception e)
            {
                bool answer = await DisplayAlert("Warning", "Error during download database","RETRY", "CLOSE");
                if (answer)
                    DownloadTables();

                Console.WriteLine(e.ToString());

            }



            txtLoading.Text = "SQLite Dumping...";


            IsBusy = false;
        }


        private void LoginClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());
            //Navigation.PushAsync(new MainPage());
        }

        private void RetryClicked(object sender, EventArgs e)
        {
            DownloadTables();
        }

        private void PendingUploadClicked(object sender, EventArgs e)
        {
            // close, exit apps
            // var closer = DependencyService.Get<ICloseApps>();
            // closer?.closeApplication();
            //Navigation.PushAsync(new PendingSendPage());
            Navigation.PushAsync(new SyncPage());

        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {

        }
    }
}