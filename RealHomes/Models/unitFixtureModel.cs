using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Persistence;

namespace RealHomes.Models
{
    public class UnitFixtureModel
    {
        [Column("FixId")]
        public long FixtureId { get; set; }

        [Column("PropId")]
        public long UnitId { get; set; }

        public FixturesAndFeaturesModel UnitFixtures;

        public PropertyViewModel Unit;

    }
}