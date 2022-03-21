using FTCollectorApp.Model;
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
        public ObservableCollection<CodeSiteType> CodeSiteTypes
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
            set
            {

            }
        }

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
                    //listviewPost.ItemsSource = listPost; //= new ObservableCollection<Post>(posts);

                }
            }
            IsBusy = false;


            //var majorTypes = CodeSiteTypes.GroupBy(b => b.MajorType).Select(g => g.First()).ToList();
            //foreach (var majorType in majorTypes)
            // majorTypePicker.Items.Add(majorType.MajorType);
            
            // majorTypePicker.ItemsSource = MajorTypes;  // improve bugs , when back

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

            Device.StartTimer(TimeSpan.FromSeconds(5), () => OnTimerTick());


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
                    Session.longitude2 = String.Format("{0:0.######}", LocationService.Coords.Longitude);
                    Session.lattitude2 = String.Format("{0:0.######}", LocationService.Coords.Latitude);
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

        private async void btnRecordGPS_Clicked(object sender, EventArgs e)
        {
            if (TagNumberMatch)
            {
                if (selectedMajorType.Equals("Building"))
                {
                    await CloudDBService.PostCreateSiteAsync(entryTagNum.Text, codekey);
                    await Navigation.PushAsync(new BuildingSitePage(selectedMinorType, entryTagNum.Text));
                }
                else if (selectedMajorType.Equals("Cabinet"))
                {
                    await CloudDBService.PostCreateSiteAsync(entryTagNum.Text, codekey);
                    await Navigation.PushAsync(new CabinetSitePage(selectedMinorType, entryTagNum.Text));
                }
                else if (selectedMajorType.Equals("Pull Box"))
                {
                    await CloudDBService.PostCreateSiteAsync(entryTagNum.Text, codekey);
                    await Navigation.PushAsync(new PullBoxSitePage(selectedMinorType, entryTagNum.Text));
                }
                else if (selectedMajorType.Equals("Structure"))
                {
                    await CloudDBService.PostCreateSiteAsync(entryTagNum.Text, codekey);
                    await Navigation.PushAsync(new StructureSitePage(selectedMinorType, entryTagNum.Text));
                }
            }
            else
               await  DisplayAlert("Warning", "Re enter Tag number correctly", "OK");
        }

        private async void btnGPSOffset_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new OffsetGPSPopUp());
        }

        private void majorTypeP_SelectedIdxChanged(object sender, EventArgs e)
        {
            //var selectedMajorType = majorTypePicker.Items[majorTypePicker.SelectedIndex];

            if (majorTypePicker.SelectedIndex != -1)
            {
                selectedMajorType = majorTypePicker.SelectedItem.ToString();
                var minorTypes = CodeSiteTypes.Where(a => a.MajorType == selectedMajorType).GroupBy(b => b.MinorType).Select(g => g.First()).ToList();
                minorTypePicker.Items.Clear();
                foreach (var minorType in minorTypes)
                    minorTypePicker.Items.Add(minorType.MinorType);
            }
        }

        public ObservableCollection<string> MajorTypes
        {
            get
            {
                var data = CodeSiteTypes.GroupBy(b => b.MajorType).Select(g => g.First()).ToList();
                List<string> temp = new List<string>();
                foreach (var col in data)
                {
                    temp.Add(col.MajorType);
                }
                return new ObservableCollection<string>(temp);
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