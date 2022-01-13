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
using FTCollectorApp.Page;
using System.Net.Http.Headers;
using Plugin.Connectivity;
using FTCollectorApp.View;

namespace FTCollectorApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerifyJobPage : ContentPage
    {
        private IList<Job> _job;
        private Job _currjob;
        public List<string> OwnerName;


        // Rajib API variables
        private HttpClient httpClient = new HttpClient();
        private ObservableCollection<Job> _jobdetails = new ObservableCollection<Job>();

        public VerifyJobPage()
        {
            InitializeComponent();

            BindingContext = _jobdetails;
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
                var response = await httpClient.GetStringAsync(Constants.GetJobTableUrl);
                var content = JsonConvert.DeserializeObject<List<Job>>(response);

                _jobdetails = new ObservableCollection<Job>(content);
                Console.WriteLine(response);

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
        }

        private async void OnConnectivityHandler(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                // grab Job tables from Url https://collector.fibertrak.com/phonev4/xamarinJob.php
                _jobdetails.Clear();
                var response = await httpClient.GetStringAsync(Constants.GetJobTableUrl);
                var content = JsonConvert.DeserializeObject<List<Job>>(response);

                _jobdetails = new ObservableCollection<Job>(content);
                Console.WriteLine(response);

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

            OnSubmit();

            await Navigation.PushAsync(new BeginWorkPage());
        }

        async void OnSubmit()
        {
            Uri uri = new Uri(string.Format(Constants.InsertTimeSheetUrl, string.Empty));

            // obsolete : POST properties in TimeSheetParams 
            // this metode not worked for x-wwww-url-formencoded
            //
            // Properties in TimeSheetParams similar with SESSION 
            //TimeSheetParams param = new TimeSheetParams
            //{
            //    jobnum = Session.jobnum,
            //    uid = Session.uid
            //};
            // string json = JsonConvert.SerializeObject(param);
            // HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            // Console.WriteLine("json "+ json);

            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("jobnum",Session.jobnum),
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            };
            // this Httpconten will work for Content-type : x-wwww-url-formencoded REST
            HttpContent content = new FormUrlEncodedContent(keyValues);


            HttpResponseMessage response = null;
            response = await httpClient.PostAsync(uri, content);
            
            if (response.IsSuccessStatusCode)
            {
                var isi = await response.Content.ReadAsStringAsync();
                Console.WriteLine("OK 200 " + isi);
            }
        }
    }
}