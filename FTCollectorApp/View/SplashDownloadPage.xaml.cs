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
                DisplayAlert("Warning", "No Internet Available. Turn it now then retry", "Close");
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
                var contentCodeSiteType = await CloudDBService.GetEndUserFromAWSMySQLTable();
                var contentSite = await CloudDBService.GetJobFromAWSMySQLTable();
                var contentCrewDefault = await CloudDBService.GetCrewDefaultFromAWSMySQLTable();
                var contentManuf = await CloudDBService.GetManufacturerTable();
                var contentJobSumittal = await CloudDBService.GetJobSubmittalTable();
                var contentKeyType = await CloudDBService.GetKeyTypeTable();
                var contentMaterialCode = await CloudDBService.GetMaterialCodeTable();
                var contentMounting = await CloudDBService.GetMountingTable();


                var contentRoadway = await CloudDBService.GetRoadway();
                var contentOwnRoadway = await CloudDBService.GetOwnerRoadway();
                var contentElectCircuit = await CloudDBService.GetElectricCircuit();
                var contentIntersection = await CloudDBService.GetIntersection();
                var contentDirection = await CloudDBService.GetDirection();
                var contentDuctSize = await CloudDBService.GetDuctSize();
                var contentDuctType = await CloudDBService.GetDuctType();
                var contentGroupType = await CloudDBService.GetGroupType();
                var contentDevType = await CloudDBService.GetDevType();
                var contentRackNumber = await CloudDBService.GetRackNumber();
                var contentRackType = await CloudDBService.GetRackType();
                var contentSheath = await CloudDBService.GetSheath();
                var contentReelId = await CloudDBService.GetReelId();
                var contentOrientation = await CloudDBService.GetOrientation();
                var contentDimension = await CloudDBService.GetDimensions();
                var contentFilterSize = await CloudDBService.GetFilterSize();
                var contentSpliceType = await CloudDBService.GetSpliceType();
                var contentLaborClass  = await CloudDBService.GetLaborClass();
                var contentCabStructure = await CloudDBService.GetCableStructure();
                var contentTravellen = await CloudDBService.GetCompassDir();
                var contentBuildingType = await CloudDBService.GetBuildingType();


                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<User>();
                    conn.InsertAll(contentUser);

                    conn.CreateTable<Job>();
                    conn.InsertAll(contentJob);

                    conn.CreateTable<CodeSiteType>();
                    conn.InsertAll(contentCodeSiteType);

                    conn.CreateTable<Site>();
                    conn.InsertAll(contentSite);

                    conn.CreateTable<Crewdefault>();
                    conn.InsertAll(contentCrewDefault);

                    conn.CreateTable<Manufacturer>();
                    conn.InsertAll(contentManuf);

                    conn.CreateTable<JobSubmittal>();
                    conn.InsertAll(contentJobSumittal);

                    conn.CreateTable<KeyType>();
                    conn.InsertAll(contentKeyType);

                    conn.CreateTable<MaterialCode>();
                    conn.InsertAll(contentMaterialCode);

                    conn.CreateTable<Mounting>();
                    conn.InsertAll(contentMounting);

                    conn.CreateTable<Roadway>();
                    conn.InsertAll(contentRoadway);

                    conn.CreateTable<OwnerRoadway>();
                    conn.InsertAll(contentOwnRoadway);



                    conn.CreateTable<InterSectionRoad>();
                    conn.InsertAll(contentIntersection);


                    conn.CreateTable<ElectricCircuit>();
                    conn.InsertAll(contentElectCircuit);

                    conn.CreateTable<Direction>();
                    conn.InsertAll(contentDirection);

                    conn.CreateTable<DuctSize>();
                    conn.InsertAll(contentDuctSize);

                    conn.CreateTable<DuctType>();
                    conn.InsertAll(contentDuctType);

                    conn.CreateTable<GroupType>();
                    conn.InsertAll(contentGroupType);


                    conn.CreateTable<DevType>();
                    conn.InsertAll(contentDevType);

                    conn.CreateTable<RackNumber>();
                    conn.InsertAll(contentRackNumber);

                    conn.CreateTable<RackType>();
                    conn.InsertAll(contentRackType);

                    conn.CreateTable<Sheath>();
                    conn.InsertAll(contentSheath);

                    conn.CreateTable<ReelId>();
                    conn.InsertAll(contentReelId);

                    conn.CreateTable<Dimensions>();
                    conn.InsertAll(contentDimension);

                    conn.CreateTable<Orientation>();
                    conn.InsertAll(contentOrientation);

                    conn.CreateTable<FilterSize>();
                    conn.InsertAll(contentFilterSize);

                    conn.CreateTable<SpliceType>();
                    conn.InsertAll(contentSpliceType);

                    conn.CreateTable<LaborClass>();
                    conn.InsertAll(contentLaborClass);

                    conn.CreateTable<CompassDirection>();
                    conn.InsertAll(contentTravellen);

                    conn.CreateTable<BuildingType>();
                    conn.InsertAll(contentBuildingType);
                }
            }
            catch(Exception e)
            {
                DisplayAlert("Warning", "Error during download database","RETRY");
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