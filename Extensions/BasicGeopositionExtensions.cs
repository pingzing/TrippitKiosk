using Windows.Devices.Geolocation;

namespace TrippitKiosk.Extensions
{
    public static class BasicGeopositionExtensions
    {
        public static bool Equals(this BasicGeoposition geo, BasicGeoposition other)
        {
            return geo.Altitude == other.Altitude
                && geo.Latitude == other.Latitude
                && geo.Longitude == other.Longitude;
        }

        public static BasicGeoposition Create(double altitude, double latitude, double longitutde)
        {
            return new BasicGeoposition
            {
                Altitude = altitude,
                Latitude = latitude,
                Longitude = longitutde
            };
        }
    }
}
