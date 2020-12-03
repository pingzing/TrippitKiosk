using System;
using TrippitKiosk.Models.ApiModels;


namespace TrippitKiosk.Models
{
    public class TransitStopArrivalDeparture
    {
        private const int SecondsPerDay = 86400;

        public string LineShortName { get; set; }
        public string LineLongName { get; set; }
        public DateTime ScheduledArrival { get; set; }
        public DateTime RealtimeArrival { get; set; }
        public DateTime ScheduledDeparture { get; set; }
        public DateTime RealtimeDeparture { get; set; }
        public int ArrivalDelaySeconds { get; set; }
        public int DepartureDelaySeconds { get; set; }
        public bool IsRealtime { get; set; }
        public DateTime ServiceDay { get; set; }
        public string Headsign { get; set; }

        public TransitStopArrivalDeparture(ApiStoptime backingStoptime)
        {
            LineShortName = backingStoptime.Trip.Route.ShortName;
            LineLongName = backingStoptime.Trip.Route.LongName;
            ScheduledArrival = HslTimeToDateTime(backingStoptime.ScheduledArrival.Value);
            RealtimeArrival = HslTimeToDateTime(backingStoptime.RealtimeArrival.Value);
            ScheduledDeparture = HslTimeToDateTime(backingStoptime.ScheduledDeparture.Value);
            RealtimeDeparture = HslTimeToDateTime(backingStoptime.RealtimeDeparture.Value);
            ArrivalDelaySeconds = backingStoptime.ArrivalDelay.Value;
            IsRealtime = backingStoptime.Realtime.Value;

            // Note: This will be subtly wrong if this device ever leaves Finland. ServiceDay is a Unix
            // timestamp in LOCAL time. wtf
            ServiceDay = DateTimeOffset.FromUnixTimeSeconds(backingStoptime.ServiceDay.Value).DateTime;

            Headsign = backingStoptime.Headsign;
        }

        private DateTime HslTimeToDateTime(int hslTime)
        {
            if (hslTime > SecondsPerDay)
            {
                return DateTime.Today.AddDays(-1).AddSeconds(hslTime);
            }
            else
            {
                return DateTime.Today.AddSeconds(hslTime);
            }
        }
    }
}
