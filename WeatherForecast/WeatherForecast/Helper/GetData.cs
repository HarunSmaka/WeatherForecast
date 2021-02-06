using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace WeatherForecast.Helper
{
    public class GetData
    {
        public async Task<string> GetCity(Xamarin.Essentials.Location location)
        {
            var places = await Geocoding.GetPlacemarksAsync(location);
            var currentPlace = places?.FirstOrDefault();

            if (currentPlace != null)
            {
                return $"{currentPlace.Locality}, {currentPlace.CountryName}";
            }

            return null;
        }

        public async Task<bool> LocationExists(string Location)
        {
            bool exists = false;
            try
            {
                var locations = await Geocoding.GetLocationsAsync(Location);
                var location = locations?.FirstOrDefault();
                if (location != null)
                {
                    exists = true;
                }

            }
            catch (Exception)
            {

            }

            return exists;
        }

    }
}
