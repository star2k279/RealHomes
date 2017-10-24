using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealHomes.Models
{
    public partial class City
    {
        public long CityId { get; set; }
        public long RegionId { get; set; }
        public string CityName { get; set; }
    }
}