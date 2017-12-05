using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace RealHomes.Models
{
    [TableName("RHLocation")]
    [PrimaryKey("LocationID", autoIncrement = true)]
    public class LocationModel
    {
        [Column("LocationId")]
        [KeyAttribute]
        public long LocationID { get; set; }

        [Column("CityId")]
        public long CityId { get; set; }

        [Column("Name")]
        public string LocationName { get; set; }

        [Column("Description")]
        public string Description { get; set; }


    }
}