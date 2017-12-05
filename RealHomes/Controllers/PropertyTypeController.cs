using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RealHomes.Models;

using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Web.Mvc;


namespace RealHomes.Controllers
{
    public class PropertyTypeController : SurfaceController
    {
        private const string ID_COLUMN_NAME = "TypeId";
        private const string TABLE_NAME = "RHPropertyType";

        private UmbracoDatabase db = ApplicationContext.Current.DatabaseContext.Database;

        // GET: api/PropertyTypeModels
        public IEnumerable<PropertyTypeModel> GetAllPropertyTypes()
        {
            IEnumerable<PropertyTypeModel> propertyTypeModels = (IEnumerable<PropertyTypeModel>)db.Fetch<PropertyTypeModel>(new Sql().Select("*").From(TABLE_NAME));
            return propertyTypeModels;
        }

        // GET: PropertyType
        public PropertyTypeModel GetPropertyType(long id)
        {
            try
            {
                PropertyTypeModel propertyTypeModel = db.Fetch<PropertyTypeModel>(new Sql().Select("*").From(TABLE_NAME).Where<PropertyTypeModel>(x => x.TypeId == id, DatabaseContext.SqlSyntax)).FirstOrDefault();

                return propertyTypeModel;
            }
            catch(Exception ex)
            { 
}
            return null;
        }
    }
}