using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model;
using FTCollectorApp.Service;
using FTCollectorApp.View.SitesPage;
using FTCollectorApp.View.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public partial class LocatePointViewModel : ObservableObject
    {
        [ObservableProperty]
        string selectedCodeLocatePoint;

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
        string comment;

        [ObservableProperty]
        string commentText;

        Location location;


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
                new KeyValuePair<string, string>("locate_point_number", Session.uid.ToString()),
                new KeyValuePair<string, string>("longitude", Session.RowId),

                new KeyValuePair<string, string>("lattitude", Session.lattitude2),
                new KeyValuePair<string, string>("altitude", Session.altitude),
                new KeyValuePair<string, string>("tag_from", ""),
                new KeyValuePair<string, string>("duct_from", ""),
                new KeyValuePair<string, string>("tag_to", ""),

                new KeyValuePair<string, string>("duct_to", ""),
                new KeyValuePair<string, string>("user_id", ""),

            };

            return keyValues;

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
