using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace RealHomes.Models
{
    [TableName("RHCity")]
    [PrimaryKey("CityId", autoIncrement = true)]
    public partial class CityModel
    {
        public long CityId { get; set; }
        public long RegionId { get; set; }
        public string CityName { get; set; }
    }
}