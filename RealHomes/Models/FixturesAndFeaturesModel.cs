using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Umbraco.Core.Persistence;

namespace RealHomes.Models
{
    [TableName("RHFixturesAndFeatures")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class FixturesAndFeaturesModel
    {
        [Column("Id")]
        [System.ComponentModel.DataAnnotations.KeyAttribute]
        public long FeatureId { get; set; }

        [Column("Name")]
        public string FeatureName { get; set; }

    }
}