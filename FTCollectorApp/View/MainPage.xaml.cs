using FTCollectorApp.Page;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using Xamarin.Essentials;
using FTCollectorApp.Model;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;
using Plugin.Connectivity;
//using FTCollectorApp.ViewModel;

namespace FTCollectorApp
{
    public partial class MainPage : ContentPage
    {

        // Rajib API variables
        private HttpClient httpClient = new HttpClient();
        private ObservableCollection<User> Users = new ObservableCollection<User>();

        public MainPage()
        {
            InitializeComponent();
            //BindingContext = new MainPageViewModel();
        }


        protected override async void OnAppearing()
        {
            Console.WriteLine("Connectivity : " + Connectivity.NetworkAccess);

            // https://stackoverflow.com/questions/40458842/internet-connectivity-listener-in-xamarin-forms
            // https://www.youtube.com/watch?v=aA-sA0ACum0
            CrossConnectivity.Current.ConnectivityChanged += OnConnectivityHandler;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await GetEndUserFromAWSMySQLTable();
            }
            else
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<User>();
                    Console.WriteLine("CreateTable<User> ");
                    var userdetails = conn.Table<User>().ToList();
                    Users = new ObservableCollection<User>(userdetails);
                }
            }

            base.OnAppearing();
        }


        private async Task GetEndUserFromAWSMySQLTable()
        {
            Users.Clear();

            // grab End User tables from Url https://collector.fibertrak.com/phonev4/xamarinLogin.php
            var response = await httpClient.GetStringAsync(Constants.GetEndUserTableUrl);

            var content = JsonConvert.DeserializeObject<List<User>>(response);
            Users = new ObservableCollection<User>(content);
            Console.WriteLine(response);

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<User>();
                conn.InsertAll(content);
            }
        }

        private async void OnConnectivityHandler(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await GetEndUserFromAWSMySQLTable();
            }
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            Session.uid = Users.Where(a => (a.email == entryEmail.Text) && (a.password == entryPassword.Text)).Select(a => a.UserKey).First(); // populate uid to Static-class (session) property uid  

            await Navigation.PushAsync(new VerifyJobPage());
            // await Navigation.PushModalAsync(new VerifyJobPage());

            // update timesheet table in AWS
            // update end_user table in AWS
        }


        private void entryEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                txtFirstName.Text = Users.Where(a => (a.email == entryEmail.Text) && (a.password == entryPassword.Text)).Select(a => a.first_name).First();
                txtLastName.Text = Users.Where(a => (a.email == entryEmail.Text) && (a.password == entryPassword.Text)).Select(a => a.last_name).First();
                Console.WriteLine(txtFirstName.Text + " " + txtLastName.Text);
            }
            catch (Exception exception)
            {
                txtFirstName.Text = "";
                txtLastName.Text = "";
                Console.WriteLine(exception.ToString());
            }
        }

        private void entryPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                txtFirstName.Text = Users.Where(a => (a.email == entryEmail.Text) && (a.password == entryPassword.Text)).Select(a => a.first_name).First();
                txtLastName.Text = Users.Where(a => (a.email == entryEmail.Text) && (a.password == entryPassword.Text)).Select(a => a.last_name).First();
                Console.WriteLine(txtFirstName.Text + " " + txtLastName.Text);
            }
            catch (Exception exception)
            {
                txtFirstName.Text = "";
                txtLastName.Text = "";

                Console.WriteLine(exception.ToString());
            }
        }

    }
}
