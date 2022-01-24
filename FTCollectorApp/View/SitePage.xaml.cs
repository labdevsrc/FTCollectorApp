using FTCollectorApp.Model;
using FTCollectorApp.Service;
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
    public partial class SitePage : ContentPage
    {

        private ObservableCollection<Site> Sites = new ObservableCollection<Site>();
        private bool _isBusy;
        public SitePage()
        {
            InitializeComponent();
            BindingContext = Sites;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            IsBusy = true;
            Console.WriteLine("Connection : " + Connectivity.NetworkAccess.ToString());

            CrossConnectivity.Current.ConnectivityChanged += OnConnectivityHandler;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                // grab Job tables from Url https://collector.fibertrak.com/phonev4/xSite.php
                Sites.Clear();

                var content = await CloudDBService.GetSiteFromAWSMySQLTable();

                Sites = new ObservableCollection<Site>(content);
                Console.WriteLine(content);

                // push Job Tables to local SQLite. Model is in Model.Job
                // with using(SQLiteConnection) we didn't have to do conn.close()
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Site>();
                    conn.InsertAll(content);
                }
            }
            else
            {
                // because no internet network
                // Read Job Tables to local SQLite. Model is in Model.Job
                // with using(SQLiteConnection) we didn't have to do conn.close()
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Site>();
                    var sites = conn.Table<Site>().ToList();
                    Sites = new ObservableCollection<Site>(sites);
                    //listviewPost.ItemsSource = listPost; //= new ObservableCollection<Post>(posts);

                }
            }



            // select MajorSite type from Sites (LINQ command)
            var majorSites = Sites.GroupBy(b => b.MajorSites).Select(g => g.First()).ToList();
            // populate to JobOwnerPicker
            foreach (var majorSite in majorSites)
                majorSitePicker.Items.Add(majorSite.MajorSites);

            //await LocateService.GetLocation(); // get current location
            //await PopupNavigation.Instance.PushAsync(new GpsDevicePopUpView()); // for Rg.plugin popup

            IsBusy = false;
        }

        private async void OnConnectivityHandler(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                // grab Job tables from Url https://collector.fibertrak.com/phonev4/xamarinJob.php
                Sites.Clear();
                var content = await CloudDBService.GetSiteFromAWSMySQLTable();

                Sites = new ObservableCollection<Site>(content);
                Console.WriteLine(content);

                // push Job Tables to local SQLite. Model is in Model.Job
                // with using(SQLiteConnection) we didn't have to do conn.close()
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Site>();
                    conn.InsertAll(content);
                }
            }
        }

        private void btnRecordGPS_Clicked(object sender, EventArgs e)
        {

        }

        private void btnGPSOffset_Clicked(object sender, EventArgs e)
        {

        }
    }
}