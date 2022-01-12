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
            base.OnAppearing();
            
            Console.WriteLine("Connectivity : " + Connectivity.NetworkAccess);

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                
                Users.Clear();
                var response = await httpClient.GetStringAsync(Constants.GetEndUserTableUrl); // grab End User tables from Url https://collector.fibertrak.com/phonev4/xamarinLogin.php
                var content = JsonConvert.DeserializeObject<List<User>>(response);
                Users = new ObservableCollection<User>(content);
                Console.WriteLine(response);

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<User>();
                    conn.InsertAll(content);
                }
            }
            else
            {


                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<User>();
                    var userdetails = conn.Table<User>().ToList();
                    Users = new ObservableCollection<User>(userdetails);
                }
            }


        }
        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            Session.uid = Users.Where(a => (a.email == entryEmail.Text) && (a.password == entryPassword.Text)).Select(a => a.UserKey).First(); // populate uid to Static-class (session) property uid  

            await Navigation.PushModalAsync(new VerifyJobPage());
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
