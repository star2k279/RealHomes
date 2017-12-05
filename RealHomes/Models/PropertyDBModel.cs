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
    [PrimaryKey("PropId",autoIncrement =true)]
    public class PropertyDBModel
    {
        [PrimaryKeyColumn(AutoIncrement = true)]
        [System.ComponentModel.DataAnnotations.KeyAttribute]
        public long PropId { get; set; }


        [Column("Title")]
        public string PropertyTitle { get; set; }

        [Column("Refno")]
        public string ReferenceNo { get; set; }

        [Column("CategoryId")]
        public long CategoryId { get; set; }

        [Column("TypeId")]
        public long TypeId { get; set; }

        [Column("Address")]
        public string Address { get; set; }

        [Column("LocationId")]
        public long LocationId { get; set; }

        [Column("ServiceId")]
        public long ServiceId { get; set; }

        [Column("MainFeature")]
        public string MainFeature { get; set; }

        [Column("TotalSize")]
        public long TotalSize { get; set; }

        [Column("BuildUpArea")]
        public long BuildUpArea { get; set; }

        [Column("ReraPermitNo")]
        public long ReraPermitNo { get; set; }

        [Column("SellPrice")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public long SellPrice { get; set; }

        [Column("carparking")]
        public short carparking { get; set; }

        [Column("bedrooms")]
        public short bedrooms { get; set; }

        [Column("baths")]
        public short baths { get; set; }

        [Column("AvailabilityStatus")]
        public int AvailabilityStatus { get; set; }

        [Column("DevelopmentStatus")]
        public int DevelopmentStatus { get; set; }

        [Column("PropDetailPageId")]
        public string PropertyDetailPageID { get; set; }

        [Column("ImagePath")]
        public string PropertyImagePath { get; set; }

        [Column("OwnerId")]
        public long PropertyOwnerId { get; set; }

        [Column("UserId")]
        public long UserId { get; set; }

        [Column("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        



    }
}