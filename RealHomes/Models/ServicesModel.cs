using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Persistence;

namespace RealHomes.Models
{
    [TableName("RHServices")]
    [PrimaryKey("ServiceId", autoIncrement = true)]
    public class ServicesModel
    {
        [Column("ServiceId")]
        public long ServiceId { get; set; }

        [Column("ServiceName")]
        public string ServiceName { get; set; }

       
    }
}