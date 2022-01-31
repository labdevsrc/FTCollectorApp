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
using FTCollectorApp.Utils;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerifyJobPage : ContentPage
    {

        public List<string> OwnerName;

        private ObservableCollection<Job> _jobdetails = new ObservableCollection<Job>();
        private bool _isBusy;
        public bool isBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value)
                    return;
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        string selectedowner;
        public string SelectedOwner
        {
            get { return selectedowner; }
            set
            {
                if (selectedowner == value)
                    return;
                selectedowner = value;
                OnPropertyChanged();
            }
        }

        public VerifyJobPage()
        {
            InitializeComponent();

            BindingContext = this;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            isBusy = true;
            Console.WriteLine("Connection : " + Connectivity.NetworkAccess.ToString());

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


                }
            }

            // Data Binding Trial
            //var _ownerNames = _jobdetails.GroupBy(b => b.OwnerName).Select(g => g.First()).ToList();
            //var ownerNames = new ObservableCollection<string>();
            //foreach (var _ownerName in _ownerNames)
            //    ownerNames.Add(_ownerName.OwnerName);
            //jobOwnersPicker.ItemsSource = ownerNames;


            // select OwnerName from Job (LINQ command)
            var ownerNames = _jobdetails.GroupBy(b => b.OwnerName).Select(g => g.First()).ToList();
            // populate to JobOwnerPicker
            foreach (var ownerName in ownerNames)
                  jobOwnersPicker.Items.Add(ownerName.OwnerName);

            await PopupNavigation.Instance.PushAsync(new GpsDevicePopUpView()); // for Rg.plugin popup

            isBusy = false;
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
            
            // Data Binding Trial
            //var owner = SelectedOwner as string;
            //Console.WriteLine($"SelectedOwner {SelectedOwner}");
            
            
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

            // Data Binding Trial
            // var owner = SelectedOwner;

            // SELECT JobLocation, ContactName, CustomerName, CustPhoneNum from Job where OwnerName = (selected) owner
            // and JobNumber = (selected) jobNumber (LINQ command)
            var job_Location  = _jobdetails.Where(a => (a.OwnerName == owner) && (a.JobNumber == jobNumber)).Select(a => a.JobLocation).First();
            jobLocation.Text = job_Location.ToString();

            contactName.Text = _jobdetails.Where(a => (a.OwnerName == owner) && (a.JobNumber == jobNumber)).Select(a => a.ContactName).First();
            custName.Text = _jobdetails.Where(a => (a.OwnerName == owner) && (a.JobNumber == jobNumber)).Select(a => a.CustomerName).First();
            custPhoneNum.Text = _jobdetails.Where(a => (a.OwnerName == owner) && (a.JobNumber == jobNumber)).Select(a => a.CustomerPhone).First();
            Session.stage =  _jobdetails.Where(a => (a.OwnerName == owner) && (a.JobNumber == jobNumber)).Select(a => a.stage).First();
            Session.ownerkey = _jobdetails.Where(a => (a.OwnerName == owner) && (a.JobNumber == jobNumber)).Select(a => a.OwnerKey).First();
            Session.jobkey = _jobdetails.Where(a => (a.OwnerName == owner) && (a.JobNumber == jobNumber)).Select(a => a.JobKey).First();
            var ownerCD = _jobdetails.Where(a => (a.OwnerName == owner) && (a.JobNumber == jobNumber)).Select(a => a.OWNER_CD).First();
            Session.jobnum = jobNumber;
            
            Session.ownerCD = ownerCD.ToString();
        }

        private async void submit_Clicked(object sender, EventArgs e)
        {

            await OnSubmit();
            
            var speaker = DependencyService.Get<ITextToSpeech>();
            speaker?.Speak("Job verified!");


            await Navigation.PushAsync(new SiteInputPage());
            //await Navigation.PushAsync(new EquipmenReturnPage());
            //await Navigation.PushAsync(new BeginWorkPage());
        }

        async Task OnSubmit()
        {

            Session.event_type = Session.JOB_VERIFIED;
            isBusy = true;
            try
            {
                await CloudDBService.PostJobEvent();
            }
            catch
            {
                await DisplayAlert("Error", "Update JobEvent table failed", "OK");
            }
            isBusy = false;

        }

        private async void btnGPSSetting_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new GpsDevicePopUpView()); // for Rg.plugin popup
        }
    }
}