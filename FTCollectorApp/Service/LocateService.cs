using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FTCollectorApp.Service
{
    public static class LocateService
    {
        public static Location Coords;
        public static async Task GetLocation()
        {
            var status = await CheckAndRequestLocationPermission();
            if (status == PermissionStatus.Granted)
            {
                Coords = await Geolocation.GetLocationAsync();
            }
        }

        public static async Task<PermissionStatus> CheckAndRequestLocationPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // permission 
            }

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            return status;


        }
    }
}
