﻿using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Service;
using FTCollectorApp.View.SitesPage;
using FTCollectorApp.View.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public partial class LocatePointViewModel : ObservableObject
    {

        string accuracy;
        public string GPSAccuracy
        {
            get => location.Accuracy.ToString();
            set
            {
                SetProperty(ref accuracy, value);
            }
        }


        [ObservableProperty]
        string commentText;

        [ObservableProperty]
        CodeLocatePoint selectedSiteType;

        Location location;


        string curLocPoint;
        public string CurLocPoint
        {
            get {
                Console.WriteLine(LocPointNumber);
                return LocPointNumber.ToString();
            } 
            set
            {
                Console.WriteLine();
                SetProperty(ref curLocPoint, value);
            }
        }


        public ICommand RecordCommand { get; set; }
        public ICommand OpenGPSOffsetPopupCommand { get; set; }

        public ICommand FinishCommand { get; set; }
        public ICommand CaptureCommand { get; set; }

        GpsPoint? maxGPSpoint;
        int LocPointNumber = 0;
        ReadGPSTimer RdGpstimer;

        public LocatePointViewModel()
        {

            // Get accuracy
            location = LocationService.Coords;


            CaptureCommand = new Command(() => ExecuteCaptureCommand());
            FinishCommand = new Command(() => ExecuteFinishCommand());
            OpenGPSOffsetPopupCommand = new Command(() => ExecuteOpenGPSOffsetPopupCommand());
            RecordCommand = new Command(() => ExecuteRecordCommand());

            // get max value in gps_point table
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<GpsPoint>();
                maxGPSpoint = conn.Table<GpsPoint>().First();

                // compare with from gps_point MAX(id)
                if (int.Parse(maxGPSpoint?.MaxId) != int.Parse(Session.GpsPointMaxIdx))
                    maxGPSpoint.MaxId = Session.GpsPointMaxIdx;

                LocPointNumber = int.Parse(Session.GpsPointMaxIdx) + 1;
                Console.WriteLine(maxGPSpoint.MaxId);
            }

            Session.lattitude_offset = string.Empty;
            Session.longitude_offset = string.Empty;
            Session.gps_offset_bearing = string.Empty;
            Session.gps_offset_distance = string.Empty;

            if (RdGpstimer == null)
            {
                RdGpstimer = new ReadGPSTimer(TimeSpan.FromSeconds(5), OnGPSTimerStart);
                RdGpstimer.Start();
            }


        }

        async void OnGPSTimerStart()
        {
            try
            {
                await LocationService.GetLocation();

                Session.accuracy = String.Format("{0:0.######}", LocationService.Coords.Accuracy);
                //Session.longitude2 = String.Format("{0:0.######}", LocationService.Coords.Longitude);
                //Session.lattitude2 = String.Format("{0:0.######}", LocationService.Coords.Latitude);
                Session.live_longitude = String.Format("{0:0.######}", LocationService.Coords.Longitude);
                Session.live_lattitude = String.Format("{0:0.######}", LocationService.Coords.Latitude);
                Session.altitude = String.Format("{0:0.######}", LocationService.Coords.Altitude);
                //{ String.Format("{0:0.#######}", _location.Latitude.ToString())}
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

        }

        List<KeyValuePair<string, string>> keyvaluepairLocate()
        {
            Session.GpsPointMaxIdx = (int.Parse(maxGPSpoint?.MaxId) + 1).ToString();

            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new KeyValuePair<string, string>("jobnum", Session.jobnum),
                new KeyValuePair<string, string>("ownerkey", Session.ownerkey),
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD),

                new KeyValuePair<string, string>("locate_point_number", Session.GpsPointMaxIdx),
                new KeyValuePair<string, string>("tag_from", Session.FromDuct?.HosTagNumber is null ? "0" :Session.FromDuct.HosTagNumber ),
                new KeyValuePair<string, string>("tag_from_key", Session.FromDuct?.HostSiteKey is null ? "0" :Session.FromDuct.HostSiteKey ),
                new KeyValuePair<string, string>("duct_from", Session.FromDuct?.HosTagNumber is null ? "0" :Session.FromDuct.HosTagNumber ),
                new KeyValuePair<string, string>("duct_from_key", Session.FromDuct?.ConduitKey is null ? "0" :Session.FromDuct.ConduitKey ),
                new KeyValuePair<string, string>("cable_id1", Session.Cable1.AFRKey),
                new KeyValuePair<string, string>("cable_type", Session.Cable1.CableType),

                new KeyValuePair<string, string>("lattitude", Session.lattitude2),
                new KeyValuePair<string, string>("longitude", Session.longitude2),

                new KeyValuePair<string, string>("gps_offset_latitude", Session.lattitude_offset),
                new KeyValuePair<string, string>("gps_offset_longitude", Session.longitude_offset),

                new KeyValuePair<string, string>("gps_offset_bearing", Session.gps_offset_bearing),
                new KeyValuePair<string, string>("gps_offset_distance", Session.gps_offset_distance),

                new KeyValuePair<string, string>("comment", CommentText),

                new KeyValuePair<string, string>("site_type", SelectedSiteType?.IdLocatePoint is null ? "0" :SelectedSiteType.IdLocatePoint),


            };

            return keyValues;

        }
        public ObservableCollection<CodeLocatePoint> LocatePointType
        {
            get
            {

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CodeLocatePoint>();
                    Console.WriteLine();
                    var table = conn.Table<CodeLocatePoint>().ToList();
                    Console.WriteLine();
                    return new ObservableCollection<CodeLocatePoint>(table);
                }

            }
        }

        private async void ExecuteRecordCommand()
        {
            Console.WriteLine();
            if (string.IsNullOrEmpty(Session.gps_offset_bearing) && string.IsNullOrEmpty(Session.gps_offset_distance))
            {
                Console.WriteLine();
                Session.lattitude2 = location.Latitude.ToString();
                Session.longitude2 = location.Longitude.ToString();

                Session.lattitude_offset = string.Empty;
                Session.longitude_offset = string.Empty;
            }
            else // offset gps record 
            {
                Console.WriteLine();
                Session.lattitude_offset = location.Latitude.ToString();
                Session.longitude_offset = location.Longitude.ToString();
            }

            var KVPair = keyvaluepairLocate(); // update existed chassis
            var result = await CloudDBService.Insert_gps_point(KVPair);

            if (result.Equals("OK"))
            {

                LocPointNumber++;
                OnPropertyChanged(nameof(CurLocPoint)); // update Point number count
                await Application.Current.MainPage.DisplayAlert("DONE", "Gps point inserted", "OK");

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

        }

        public async void ExecuteFinishCommand()
        {
            RdGpstimer.Stop();
            await Application.Current.MainPage.Navigation.PopAsync();

        }
        public async void ExecuteCaptureCommand()
        {

            await Application.Current.MainPage.Navigation.PushAsync(new CameraViewPage());
        }
    }
}
