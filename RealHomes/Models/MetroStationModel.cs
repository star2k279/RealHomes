using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealHomes.Models
{
    public class MetroStationModel
    {
        public long Id { get; set; }
        public long LocationId { get; set; }
        public string StationName { get; set; }
    }
}