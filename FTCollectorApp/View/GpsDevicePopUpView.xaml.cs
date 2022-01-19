using FTCollectorApp.Model;
using FTCollectorApp.Service;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GpsDevicePopUpView
    {
        Location _location;

        public GpsDevicePopUpView()
        {
            InitializeComponent();
            //_location = loc;
        }

        protected override async void OnAppearing()
        {
            Session.manual_latti = String.Empty;
            Session.manual_longi = String.Empty;
            Session.lattitude2 = String.Empty;
            Session.longitude2 = String.Empty;

            await LocationService.GetLocation();
            if (LocationService.Coords != null)
            {
                _location = LocationService.Coords;
                txtAccuracy.Text = $"Accuracy is {String.Format("{0:0.###} m", _location.Accuracy.ToString())}";
                txtCoords.Text = $"Current Point is {String.Format("{0:0.#######}", _location.Latitude.ToString())} ,{String.Format("{0:0.#######}", _location.Longitude.ToString())} ";
                Session.gps_sts = "1";
            }
            else
            {
                Session.gps_sts = "0";

            }
            base.OnAppearing();
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void DeviceChecked(object sender, CheckedChangedEventArgs e)
        {
            _location = LocationService.Coords;
            if (_location == null)
            {
                await LocationService.GetLocation();
                

            }

            if (_location != null)
            {
                var accuracy = String.Format("{0:0.###} m", _location.Accuracy.ToString());
                var lattitude = String.Format("{0:0.#######}", _location.Latitude.ToString());
                var longitude = String.Format("{0:0.#######}", _location.Longitude.ToString());
                txtAccuracy.Text = $"Accuracy is {accuracy}";
                txtCoords.Text = $"Current Point is {lattitude} ,{longitude} ";
                Session.gps_sts = "1";
                Session.lattitude2 = lattitude;
                Session.longitude2 = longitude;
            }
        }

        private async void ExternalChecked(object sender, CheckedChangedEventArgs e)
        {
            _location = LocationService.Coords;
            if (_location == null)
            {
                await LocationService.GetLocation();


            }

            if (_location != null)
            {
                var accuracy = String.Format("{0:0.###} m", _location.Accuracy.ToString());
                var lattitude = String.Format("{0:0.#######}", _location.Latitude.ToString());
                var longitude = String.Format("{0:0.#######}", _location.Longitude.ToString());
                txtAccuracy.Text = $"Accuracy is {accuracy}";
                txtCoords.Text = $"Current Point is {lattitude} ,{longitude} ";
                Session.gps_sts = "1";
                Session.lattitude2 = lattitude;
                Session.longitude2 = longitude;
            }
        }

        private void NoGPSChecked(object sender, CheckedChangedEventArgs e)
        {

            Session.gps_sts = "0";
            Session.manual_latti = String.Format("{0:0.#######}", entryLat.Text);
            Session.manual_longi = String.Format("{0:0.#######}", entryLon.Text);

        }
    }
}