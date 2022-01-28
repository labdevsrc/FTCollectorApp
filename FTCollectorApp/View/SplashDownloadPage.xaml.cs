using FTCollectorApp.Model;
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
            var contentUser = await CloudDBService.GetEndUserFromAWSMySQLTable();
            var contentJob = await CloudDBService.GetJobFromAWSMySQLTable();
            var contentCodeSiteType = await CloudDBService.GetEndUserFromAWSMySQLTable();
            var contentSite = await CloudDBService.GetJobFromAWSMySQLTable();
            var contentCrewDefault = await CloudDBService.GetCrewDefaultFromAWSMySQLTable();

            txtLoading.Text = "SQLite Dumping...";
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

            }

            IsBusy = false;
        }


        private void LoginClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }

        private void ExitClicked(object sender, EventArgs e)
        {
            var closer = DependencyService.Get<ICloseApps>();
            closer?.closeApplication();
        }
    }
}