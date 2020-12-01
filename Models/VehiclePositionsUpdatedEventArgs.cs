using System.Collections.Generic;

namespace TrippitKiosk.Models
{
    public class VehiclePositionsUpdatedEventArgs
    {
        public List<VehiclePosition> Positions { get; set; }
    }
}
