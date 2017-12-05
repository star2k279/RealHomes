using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RealHomes.Models;


using System.Configuration;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseModelDefinitions;


namespace RealHomes.Controllers
{
    public class CategoryApiController : UmbracoApiController
    {
        private const string ID_COLUMN_NAME= "CatId";
        private const string TABLE_NAME = "RHCategory";

        //private RealHomesContext db = new RealHomesContext();
        private UmbracoDatabase db = ApplicationContext.Current.DatabaseContext.Database;
        
        // GET: api/CategoryModels
        public IQueryable<CategoryModel> GetCategoryModels()
        {
            IQueryable<CategoryModel> CategoryModels = (IQueryable<CategoryModel>) db.Fetch<CategoryModel>(new Sql().Select("*").From(TABLE_NAME));
            return CategoryModels;
        }

        // GET: api/CategoryModels/5
        [ResponseType(typeof(CategoryModel))]
        public IHttpActionResult GetCategory(long id)
        {
            var query = new Sql().Select("*").From(TABLE_NAME).Where<CategoryModel>(x => x.CategoryId == id, DatabaseContext.SqlSyntax);
            
            CategoryModel categoryModel =  (CategoryModel)db.Fetch<CategoryModel>(query).FirstOrDefault();

            if (categoryModel == null)
            {
                return NotFound();
               // return 
            }

            return Ok(categoryModel);
        }

        // PUT: api/CategoryModels/5
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateCategoryModel(long id, CategoryModel categoryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != categoryModel.CategoryId)
            {
                return BadRequest();
            }

            //db.Entry(categoryModel).State = EntityState.Modified;
            

            try
            {
                db.Update(categoryModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CategoryModels
        [ResponseType(typeof(CategoryModel))]
        public IHttpActionResult PostCategoryModel(CategoryModel categoryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
                
            }

            //db.CategoryModels.Add(categoryModel);
            //db.SaveChanges();
            db.Save(categoryModel);

            return CreatedAtRoute("DefaultApi", new { id = categoryModel.CategoryId }, categoryModel);

        }

        // DELETE: api/CategoryModels/5
        [ResponseType(typeof(CategoryModel))]
        public IHttpActionResult DeleteCategoryModel(long id)
        {

            var query = new Sql().Select("*").From(TABLE_NAME).Where<CategoryModel>(x => x.CategoryId == id, DatabaseContext.SqlSyntax);

            CategoryModel categoryModel = (CategoryModel)db.Fetch<CategoryModel>(query).FirstOrDefault();


            if (categoryModel == null)
            {
                return NotFound();
            }

            db.Delete(categoryModel);
            
            return Ok(categoryModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryModelExists(long id)
        {
            var query = new Sql().Select("*").From(TABLE_NAME).Where<CategoryModel>(x => x.CategoryId == id,null);

            CategoryModel categoryModel = (CategoryModel)db.Fetch<CategoryModel>(query).FirstOrDefault();

            return (categoryModel != null);

           
        }


        public IHttpActionResult RenderForm()
        {
            return null;

        }


        public IHttpActionResult SubmitForm()
        {
            return BadRequest("Not a Good Idea.");
        }

    }
}