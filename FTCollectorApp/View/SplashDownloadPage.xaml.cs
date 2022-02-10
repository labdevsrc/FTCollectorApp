using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Service;
using FTCollectorApp.Utils;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            CrossConnectivity.Current.ConnectivityChanged += OnConnectivityHandler;
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await DownloadTables();
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
                DisplayAlert("Warning", "No Internet Available. Turn it ON now then retry", "Close");
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
                var contentUser = await CloudDBService.GetEndUserFromAWSMySQLTable();
                var contentJob = await CloudDBService.GetJobFromAWSMySQLTable();
                var contentCodeSiteType = await CloudDBService.GetCodeSiteTypeFromAWSMySQLTable();
                var contentSite = await CloudDBService.GetSiteFromAWSMySQLTable();
                var contentCrewDefault = await CloudDBService.GetCrewDefaultFromAWSMySQLTable();


                var contentManuf = await CloudDBService.GetManufacturerTable(); //manufacturer_list 
                var contentJobSumittal = await CloudDBService.GetJobSubmittalTable(); //job_submittal
                var contentKeyType = await CloudDBService.GetKeyTypeTable(); // keytype
                var contentMaterialCode = await CloudDBService.GetMaterialCodeTable(); // material
                var contentMounting = await CloudDBService.GetMountingTable(); // mounting


                var contentRoadway = await CloudDBService.GetRoadway();  // roadway
                //var contentOwnRoadway = await CloudDBService.GetOwnerRoadway();  // owner_roadway, this will be joined to roadway
                var contentElectCircuit = await CloudDBService.GetElectricCircuit(); //intersection
                var contentIntersection = await CloudDBService.GetIntersection(); //electric
                var contentDirection = await CloudDBService.GetDirection(); //direction
                var contentDuctSize = await CloudDBService.GetDuctSize(); //dsize
                var contentDuctType = await CloudDBService.GetDuctType(); //ducttype
                var contentGroupType = await CloudDBService.GetGroupType(); //grouptype
                var contentDevType = await CloudDBService.GetDevType(); //devtype
                var contentRackNumber = await CloudDBService.GetRackNumber();
                var contentRackType = await CloudDBService.GetRackType(); //racktype
                var contentSheath = await CloudDBService.GetSheath(); // sheath
                var contentReelId = await CloudDBService.GetReelId(); // reelid
                var contentOrientation = await CloudDBService.GetOrientation();  // sbto
                var contentChassis = await CloudDBService.GetChassis();  // sbto
                var contentAFCable = await CloudDBService.GetAFCable();  // frcable
                var contentCabStructure = await CloudDBService.GetCableStructure(); //cable_structure

                //var contentSide = await CloudDBService.GetSide(); //side
                var contentTraceWareTag = await CloudDBService.GetTraceWareTag(); // tracewaretag
                var contentOwner    = await CloudDBService.GetOwners(); //owners
                var contentOwners   = await CloudDBService.GetConduits(); // conduits
                var contentAllCountry = await CloudDBService.GetAllCountry(); //allcountry
                var contentInstallType = await CloudDBService.GetInstallType(); // installtype
                var contentDuctUsed = await CloudDBService.GetDuctUsed(); // ductused

                var contentDimension = await CloudDBService.GetDimensions();   // dimesnsions
                var contentFilterSize = await CloudDBService.GetFilterSize(); //fltrsizes
                var contentFilterType = await CloudDBService.GetFilterType(); //fltrsizes
                var contentSpliceType = await CloudDBService.GetSpliceType();//splicetype
                var contentLaborClass  = await CloudDBService.GetLaborClass();// laborclass


                var contentTravellen = await CloudDBService.GetCompassDir(); // travellen
                var contentBuildingType = await CloudDBService.GetBuildingType(); //bClassification


                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<User>();
                    conn.DeleteAll<User>();
                    conn.InsertAll(contentUser);

                    conn.CreateTable<Job>();
                    conn.DeleteAll<Job>();
                    conn.InsertAll(contentJob);

                    conn.CreateTable<CodeSiteType>();
                    conn.DeleteAll<CodeSiteType>();
                    conn.InsertAll(contentCodeSiteType);

                    conn.CreateTable<Site>();
                    conn.DeleteAll<Site>();
                    conn.InsertAll(contentSite);

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

                    //conn.CreateTable<OwnerRoadway>();
                    //conn.DeleteAll<OwnerRoadway>();
                    //conn.InsertAll(contentOwnRoadway);



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

                    conn.CreateTable<RackNumber>();
                    conn.InsertAll(contentRackNumber);

                    conn.CreateTable<RackType>();
                    conn.InsertAll(contentRackType);

                    conn.CreateTable<Sheath>();
                    conn.InsertAll(contentSheath);

                    conn.CreateTable<ReelId>();
                    conn.InsertAll(contentReelId);

                    conn.CreateTable<Chassis>();
                    conn.DeleteAll<Chassis>();
                    conn.InsertAll(contentChassis);

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
                    conn.InsertAll(contentLaborClass);

                    conn.CreateTable<CompassDirection>();
                    conn.DeleteAll<CompassDirection>();
                    conn.InsertAll(contentTravellen);

                    conn.CreateTable<BuildingType>();
                    conn.DeleteAll<BuildingType>();
                    conn.InsertAll(contentBuildingType);
                }
            }
            catch(Exception e)
            {
                await DisplayAlert("Warning", "Error during download database","RETRY");
                Console.WriteLine(e.ToString());

            }



            txtLoading.Text = "SQLite Dumping...";


            IsBusy = false;
        }


        private void LoginClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
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
            Navigation.PushAsync(new PendingSendPage());
            
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {

        }
    }
}