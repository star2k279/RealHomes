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
    public class ServiceController : SurfaceController
    {
        private const string ID_COLUMN_NAME = "ServiceId";
        private const string TABLE_NAME = "RHServices";

        //private RealHomesContext db = new RealHomesContext();
        private UmbracoDatabase db = ApplicationContext.Current.DatabaseContext.Database;

        // GET: api/CategoryModels
        public IEnumerable<ServicesModel> GetAllServices()
        {
            IEnumerable<ServicesModel> ServiceModels = (IEnumerable<ServicesModel>)db.Fetch<ServicesModel>(new Sql().Select("*").From(TABLE_NAME));
            return ServiceModels;
        }

        // GET: Category
        public ServicesModel GetService(long id)
        {
            ServicesModel serviceModel = db.Fetch<ServicesModel>(new Sql().Select("*").From(TABLE_NAME).Where<ServicesModel>(x => x.ServiceId == id, DatabaseContext.SqlSyntax)).FirstOrDefault();

            return serviceModel;
        }
    }
}