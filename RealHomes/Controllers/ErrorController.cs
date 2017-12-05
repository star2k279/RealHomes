using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealHomes.Models;

using Umbraco.Core;
using Umbraco.Web.Mvc;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseModelDefinitions;


namespace RealHomes.Controllers
{
    public class ErrorController : SurfaceController
    {
        private const string ID_COLUMN_NAME = "errorId";
        private const string TABLE_NAME = "RHApplicationErrors";

        //private RealHomesContext db = new RealHomesContext();
        private UmbracoDatabase db = ApplicationContext.Current.DatabaseContext.Database;

        // GET: api/CategoryModels
        public IEnumerable<ApplicationErrorModel> GetAllCategoryies()
        {
            IEnumerable<ApplicationErrorModel> errorModels = (IEnumerable<ApplicationErrorModel>)db.Fetch<ApplicationErrorModel>(new Sql().Select("*").From(TABLE_NAME));
            return errorModels;
        }

        // GET: Category
        public ApplicationErrorModel GetError(long id)
        {
            ApplicationErrorModel errorModel = db.Fetch<ApplicationErrorModel>(new Sql().Select("*").From(TABLE_NAME).Where<ApplicationErrorModel>(x => x.ErrorId == id, DatabaseContext.SqlSyntax)).FirstOrDefault();

            return errorModel;
        }

        public bool LogErrorinDb(Exception ex)
        {
            ApplicationErrorModel err = new ApplicationErrorModel();
            err.Message =  ex.InnerException.Message;
            err.OccuredOn = System.DateTime.Now;
            err.MachineID = "test";
            try
            {
                db.Save(err);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}