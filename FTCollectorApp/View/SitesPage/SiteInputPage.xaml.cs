﻿using FTCollectorApp.Model;
using FTCollectorApp.Service;
using FTCollectorApp.View.Utils;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
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

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SiteInputPage : ContentPage
    {

        private ObservableCollection<CodeSiteType> CodeSiteTypes = new ObservableCollection<CodeSiteType>();
        private ObservableCollection<Site> Sites = new ObservableCollection<Site>();
        private ObservableCollection<string> TagNumbers;
        string selectedMinorType;
        string selectedMajorType;
        string codekey;


        private bool TagNumberMatch = false;



        public SiteInputPage()
        {
            InitializeComponent();
            BindingContext = this;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            IsBusy = true;
            Console.WriteLine("Connection : " + Connectivity.NetworkAccess.ToString());

            CrossConnectivity.Current.ConnectivityChanged += OnConnectivityHandler;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {

                CodeSiteTypes.Clear();
                Sites.Clear();

                var contentCodeSiteType = await CloudDBService.GetCodeSiteTypeFromAWSMySQLTable();

                CodeSiteTypes = new ObservableCollection<CodeSiteType>(contentCodeSiteType);
                Console.WriteLine(contentCodeSiteType);


                var contentSite = await CloudDBService.GetSiteFromAWSMySQLTable();

                Sites = new ObservableCollection<Site>(contentSite);
                Console.WriteLine(contentSite);

                // push code_site_type Tables to local SQLite. Model is in Model.Job
                // with using(SQLiteConnection) we didn't have to do conn.close()
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CodeSiteType>();
                    conn.InsertAll(contentCodeSiteType);

                    conn.CreateTable<Site>();
                    conn.InsertAll(contentSite);
                }
            }
            else
            {
                // because no internet network
                // Read Job Tables to local SQLite. Model is in Model.Job
                // with using(SQLiteConnection) we didn't have to do conn.close()
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CodeSiteType>();
                    var codesiteypes = conn.Table<CodeSiteType>().ToList();
                    CodeSiteTypes = new ObservableCollection<CodeSiteType>(codesiteypes);
                    //listviewPost.ItemsSource = listPost; //= new ObservableCollection<Post>(posts);

                }
            }



            // select MajorSite type from Sites (LINQ command)
            //var majorSites = CodeSiteTypes.GroupBy(b => b.MajorSites).Select(g => g.First()).ToList();
            //foreach (var majorSite in majorSites)
            //    majorSitePicker.Items.Add(majorSite.MajorSites);
            var majorTypes = CodeSiteTypes.GroupBy(b => b.MajorType).Select(g => g.First()).ToList();
            foreach (var majorType in majorTypes)
                majorTypePicker.Items.Add(majorType.MajorType);


            List<string> tagnumbers = new List<string>();
            var tagNumbers = Sites.GroupBy(b => b.TagNumber).Select(g => g.First()).ToList();
            foreach (var tagNumber in tagNumbers) {
                tagnumbers.Add(tagNumber.TagNumber);
                Console.WriteLine($"tagnumber : {tagNumber.TagNumber}");
            }
            TagNumbers = new ObservableCollection<string>(tagnumbers);

            //await LocateService.GetLocation(); // get current location
            //await PopupNavigation.Instance.PushAsync(new GpsDevicePopUpView()); // for Rg.plugin popup
            stagePicker.Text = Session.stage;
            //stagePicker.Items.Add(Session.stage);

            IsBusy = false;
        }

        private async void OnConnectivityHandler(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                CodeSiteTypes.Clear();
                Sites.Clear();

                var contentCodeSiteType = await CloudDBService.GetCodeSiteTypeFromAWSMySQLTable();

                CodeSiteTypes = new ObservableCollection<CodeSiteType>(contentCodeSiteType);
                Console.WriteLine(contentCodeSiteType);


                var contentSite = await CloudDBService.GetSiteFromAWSMySQLTable();

                Sites = new ObservableCollection<Site>(contentSite);
                Console.WriteLine(contentSite);

                // push code_site_type Tables to local SQLite. Model is in Model.Job
                // with using(SQLiteConnection) we didn't have to do conn.close()
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CodeSiteType>();
                    conn.InsertAll(contentCodeSiteType);

                    conn.CreateTable<Site>();
                    conn.InsertAll(contentSite);
                }
            }
        }

        private async void btnRecordGPS_Clicked(object sender, EventArgs e)
        {
            if (TagNumberMatch)
            {
                if (selectedMajorType.Equals("Building"))
                {
                    await CloudDBService.PostCreateSiteAsync(entryTagNum.Text, codekey);
                    Navigation.PushAsync(new BuildingSitePage(selectedMajorType, selectedMinorType, entryTagNum.Text));
                }
                else if (selectedMajorType.Equals("Cabinet"))
                {
                    Navigation.PushAsync(new CabinetSitePage(selectedMajorType, selectedMinorType, entryTagNum.Text));
                }
                else if (selectedMajorType.Equals("Pull Box"))
                {
                    Navigation.PushAsync(new PullBoxSitePage(selectedMajorType, selectedMinorType, entryTagNum.Text));
                }
                else if (selectedMajorType.Equals("Structure"))
                {
                    Navigation.PushAsync(new StructureSitePage(selectedMajorType, selectedMinorType, entryTagNum.Text));
                }
            }
            else
                DisplayAlert("Warning", "Re enter Tag number correctly", "OK");
        }

        private void btnGPSOffset_Clicked(object sender, EventArgs e)
        {

        }

        private void majorTypeP_SelectedIdxChanged(object sender, EventArgs e)
        {
            var selectedMajorType = majorTypePicker.Items[majorTypePicker.SelectedIndex];

            var minorTypes = CodeSiteTypes.Where(a => a.MajorType == selectedMajorType).GroupBy(b => b.MinorType).Select(g => g.First()).ToList();
            minorTypePicker.Items.Clear();
            foreach (var minorType in minorTypes)
                minorTypePicker.Items.Add(minorType.MinorType);
        }

        private async void minorTypeP_SelectedIdxChanged(object sender, EventArgs e)
        {
            var selectedMajorTypeIdx =majorTypePicker.SelectedIndex;
            if(selectedMajorTypeIdx == -1)
            {
                await DisplayAlert("Warning", "Please select Major Type first", "OK");
            }
            var selectedMinorTypeIdx = minorTypePicker.SelectedIndex;
            if (selectedMinorTypeIdx == -1)
            {
                Console.WriteLine("minorTypePicker.SelectedIndex : -1");
                return;
            }


            selectedMinorType = minorTypePicker.Items[minorTypePicker.SelectedIndex];
            selectedMajorType = majorTypePicker.Items[majorTypePicker.SelectedIndex];


            codekey = CodeSiteTypes.Where(a => (a.MajorType == selectedMajorType) && (a.MinorType == selectedMinorType)).Select(a => a.CodeKey).First();
            
            Console.WriteLine($"key {codekey}, MajorType {selectedMajorType.ToString()}, MinorType {selectedMinorType.ToString()}");

        }


        private async void btnGPSSetting_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new GpsDevicePopUpView()); // for Rg.plugin popup
        }

        private async void entryTagNum_TextChanged(object sender, EventArgs e)
        {
            if (TagNumbers.Contains(entryTagNum.Text))
            {
                await DisplayAlert("Warning", $"Tag {entryTagNum.Text} already taken", "OK");
                entryTagNum.Text = "";
            }
        }

        private void entryTagNum2_TextChanged(object sender, EventArgs e)
        {
            if (entryTagNum.Text.Equals(entryTagNum2.Text))
            {
                stsReEnter.Text = "Match!";
                stsReEnter.TextColor = Color.Blue;
                btnRecordGPS.IsEnabled = true;
                btnGPSOffset.IsEnabled = true;
                TagNumberMatch = true;
            }
            else
            {
                stsReEnter.Text = "Check Again Tag";
                stsReEnter.TextColor = Color.Red;
                btnRecordGPS.IsEnabled = false;
                btnGPSOffset.IsEnabled = false;
            }
        }
    }
}