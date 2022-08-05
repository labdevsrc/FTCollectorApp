using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model;
using FTCollectorApp.Service;
using FTCollectorApp.View;
using FTCollectorApp.View.SitesPage;
using FTCollectorApp.View.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Input;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public partial class SiteInputViewModel : ObservableObject
    {
        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(MinorSiteList))]
        string selectedMajorType;

        [ObservableProperty]
        string selectedMinorType;

        string tagNumber;
        public string TagNumber
        {
            get => tagNumber;
            set
            {
                SetProperty(ref tagNumber, value);
                Session.tag_number = value;
                if (!string.IsNullOrEmpty(ReEnterTagNumber))
                    CheckTagNumberCommand?.Execute(null); // when reentertag changed, do checktagnumber 
            }
        }

        [ObservableProperty]
        string accuracy;

        [ObservableProperty]
        string reEnterStatus;

        string stage;
        public string Stage
        {
            get
            {
                if (Session.stage.Equals("I"))
                    return "Install";
                else if (Session.stage.Equals("A"))
                    return "As Built";
                else if (Session.stage.Equals("R"))
                    return "Repair";
                else
                    return "Unknown";

            }
        }

        string reEnterTagNumber;
        public string ReEnterTagNumber
        {
            get => reEnterTagNumber;
            set
            {
                SetProperty(ref reEnterTagNumber, value);
                if (!string.IsNullOrEmpty(TagNumber))
                    CheckTagNumberCommand?.Execute(null); // when reentertag changed, do checktagnumber 
            }
        }


        public ICommand RecordGPSCommand { get; set; }
        public ICommand OpenGPSOffsetPopupCommand { get; set; }

        public ICommand FinishCommand { get; set; }
        public ICommand CaptureCommand { get; set; }

        public ICommand CheckTagNumberCommand { get; set; }
        public ICommand DisplayGPSOption { get; set; }

        bool GPSMode_NoOffset = true;
        ReadGPSTimer timer;
        string codekey;


        ObservableCollection<CodeSiteType> CodeSiteTypeList;


        public SiteInputViewModel()
        {
            CaptureCommand = new Command(() => ExecuteCaptureCommand());
            FinishCommand = new Command(() => ExecuteFinishCommand());
            OpenGPSOffsetPopupCommand = new Command(() => ExecuteOpenGPSOffsetPopupCommand());
            RecordGPSCommand = new Command(() => ExecuteRecordCommand());
            CheckTagNumberCommand = new Command(() => ExecuteCheckTagNumberCommand());

            DisplayGPSOption = new Command(() => ExecuteDisplayGPSOption());

            Session.lattitude_offset = string.Empty;
            Session.longitude_offset = string.Empty;
            Session.gps_offset_bearing = string.Empty;
            Session.gps_offset_distance = string.Empty;

            if (timer == null)
            {
                timer = new ReadGPSTimer(TimeSpan.FromSeconds(5), OnGPSTimerStart);
                timer.Start();
            }


            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<CodeSiteType>();
                var table = conn.Table<CodeSiteType>().ToList();
                foreach (var col in table)
                {
                    col.MinorType = HttpUtility.HtmlDecode(col.MinorType); // should use for escape char "
                }
                CodeSiteTypeList = new ObservableCollection<CodeSiteType>(table);
            }
        }


        async void OnGPSTimerStart()
        {
            try
            {
                await LocationService.GetLocation();
                Accuracy = $"{LocationService.Coords.Accuracy}";
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
                Accuracy = "No GPS";
            }

        }
        private void ExecuteCheckTagNumberCommand()
        {
            if (TagNumber.Equals(ReEnterTagNumber))
            {
                ReEnterStatus = "MATCH!";
                IsTagNumberMatch = true;
            }
            else
            {
                ReEnterStatus = "No Match";
                IsTagNumberMatch = false;
            }
        }

        private async void ExecuteOpenGPSOffsetPopupCommand()
        {
            // OffsetGPSPopUp output 
            // Computed new coord 
            // - Session.lattitude2
            // - Session.longitude2
            // bearing, distance value
            // - Session.gps_offset_bearing
            // - Session.gps_offset_distance


            await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new OffsetGPSPopUp());
            // if offset bearing and offset distance is not empty, offset mode true

        }

        ObservableCollection<CodeSiteType> BufferMajorSite = new ObservableCollection<CodeSiteType>();
        public ObservableCollection<string> MajorSiteList
        {
            get
            {
                var table = CodeSiteTypeList.GroupBy(b => b.MajorType).Select(g => g.First()).Select(c => c.MajorType).ToList();
                return new ObservableCollection<string>(table);

            }
        }


        public ObservableCollection<string> MinorSiteList
        {
            get
            {

                var table = CodeSiteTypeList.Where(a => a.MajorType == SelectedMajorType).OrderBy(d => d.MinorType).ToList();
                BufferMajorSite = new ObservableCollection<CodeSiteType>(table);
                var table2 = BufferMajorSite.Select(c => c.MinorType);
                return new ObservableCollection<string>(table2);
            }
        }


        async void ExecuteDisplayGPSOption()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new GpsDevicePopUpView()); // for Rg.plugin popup
        }
        
        [ObservableProperty]
        bool isTagNumberMatch = false;

        private async void ExecuteRecordCommand()
        {
            // get current tag_number's site_type
            string result = String.Empty;
            Console.WriteLine();
            codekey = CodeSiteTypeList.Where(a => (a.MajorType == SelectedMajorType) && (a.MinorType == SelectedMinorType)).Select(a => a.CodeKey).First();
            Session.site_type_key = codekey;
            Console.WriteLine($"key {codekey}, MajorType {SelectedMajorType}, MinorType {SelectedMinorType}");

            // get current tag_number's key
            ObservableCollection<Site> Sites;
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Site>();
                var table = conn.Table<Site>().ToList();
                int maxSiteKey = 0;
                bool siteKeyTableExisted = false;
                foreach (var col in table)
                {
                    if (col.SiteKey != null)
                    {
                        if (int.Parse(col.SiteKey) > maxSiteKey)
                            maxSiteKey = int.Parse(col.SiteKey);
                    }
                    // check if entered Site is already existed in Local SQLite
                    if (col.SiteName.Equals(Session.tag_number))
                    {
                        Session.site_key = col.SiteKey;
                        siteKeyTableExisted = true;
                    }
                }

                if (!siteKeyTableExisted)
                {
                    Session.site_key = (maxSiteKey+1).ToString();
                    conn.Insert(new Site
                    {
                        SiteKey = (maxSiteKey + 1).ToString(),
                        SiteName = Session.tag_number,
                        TagNumber = Session.tag_number,
                        SiteId = Session.tag_number,
                        OwnerKey = Session.ownerkey,
                        SiteTypeKey = Session.site_type_key
                    });
                    Console.WriteLine();
                }

            }




            if (IsTagNumberMatch)
            {
                OnPropertyChanged(nameof(ReEnterStatus)); // notify the status to display "Match"

                if (string.IsNullOrEmpty(Session.gps_offset_bearing) && string.IsNullOrEmpty(Session.gps_offset_distance))
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


                result = await CloudDBService.PostCreateSiteAsync(TagNumber, codekey);
                if (result.Equals("DUPLICATED"))
                {
                    Session.Result = "CreateSiteOK";

                    var OkAnswer = await Application.Current.MainPage.DisplayAlert("Please Confirm", "Update existed Tag Number ? ", "OK", "Cancel");
                    if (OkAnswer)
                    {
                        result = await CloudDBService.UpdateSite(TagNumber , codekey);
                        Console.WriteLine(result);
                        Session.Result = "CreateSiteOK";
                    }
                    else
                        return;

                }
                
                
                if (result.Equals("CREATE_DONE") || result.Equals("UPDATE_DONE"))
                {
                    // get answer from popup 
                    var OkAnswer = await Application.Current.MainPage.DisplayAlert("DONE", result.Equals("CREATE_DONE") ? "Create Site Success" : "Update Site Success", "Goto " + SelectedMajorType, "Create Again");
                    if (OkAnswer)
                    {
                        // stop timer gps
                        timer.Stop();

                        // move to next page
                        if (SelectedMajorType.Equals("Building"))
                        {
                            await Application.Current.MainPage.Navigation.PushAsync(new BuildingSitePageView(SelectedMinorType, TagNumber));
                        }
                        else if (SelectedMajorType.Equals("Cabinet"))
                        {
                            await Application.Current.MainPage.Navigation.PushAsync(new CabinetSitePageView(SelectedMinorType, TagNumber));
                        }
                        else if (SelectedMajorType.Equals("Pull Box"))
                        {
                            await Application.Current.MainPage.Navigation.PushAsync(new PullBoxSitePage(SelectedMinorType, TagNumber));
                        }
                        else if (SelectedMajorType.Equals("Structure"))
                        {
                            await Application.Current.MainPage.Navigation.PushAsync(new StructureSitePage(SelectedMinorType, TagNumber));
                        }
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Unknown Response", result, "TRY AGAIN");
                }



            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Warning", "Re enter Tag number correctly", "OK");
            }


        }
        public async void ExecuteFinishCommand()
        {
            await Application.Current.MainPage.Navigation.PopAsync();

        }
        public async void ExecuteCaptureCommand()
        {

            await Application.Current.MainPage.Navigation.PushAsync(new CameraViewPage());
        }
    }
}
