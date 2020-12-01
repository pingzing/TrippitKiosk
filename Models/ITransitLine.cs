using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TrippitKiosk.Models.ApiModels.ApiEnums;

namespace TrippitKiosk.Models
{
    public interface ITransitLine
    {
        ApiMode TransitMode { get; set; }
        string GtfsId { get; set; }
        string ShortName { get; set; }
        string LongName { get; set; }
    }
}
