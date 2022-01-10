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
        private const string Url = "https://collector.fibertrak.com/phonev4/xamarinJob.php";
        private const string SubmitUrl = "https://collector.fibertrak.com/phonev4/xPostTimesCheck.php";
        private ObservableCollection<Job> _jobdetails;

        public VerifyJobPage()
        {
            InitializeComponent();
            //_job = GetJobs();
            _jobdetails = new ObservableCollection<Job>();
            //var _ownergrouped = _job.GroupBy(b => b.OwnerName).Select(g => g.First()).ToList();
            //foreach (var ownergrouped in _ownergrouped)
            //    jobOwnersPicker.Items.Add(ownergrouped.OwnerName);
            BindingContext = _jobdetails;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine("Connection : "+ Connectivity.NetworkAccess.ToString());

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

        private void submit_Clicked(object sender, EventArgs e)
        {

            OnSubmit();
            Navigation.PushAsync(new SelectCrewPage());
        }

        async void OnSubmit()
        {
            // POST properties in TimeSheetParams 
            // Properties in TimeSheetParams similar with SESSION 
            // POST Not worked yet
            // POST need header with application/x-www-form-urlencoded
            TimeSheetParams param = new TimeSheetParams
            {
                jobnum = Session.jobnum,
                uid = Session.uid
            };
            Uri uri = new Uri(string.Format(Constants.InsertTimeSheetUrl, string.Empty));
            string json = JsonConvert.SerializeObject(param);
            Console.WriteLine("json "+ json);

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            // content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded"); // not worked yet
            
            HttpResponseMessage response = null;
            response = await httpClient.PostAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("OK 200");
            }
        }
    }
}