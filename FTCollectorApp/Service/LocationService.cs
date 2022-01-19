using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FTCollectorApp.Service
{
    public static class LocationService
    {
        public static Location Coords;
        
        public static async Task GetLocation()
        {
            try
            {
                Coords = await Geolocation.GetLastKnownLocationAsync();

                if (Coords != null)
                {
                    Console.WriteLine($"Latitude: {Coords.Latitude}, Longitude: {Coords.Longitude}, Altitude: {Coords.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Console.WriteLine("Feature NotSupported Exception");

            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                Console.WriteLine("Feature NotEnabled Exception");

            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Console.WriteLine("Permission Exception");
            }
            catch (Exception ex)
            {
                // Unable to get location
                Console.WriteLine("Unable to get location");
            }
        }
    }

  
}
