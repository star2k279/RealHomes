using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;


namespace RealHomes.Models
{
    [TableName("RHProperty")]
    [PrimaryKey("PropId", autoIncrement =true)]
    public class PropertyModel
    {
         [Column("PropId")] 
         [PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1000)]
        public Int64 PropertyId	{get;}


        [Column("Title")]
        public string PropertyTitle	{get; set;}

        [Column("Refno")]
        public string ReferenceNo{get; set;}

        [Column("Category")]
        public Int64 PropertyCategory { get; set; }

        [Column("Type")]
        public Int64 PropertyType { get; set; }

        [Column("MainFeature")]
        public string MainFeature { get; set; }

        [Column("Location")]
        public Int64 Location { get; set; }

        [Column("TotalSize")]
        public Int64 TotalSize { get; set; }

        [Column("BuildUpArea")]
        public Int64 BuildUpArea { get; set; }

        [Column("ReraPermitNo")]
        public Int64 ReraPermitNo { get; set; }

        [Column("SellPrice")]
        public decimal SellPrice	 { get; set; }

        [Column("carparking")]
        public int carparking { get; set; }

        [Column("bedrooms")]
        public int bedrooms { get; set; }

        [Column("baths")]
        public int baths { get; set; }

        [Column("AvailabilityStatus")]
        public string AvailabilityStatus { get; set; }

        [Column("DevelopmentStatus")]
        public string DevelopmentStatus { get; set; }

        [Column("ServiceType")]
        public string ServiceType { get; set; }

        [Column("PropDetailPageURL")]
        public  string PropertyDetailPageURL { get; set; }
	

    }
}