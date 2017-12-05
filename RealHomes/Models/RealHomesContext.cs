using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using Umbraco.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseModelDefinitions;

namespace RealHomes.Models
{
    public class RealHomesContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public RealHomesContext() : base("name=umbracoDbDSN")
        {
        }
        

        public System.Data.Entity.DbSet<RealHomes.Models.CategoryModel> CategoryModels { get; set; }

        public System.Data.Entity.DbSet<RealHomes.Models.PropertyDBModel> PropertyModels { get; set; }

        public System.Data.Entity.DbSet<RealHomes.Models.LocationModel> LocationModels { get; set; }
    }
}
