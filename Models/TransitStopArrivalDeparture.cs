using System;
using System.Linq;
using TrippitKiosk.Models.ApiModels;


namespace TrippitKiosk.Models
{
    public class TransitStopArrivalDeparture
    {
        public string LineShortName { get; set; }
        public string LineLongName { get; set; }
        public DateTimeOffset ScheduledArrival { get; set; }
        public DateTimeOffset RealtimeArrival { get; set; }
        public DateTimeOffset ScheduledDeparture { get; set; }
        public DateTimeOffset RealtimeDeparture { get; set; }
        public int ArrivalDelaySeconds { get; set; }
        public int DepartureDelaySeconds { get; set; }
        public bool IsRealtime { get; set; }
        public string Headsign { get; set; }

        public TransitStopArrivalDeparture(ApiStoptime backingStoptime)
        {
            LineShortName = backingStoptime.Trip.Route.ShortName;
            LineLongName = backingStoptime.Trip.Route.LongName;
            ScheduledArrival = HslTimeToDateTime(backingStoptime.ScheduledArrival.Value, backingStoptime.ServiceDay.Value);
            RealtimeArrival = HslTimeToDateTime(backingStoptime.RealtimeArrival.Value, backingStoptime.ServiceDay.Value);
            ScheduledDeparture = HslTimeToDateTime(backingStoptime.ScheduledDeparture.Value, backingStoptime.ServiceDay.Value);
            RealtimeDeparture = HslTimeToDateTime(backingStoptime.RealtimeDeparture.Value, backingStoptime.ServiceDay.Value);
            ArrivalDelaySeconds = backingStoptime.ArrivalDelay.Value;
            IsRealtime = backingStoptime.Realtime.Value;
            Headsign = backingStoptime.Headsign;
        }

        private DateTimeOffset HslTimeToDateTime(int secondsSinceMidnight, long dayUnixSeconds)
        {
            // Day is given in Unix seconds, except in the Finland time zone, rather than UTC
            // That means this date _should_ always be midnight, but will be either 2 or 3 hours shy.
            // Add a day, then zero out the time component to account for this
            // This works because the ServiceDay datetime is always at midnight.
            var day = DateTimeOffset.FromUnixTimeSeconds(dayUnixSeconds).AddDays(1);
            var dateTime = new DateTime(day.Year, day.Month, day.Day, 0, 0, 0);
            dateTime = dateTime.AddSeconds(secondsSinceMidnight);

            var finlandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");
            var offset = finlandTimeZone.GetUtcOffset(dateTime);

            return new DateTimeOffset(dateTime, offset);
        }
    }
}
