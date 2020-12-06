using System;
using System.Linq;
using Windows.Networking;
using Windows.Networking.Connectivity;

namespace TrippitKiosk.Services
{
    public static class NetworkInfo
    {
        private const string NoInternetConnectionString = "Not connected";

        public static string WifiFourBarsIcon => "\uEC3F";
        public static string WifiThreeBarsIcon => "\uEC3E";
        public static string WifiTwoBarsIcon => "\uEC3D";
        public static string WifiOneBarIcon => "\uEC3C";
        public static string EthernetIcon => "\uE839";

        public static string GetCurrentNetworkName()
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();
            if (icp != null)
            {
                return icp.ProfileName;
            }

            return NoInternetConnectionString;
        }

        public static string GetCurrentIpv4Address()
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();
            if (icp != null && icp.NetworkAdapter != null && icp.NetworkAdapter.NetworkAdapterId != null)
            {
                var name = icp.ProfileName;
                try
                {
                    var hostnames = NetworkInformation.GetHostNames();
                    foreach (var hn in hostnames)
                    {
                        if (hn.IPInformation != null &&
                            hn.IPInformation.NetworkAdapter != null &&
                            hn.IPInformation.NetworkAdapter.NetworkAdapterId != null &&
                            hn.IPInformation.NetworkAdapter.NetworkAdapterId == icp.NetworkAdapter.NetworkAdapterId &&
                            hn.Type == HostNameType.Ipv4)
                        {
                            return hn.CanonicalName;
                        }
                    }
                }
                catch (Exception)
                {
                    // do nothing
                    // in some (strange) cases NetworkInformation.GetHostNames() fails... maybe a bug in the API...
                }
            }

            return NoInternetConnectionString;
        }

        /// <summary>
        /// Gets the WiFi network currently connected to. If not
        /// connected, or unable to retrieve information, returns null.
        public static ConnectionProfile GetCurrentWifiNetwork()
        {
            var connectionProfiles = NetworkInformation.GetConnectionProfiles();
            if (connectionProfiles.Count < 1)
            {
                return null;
            }

            var validProfiles = connectionProfiles.Where(profile =>
            {
                return (profile.IsWlanConnectionProfile && profile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.None);
            });

            if (validProfiles.Count() < 1)
            {
                return null;
            }

            return validProfiles.First() as ConnectionProfile;
        }
    }
}
