using FTCollectorApp.Model;
using FTCollectorApp.Service;
using FTCollectorApp.View;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp
{
    public partial class App : Application
    {
        public static string DatabaseLocation = string.Empty;
        public static string SignatureFileLocation = string.Empty;
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage()); // root page  is MainPage()
        }

        public App(string databaseLoc)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
            DatabaseLocation = databaseLoc;

            

        }

        public App(string databaseLoc, string signatureLoc)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
            DatabaseLocation = databaseLoc;
            SignatureFileLocation = signatureLoc;



        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }



    }
}
