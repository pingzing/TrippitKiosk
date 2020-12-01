using static TrippitKiosk.Models.ApiModels.ApiEnums;

namespace TrippitKiosk.Models
{
    public class TransitLineWithoutStops : ITransitLine
    {
        public ApiMode TransitMode { get; set; }
        public string GtfsId { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
    }
}
