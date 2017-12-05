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
    public class CategoryController : SurfaceController
    {
        private const string ID_COLUMN_NAME = "CatId";
        private const string TABLE_NAME = "RHCategory";

        //private RealHomesContext db = new RealHomesContext();
        private UmbracoDatabase db = ApplicationContext.Current.DatabaseContext.Database;

        // GET: api/CategoryModels
        public IEnumerable<CategoryModel> GetAllCategoryies()
        {
            IEnumerable<CategoryModel> CategoryModels = (IEnumerable<CategoryModel>)db.Fetch<CategoryModel>(new Sql().Select("*").From(TABLE_NAME));
            return CategoryModels;
        }

        // GET: Category
        public CategoryModel GetCategory(long id)
        {
            CategoryModel categoryModel = db.Fetch<CategoryModel>(new Sql().Select("*").From(TABLE_NAME).Where<CategoryModel>(x => x.CategoryId == id, DatabaseContext.SqlSyntax)).FirstOrDefault();

            return categoryModel;
        }
    }
}