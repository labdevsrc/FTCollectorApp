using FTCollectorApp.Model;
using FTCollectorApp.Service;
using Newtonsoft.Json;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectCrewPage : ContentPage
    {

        private ObservableCollection<User> Users = new ObservableCollection<User>();
        public SelectCrewPage()
        {
            InitializeComponent();
        }

        private async void btnFinish_Clicked(object sender, EventArgs e)
        {
            Session.event_type = Session.CrewAssembled;
            await CloudDBService.PostJobEvent();
            await Navigation.PushAsync(new StartTimePage());
        }

        private void btnLogOut_Clicked(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }
        //database user table

        protected override async void OnAppearing()
        {
            Console.WriteLine("[SelectCrewPage] Connectivity : " + Connectivity.NetworkAccess);

            // https://stackoverflow.com/questions/40458842/internet-connectivity-listener-in-xamarin-forms
            // https://www.youtube.com/watch?v=aA-sA0ACum0
            CrossConnectivity.Current.ConnectivityChanged += OnConnectivityHandler;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                // grab Job tables from Url https://collector.fibertrak.com/phonev4/xamarinJob.php
                Users.Clear();

                var users = await CloudDBService.GetEndUserFromAWSMySQLTable();

                Users = new ObservableCollection<User>(users);
                Console.WriteLine(users);

                // push Job Tables to local SQLite. Model is in Model.Job
                // with using(SQLiteConnection) we didn't have to do conn.close()
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<User>();
                    conn.InsertAll(users);
                }
            }
            else
            {
                // because no internet network
                // Read Job Tables to local SQLite. Model is in Model.Job
                // with using(SQLiteConnection) we didn't have to do conn.close()
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<User>();
                    var users = conn.Table<User>().ToList();
                    Users = new ObservableCollection<User>(users);
                    //listviewPost.ItemsSource = listPost; //= new ObservableCollection<Post>(posts);

                }
            }
            //

            var empNames = Users.GroupBy(b => b.first_name).Select(g => g.First()).ToList();
            // populate to employees
            foreach (var empName in empNames)
            {
                employeePicker1.Items.Add(empName.first_name + " " + empName.last_name);
                employeePicker2.Items.Add(empName.first_name + " " + empName.last_name);
                employeePicker3.Items.Add(empName.first_name + " " + empName.last_name);
                employeePicker4.Items.Add(empName.first_name + " " + empName.last_name);
                employeePicker5.Items.Add(empName.first_name + " " + empName.last_name);
                employeePicker6.Items.Add(empName.first_name + " " + empName.last_name);
            }

            base.OnAppearing();
        }

        private async Task GetEndUserFromAWSMySQLTable()
        {
            Users.Clear();

            // grab End User tables from Url https://collector.fibertrak.com/phonev4/xamarinLogin.php
            // var response = await httpClient.GetStringAsync(Constants.GetEndUserTableUrl);
            var users = await CloudDBService.GetEndUserFromAWSMySQLTable();
            Users = new ObservableCollection<User>(users);
            Console.WriteLine(users);

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<User>();
                conn.InsertAll(users);
            }
        }

        private async void OnConnectivityHandler(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Users.Clear();

                var users = await CloudDBService.GetEndUserFromAWSMySQLTable();

                Users = new ObservableCollection<User>(users);
                Console.WriteLine(users);

                // push Job Tables to local SQLite. Model is in Model.Job
                // with using(SQLiteConnection) we didn't have to do conn.close()
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<User>();
                    conn.InsertAll(users);
                }
            }
        }
    }
}