using CommunityToolkit.Mvvm.ComponentModel;
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

        [ObservableProperty]
        string curLocPoint = Session.GpsPointMaxIdx;

        public ICommand RecordCommand { get; set; }
        public ICommand OpenGPSOffsetPopupCommand { get; set; }

        public ICommand FinishCommand { get; set; }
        public ICommand CaptureCommand { get; set; }
        public LocatePointViewModel()
        {

            // Get accuracy
            location = LocationService.Coords;


            CaptureCommand = new Command(() => ExecuteCaptureCommand());
            FinishCommand = new Command(() => ExecuteFinishCommand());
            OpenGPSOffsetPopupCommand = new Command(() => ExecuteOpenGPSOffsetPopupCommand());
            RecordCommand = new Command(() => ExecuteRecordCommand());
        }
        List<KeyValuePair<string, string>> keyvaluepairLocate()
        {


            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new KeyValuePair<string, string>("jobnum", Session.jobnum),
                new KeyValuePair<string, string>("locate_point_number", Session.GpsPointMaxIdx),
                new KeyValuePair<string, string>("longitude", Session.longitude2),
                new KeyValuePair<string, string>("lattitude", Session.lattitude2),
                new KeyValuePair<string, string>("altitude", Session.altitude),
                new KeyValuePair<string, string>("lattitude_offset", Session.lattitude_offset),
                new KeyValuePair<string, string>("longitude_offset", Session.longitude_offset),
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
            Session.lattitude2 = location.Latitude.ToString();
            Session.longitude2 = location.Longitude.ToString();
            Session.altitude = location.Altitude.ToString();
            Session.accuracy = location.Accuracy.ToString();

            var KVPair = keyvaluepairLocate(); // update existed chassis
            var result = await CloudDBService.Insert_gps_point(KVPair);
        }

        private async void ExecuteOpenGPSOffsetPopupCommand()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new OffsetGPSPopUp());
        }

        public async void ExecuteFinishCommand()
        {
            await Application.Current.MainPage.Navigation.PopAsync();

        }
        public async void ExecuteCaptureCommand()
        {
            //await Application.Current.MainPage.Navigation.PopAsync();
            await Application.Current.MainPage.Navigation.PushAsync(new CameraViewPage());
        }
    }
}
