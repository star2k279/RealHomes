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
    public class ViewsController : SurfaceController
    {

        private const string ID_COLUMN_NAME = "";
        private const string TABLE_NAME = "RHViews";


        private UmbracoDatabase db = ApplicationContext.Current.DatabaseContext.Database;


        public IEnumerable<ViewsModel> GetAllViews()
        {
            IEnumerable<ViewsModel> locationModels = db.Fetch<ViewsModel>(new Sql().Select("*").From(TABLE_NAME));

            return locationModels;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DoesViewExist(long id)
        {
            return false;
        }

        /// <summary>
        /// Get Loction Model of   particular Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewsModel GetView(long id)
        {
            ViewsModel viewModel = db.Fetch<ViewsModel>(new Sql().Select("*").From(TABLE_NAME).Where<ViewsModel>(x => x.ViewId == id, DatabaseContext.SqlSyntax)).FirstOrDefault();

            return viewModel;
        }
    }
}