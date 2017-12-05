using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Persistence;
using Umbraco.Core;

namespace RealHomes.Models
{
    public class FacilitiesModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
/*
    public class ModelContext : DatabaseContext
    {
        public ModelContext() : base("name=umbracoDbDSN") { }
    }
    */
}