using FTCollectorApp.Model;
using FTCollectorApp.Service;
using FTCollectorApp.Utils;
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
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SiteInputPage : ContentPage
    {
        /// will be moved to View Model
        public ObservableCollection<CodeSiteType> CodeMajorSiteTypes
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CodeSiteType>();
                    var table = conn.Table<CodeSiteType>().GroupBy(b => b.MajorType).Select(g => g.First()).ToList();
                    foreach (var col in table)
                    {
                        col.MinorType = HttpUtility.HtmlDecode(col.MinorType); // should use for escape char "
                    }
                    return new ObservableCollection<CodeSiteType>(table);
                }
            }
        }

        public ObservableCollection<CodeSiteType> CodeSiteTypes
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CodeSiteType>();
                    var table = conn.Table<CodeSiteType>().ToList();
                    foreach (var col in table)
                    {
                        col.MinorType = HttpUtility.HtmlDecode(col.MinorType); // should use for escape char "
                    }
                    return new ObservableCollection<CodeSiteType>(table);
                }
            }
        }


        private ObservableCollection<Site> Sites = new ObservableCollection<Site>();
        private ObservableCollection<string> TagNumbers;
        string selectedMinorType;
        string selectedMajorType;
        string codekey;


        private bool TagNumberMatch = false;
        private bool TagNumberExisted = false;
        bool GPSMode_NoOffset = true;

        public SiteInputPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        GPSTimer timer;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var speaker = DependencyService.Get<ITextToSpeech>();
            speaker?.Speak("New Site");

            IsBusy = true;
            Console.WriteLine("Connection : " + Connectivity.NetworkAccess.ToString());

            CrossConnectivity.Current.ConnectivityChanged += OnConnectivityHandler;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {

                if (Constants.AutoSyncAWSTables)  // automatically sync // vic-0310
                {
                    // Get code_site_type table from AWS
                    // vic-0310
                    //var contentCodeSiteType = await CloudDBService.GetCodeSiteTypeFromAWSMySQLTable();
                    //CodeSiteTypes = new ObservableCollection<CodeSiteType>(contentCodeSiteType);
                    //Console.WriteLine(contentCodeSiteType);

                    // Get Site table from AWS
                    var contentSite = await CloudDBService.GetSiteFromAWSMySQLTable();
                    Sites = new ObservableCollection<Site>(contentSite);
                    Console.WriteLine(contentSite);

                    // push code_site_type Tables to local SQLite. Model is in Model.Job
                    // with using(SQLiteConnection) we didn't have to do conn.close()
                    using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                    {
                        // vic-0310
                        //conn.CreateTable<CodeSiteType>();
                        //conn.DeleteAll<CodeSiteType>();
                        //conn.InsertAll(contentCodeSiteType);

                        conn.CreateTable<Site>();
                        conn.DeleteAll<Site>();
                        conn.InsertAll(contentSite);
                    }
                }
            }
            else
            {
                // because no internet network
                // Read Job Tables to local SQLite. Model is in Model.Job
                // with using(SQLiteConnection) we didn't have to do conn.close()
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    // vic-0310
                    //conn.CreateTable<CodeSiteType>();
                    //var codesiteypes = conn.Table<CodeSiteType>().ToList();
                    //CodeSiteTypes = new ObservableCollection<CodeSiteType>(codesiteypes);

                    conn.CreateTable<Site>();
                    var sites = conn.Table<Site>().ToList();
                    Sites = new ObservableCollection<Site>(sites);
                    /*Console.WriteLine();

                    var table = Sites.GroupBy(b => b.TagNumber).Select(g => g.First()).ToList();
                    foreach (var col in table)
                    {
                        TagNumbers.Add(col.TagNumber);
                    }
                    Console.WriteLine();*/

                }
            }
            IsBusy = false;


            List<string> tagnumbers = new List<string>();
            var tagNumbers = Sites.GroupBy(b => b.TagNumber).Select(g => g.First()).ToList();
            foreach (var tagNumber in tagNumbers) {
                tagnumbers.Add(tagNumber.TagNumber);
                Console.WriteLine($"tagnumber : {tagNumber.TagNumber}");
            }
            TagNumbers = new ObservableCollection<string>(tagnumbers);

            if(Session.stage.Equals("I"))
                stagePicker.Text = "Install";
            else if (Session.stage.Equals("A"))
                stagePicker.Text = "As Built";
            else if (Session.stage.Equals("R"))
                stagePicker.Text = "Repair";


            //Device.StartTimer(TimeSpan.FromSeconds(10), () => OnTimerTick());

            if (timer == null)
            {
                timer = new GPSTimer(TimeSpan.FromSeconds(5), OnGPSTimerStart);
                timer.Start();
            }


        }
        async void OnGPSTimerStart()
        {
            try
            {
                await LocationService.GetLocation();
                entryAccuracy.Text = $"{LocationService.Coords.Accuracy}";
                Session.accuracy = String.Format("{0:0.######}", LocationService.Coords.Accuracy);
                //Session.longitude2 = String.Format("{0:0.######}", LocationService.Coords.Longitude);
                //Session.lattitude2 = String.Format("{0:0.######}", LocationService.Coords.Latitude);
                Session.live_longitude = String.Format("{0:0.######}", LocationService.Coords.Longitude);
                Session.live_lattitude = String.Format("{0:0.######}", LocationService.Coords.Latitude);
                Session.altitude = String.Format("{0:0.######}", LocationService.Coords.Altitude);
                //{ String.Format("{0:0.#######}", _location.Latitude.ToString())}
            }
            catch
            {
                entryAccuracy.Text = "No GPS";
            }

        }

        bool OnTimerTick()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await LocationService.GetLocation();
                    entryAccuracy.Text = $"{LocationService.Coords.Accuracy}";
                    Session.accuracy = String.Format("{0:0.######}", LocationService.Coords.Accuracy);
                    //Session.longitude2 = String.Format("{0:0.######}", LocationService.Coords.Longitude);
                    //Session.lattitude2 = String.Format("{0:0.######}", LocationService.Coords.Latitude);
                    Session.live_longitude = String.Format("{0:0.######}", LocationService.Coords.Longitude);
                    Session.live_lattitude = String.Format("{0:0.######}", LocationService.Coords.Latitude);
                    Session.altitude = String.Format("{0:0.######}", LocationService.Coords.Altitude);
                    //{ String.Format("{0:0.#######}", _location.Latitude.ToString())}
                }
                catch
                {
                    entryAccuracy.Text = "No GPS";
                }
            });
            return true;
        }

        protected override void OnDisappearing()
        {
            // need to stop timer here
            if (timer != null)
                timer.Stop();

            base.OnDisappearing();
        }

        private async void OnConnectivityHandler(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (Constants.AutoSyncAWSTables)
                {
                    // vic-0310
                    // var contentCodeSiteType = await CloudDBService.GetCodeSiteTypeFromAWSMySQLTable();
                    // CodeSiteTypes = new ObservableCollection<CodeSiteType>(contentCodeSiteType);
                    // Console.WriteLine(contentCodeSiteType);


                    var contentSite = await CloudDBService.GetSiteFromAWSMySQLTable();

                    Sites = new ObservableCollection<Site>(contentSite);
                    Console.WriteLine(contentSite);

                    // push code_site_type Tables to local SQLite. Model is in Model.Job
                    // with using(SQLiteConnection) we didn't have to do conn.close()
                    using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                    {
                        // vic-0310
                        //conn.CreateTable<CodeSiteType>();
                        //conn.DeleteAll<CodeSiteType>();
                        //conn.InsertAll(contentCodeSiteType);

                        conn.CreateTable<Site>();
                        conn.DeleteAll<Site>();
                        conn.InsertAll(contentSite);
                    }
                }
            }
        }

        CodeSiteType _selectedMajorType;
        public CodeSiteType SelectedMajorType
        {
            get => _selectedMajorType;
            set
            {
                _selectedMajorType = value;
                OnPropertyChanged(nameof(SelectedMajorType));
            }
        }

        private async void btnRecordGPS_Clicked(object sender, EventArgs e)
        {
            string result = String.Empty;
            if (TagNumberMatch)
            {
                if (GPSMode_NoOffset) // normal gps record 
                {
                    Session.lattitude2 = Session.live_lattitude;
                    Session.longitude2 = Session.live_longitude;

                    Session.lattitude_offset = string.Empty;
                    Session.longitude_offset = string.Empty;
                }
                else // offset gps record 
                {

                    Session.lattitude_offset = Session.live_lattitude;
                    Session.longitude_offset = Session.live_longitude;

                }


                result = await CloudDBService.PostCreateSiteAsync(entryTagNum.Text, codekey);
                if (result.Equals("DUPLICATED"))
                {
                    Session.Result = "CreateSiteOK";
                    var OkAnswer = await DisplayAlert("Please Confirm", "Update existed Tag Number ? ", "OK", "Cancel");
                    if (OkAnswer)
                    {
                        result = await CloudDBService.UpdateSite(entryTagNum.Text, codekey);
                        Session.Result = "CreateSiteOK";
                    }
                    else
                        return;

                }
                else if (result.Equals("DONE"))
                {
                    var OkAnswer = await DisplayAlert("DONE", "Create Site Succes", "Goto " + selectedMajorType, "Create Again");
                    if (OkAnswer)
                    {
                        if (selectedMajorType.Equals("Building"))
                        {
                            await Navigation.PushAsync(new BuildingSitePage(selectedMinorType, entryTagNum.Text));
                        }
                        else if (selectedMajorType.Equals("Cabinet"))
                        {
                            await Navigation.PushAsync(new CabinetSitePage(selectedMinorType, entryTagNum.Text));
                        }
                        else if (selectedMajorType.Equals("Pull Box"))
                        {
                            await Navigation.PushAsync(new PullBoxSitePage(selectedMinorType, entryTagNum.Text));
                        }
                        else if (selectedMajorType.Equals("Structure"))
                        {
                            await Navigation.PushAsync(new StructureSitePage(selectedMinorType, entryTagNum.Text));
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Unknown Response from Site Table", result, "TRY AGAIN");
                }

            }
            else
            {
                await DisplayAlert("Warning", "Re enter Tag number correctly", "OK");
            }
        }

        private async void btnGPSOffset_Clicked(object sender, EventArgs e)
        {
            // OffsetGPSPopUp output 
            // Computed new coord 
            // - Session.lattitude2
            // - Session.longitude2
            // bearing, distance value
            // - Session.gps_offset_bearing
            // - Session.gps_offset_distance

            GPSMode_NoOffset = true;
            await PopupNavigation.PushAsync(new OffsetGPSPopUp());
            // if offset bearing and offset distance is not empty, offset mode true
            if (!string.IsNullOrEmpty(Session.gps_offset_bearing) && !string.IsNullOrEmpty(Session.gps_offset_distance))
            {
                Session.lattitude_offset = string.Empty;
                Session.longitude_offset = string.Empty;
                GPSMode_NoOffset = false;
            }

            await PopupNavigation.Instance.PushAsync(new OffsetGPSPopUp());
        }

        private void majorTypeP_SelectedIdxChanged(object sender, EventArgs e)
        {
            //var selectedMajorType = majorTypePicker.Items[majorTypePicker.SelectedIndex];

            if (majorTypePicker.SelectedIndex != -1)
            {
                // before_code
                //selectedMajorType = majorTypePicker.SelectedItem.ToString();
                //var minorTypes = CodeSiteTypes.Where(a => a.MajorType == selectedMajorType).GroupBy(b => b.MinorType).Select(g => g.First()).ToList();
                //minorTypePicker.Items.Clear();
                //foreach (var minorType in minorTypes)
                //    minorTypePicker.Items.Add(minorType.MinorType);

                // after_code : more compact
                var selected = majorTypePicker.SelectedItem as CodeSiteType;

                SelectedMajorType = selected;

                minorTypePicker.ItemsSource = CodeSiteTypes.Where(a => a.MajorType == selected.MajorType).OrderBy(d => d.MinorType).ToList(); // change , A-Z order
            }
        }


        private async void minorTypeP_SelectedIdxChanged(object sender, EventArgs e)
        {
            var selectedMajorTypeIdx =majorTypePicker.SelectedIndex;
            if(selectedMajorTypeIdx == -1)
            {
                await DisplayAlert("Warning", "Please select Major Type first", "OK");
            }
            var selectedMinorTypeIdx = minorTypePicker.SelectedIndex;
            if (minorTypePicker.SelectedIndex == -1)
            {
                Console.WriteLine("minorTypePicker.SelectedIndex : -1");
                return;
            }


            selectedMinorType = minorTypePicker.Items[minorTypePicker.SelectedIndex];
            selectedMajorType = majorTypePicker.Items[majorTypePicker.SelectedIndex];


            codekey = CodeSiteTypes.Where(a => (a.MajorType == selectedMajorType) && (a.MinorType == selectedMinorType)).Select(a => a.CodeKey).First();
            Session.site_type_key = codekey;
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
                Console.WriteLine();
                Session.site_key = Sites.Where(a => (a.TagNumber == entryTagNum.Text)).Select(b => b.SiteKey).First();

                Console.WriteLine();
                TagNumberExisted = true;
            }
        }

        private void entryTagNum2_TextChanged(object sender, EventArgs e)
        {
            if (entryTagNum.Text.Equals(entryTagNum2.Text))
            {
                stsReEnter.Text = "Match!";

                btnRecordGPS.IsEnabled = true;
                btnGPSOffset.IsEnabled = true;
                TagNumberMatch = true;
            }
            else
            {
                stsReEnter.Text = "Check Again Tag";

                btnRecordGPS.IsEnabled = false;
                btnGPSOffset.IsEnabled = false;
            }
        }

    }
}