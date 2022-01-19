using FTCollectorApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using Xamarin.Essentials;
using System.Net.Http.Headers;
using Plugin.Connectivity;
using FTCollectorApp.View;
using FTCollectorApp.Service;
using Rg.Plugins.Popup.Services;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerifyJobPage : ContentPage
    {

        public List<string> OwnerName;


        // Rajib API variables
        private HttpClient httpClient; // = new HttpClient();
        private ObservableCollection<Job> _jobdetails = new ObservableCollection<Job>();




        public VerifyJobPage()
        {
            InitializeComponent();

            BindingContext = _jobdetails;

            try
            {
                httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(Constants.BaseUrl)
                };
            }
            catch
            {

            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine("Connection : "+ Connectivity.NetworkAccess.ToString());

            CrossConnectivity.Current.ConnectivityChanged += OnConnectivityHandler;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                // grab Job tables from Url https://collector.fibertrak.com/phonev4/xamarinJob.php
                _jobdetails.Clear();
               
                var content = await CloudDBService.GetJobFromAWSMySQLTable();

                _jobdetails = new ObservableCollection<Job>(content);
                Console.WriteLine(content);

                // push Job Tables to local SQLite. Model is in Model.Job
                // with using(SQLiteConnection) we didn't have to do conn.close()
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Job>();
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
                    conn.CreateTable<Job>();
                    var jobdetails = conn.Table<Job>().ToList();
                    _jobdetails = new ObservableCollection<Job>(jobdetails);
                    //listviewPost.ItemsSource = listPost; //= new ObservableCollection<Post>(posts);

                }
            }



            // select OwnerName from Job (LINQ command)
            var ownerNames = _jobdetails.GroupBy(b => b.OwnerName).Select(g => g.First()).ToList();
            // populate to JobOwnerPicker
            foreach (var ownerName in ownerNames)
                jobOwnersPicker.Items.Add(ownerName.OwnerName);

            //await LocateService.GetLocation(); // get current location
            await PopupNavigation.Instance.PushAsync(new GpsDevicePopUpView()); // for Rg.plugin popup

        }

        private async void OnConnectivityHandler(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                // grab Job tables from Url https://collector.fibertrak.com/phonev4/xamarinJob.php
                _jobdetails.Clear();
                var content = await CloudDBService.GetJobFromAWSMySQLTable();

                _jobdetails = new ObservableCollection<Job>(content);
                Console.WriteLine(content);

                // push Job Tables to local SQLite. Model is in Model.Job
                // with using(SQLiteConnection) we didn't have to do conn.close()
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Job>();
                    conn.InsertAll(content);
                }
            }
        }

        private void jobOwnersPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

            // get selected owner Name
            var owner = jobOwnersPicker.Items[jobOwnersPicker.SelectedIndex];

            // SELECT JobNumber from Job where OwnerName = (selected) owner (LINQ command)
            var _jobNumbergrouped = _jobdetails.Where(a => a.OwnerName == owner).GroupBy(b => b.JobNumber).Select(g => g.First()).ToList();
            foreach (var jobNumbergrouped in _jobNumbergrouped)
                jobNumbersPicker.Items.Add(jobNumbergrouped.JobNumber);

        }

        private void jobNumbersPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

            var selectedOwner = jobOwnersPicker.SelectedIndex;
            if (selectedOwner == -1)
            {
                DisplayAlert("Warning", "Select Owner First", "OK");

            }

            var jobNumber = jobNumbersPicker.Items[jobNumbersPicker.SelectedIndex];
            var owner = jobOwnersPicker.Items[jobOwnersPicker.SelectedIndex];

            // SELECT JobLocation, ContactName, CustomerName, CustPhoneNum from Job where OwnerName = (selected) owner
            // and JobNumber = (selected) jobNumber (LINQ command)
            var job_Location  = _jobdetails.Where(a => (a.OwnerName == owner) && (a.JobNumber == jobNumber)).Select(a => a.JobLocation).First();
            jobLocation.Text = job_Location.ToString();

            contactName.Text = _jobdetails.Where(a => (a.OwnerName == owner) && (a.JobNumber == jobNumber)).Select(a => a.ContactName).First();
            custName.Text = _jobdetails.Where(a => (a.OwnerName == owner) && (a.JobNumber == jobNumber)).Select(a => a.CustomerName).First();
            custPhoneNum.Text = _jobdetails.Where(a => (a.OwnerName == owner) && (a.JobNumber == jobNumber)).Select(a => a.CustomerPhone).First();

            Session.jobnum = jobNumber;
        }

        private async void submit_Clicked(object sender, EventArgs e)
        {

            await OnSubmit();
        }

        async Task OnSubmit()
        {

            if(Session.gps_sts == "1")
            {
                Session.manual_latti = string.Empty;
                Session.manual_longi = string.Empty;
            }
            Session.event_type = 2;

            
            //Uri uri = new Uri(string.Format(Constants.InsertJobEvents, string.Empty));

            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("jobnum",Session.jobnum),
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),

                new KeyValuePair<string, string>("min", "0"),
                new KeyValuePair<string, string>("hr", "0"),


                new KeyValuePair<string, string>("gps_sts", Session.gps_sts),
                
                // xSaveJobEvents.php Line 59 : $longitude=$_POST['longitude2'];
                // xSaveJobEvents.php Line 60 : $latitude =$_POST['lattitude2'];
                new KeyValuePair<string, string>("manual_latti", Session.manual_latti),
                new KeyValuePair<string, string>("manual_longi", Session.manual_longi),

                // xSaveJobEvents.php Line 73 : $longitude=$_POST['longitude2'];
                // xSaveJobEvents.php Line 74 : $latitude =$_POST['lattitude2'];
                new KeyValuePair<string, string>("lattitude2", Session.lattitude2),
                new KeyValuePair<string, string>("longitude2", Session.longitude2),

                new KeyValuePair<string, string>("evtype", "2"), // event_type 2 : verified job

                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),

                new KeyValuePair<string, string>("ajaxname", Constants.InsertJobEvents)
            };
            // this Httpconten will work for Content-type : x-wwww-url-formencoded REST
            HttpContent content = new FormUrlEncodedContent(keyValues);

            HttpResponseMessage response = null;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                response = await httpClient.PostAsync(Constants.InsertJobEvents, content);
                if (response.IsSuccessStatusCode)
                {
                    var isi = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("OK 200 " + isi);

                }
            }
            else
            {


            }
        }
    }
}