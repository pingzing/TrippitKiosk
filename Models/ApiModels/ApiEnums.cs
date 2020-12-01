﻿using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiEnums
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ApiVertexType
        {
            [EnumMember(Value = "NORMAL")]
            Normal,
            [EnumMember(Value = "TRANSIT")]
            Transit,
            [EnumMember(Value = "BIKEPARK")]
            BikePark,
            [EnumMember(Value = "BIKESHARE")]
            BikeShare
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ApiWheelchairBoarding
        {
            [EnumMember(Value = "NO_INFORMATION")]
            NoInformation,
            [EnumMember(Value = "POSSIBLE")]
            Possible,
            [EnumMember(Value = "NOT_POSSIBLE")]
            NotPossible
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ApiLocationType
        {
            [EnumMember(Value = "STOP")]
            Stop,
            [EnumMember(Value = "STATION")]
            Station,
            [EnumMember(Value = "ENTRANCE")]
            Entrance
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ApiRealtimeState
        {
            [EnumMember(Value = "SCHEDULED")]
            Scheduled,
            [EnumMember(Value = "UPDATED")]
            Updated,
            [EnumMember(Value = "CANCELED")]
            Canceled,
            [EnumMember(Value = "ADDED")]
            Added,
            [EnumMember(Value = "MODIFIED")]
            Modified
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ApiPickupDropoffType
        {
            [EnumMember(Value = "SCHEDULED")]
            Scheduled,
            [EnumMember(Value = "NONE")]
            None,
            [EnumMember(Value = "CALL_AGENCY")]
            CallAgency,
            [EnumMember(Value = "COORDINATE_WITH_DRIVER")]
            CoordinateWithDriver
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ApiBikesAllowed
        {
            [EnumMember(Value = "NO_INFORMATION")]
            NoInformation,
            [EnumMember(Value = "ALLOWED")]
            Allowed,
            [EnumMember(Value = "NOT_ALLOWED")]
            NotAllowed
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ApiMode
        {
            [EnumMember(Value = "AIRPLANE")]
            Airplane,
            [EnumMember(Value = "BICYCLE")]
            Bicycle,
            [EnumMember(Value = "BUS")]
            Bus,
            [EnumMember(Value = "BUSISH")]
            Busish, ///wtf is "busish"?
            [EnumMember(Value = "CABLE_CAR")]
            CableCar,
            [EnumMember(Value = "CAR")]
            Car,
            [EnumMember(Value = "FERRY")]
            Ferry,
            [EnumMember(Value = "FUNICULAR")]
            Funicular,
            [EnumMember(Value = "GONDOLA")]
            Gondola,
            [EnumMember(Value = "LEG_SWITCH")]
            LegSwitch,
            [EnumMember(Value = "RAIL")]
            Rail,
            [EnumMember(Value = "SUBWAY")]
            Subway,
            [EnumMember(Value = "TRAINISH")]
            Trainish,
            [EnumMember(Value = "TRAM")]
            Tram,
            [EnumMember(Value = "TRANSIT")]
            Transit,
            [EnumMember(Value = "WALK")]
            Walk
        }
    }
}
