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
    public class FixturesController : SurfaceController
    {
        private const string ID_COLUMN_NAME = "";
        private const string TABLE_NAME = "RHFixturesAndFeatures";

        private UmbracoDatabase db = ApplicationContext.Current.DatabaseContext.Database;

        public IEnumerable<FixturesAndFeaturesModel> GetAllFixtures()
        {
            IEnumerable<FixturesAndFeaturesModel> fixturesModels = db.Fetch<FixturesAndFeaturesModel>(new Sql().Select("*").From(TABLE_NAME));

            return fixturesModels;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DoesFixtureExist(long id)
        {
            return false;
        }

        /// <summary>
        /// Get Loction Model of   particular Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FixturesAndFeaturesModel GetFixture(long id)
        {
            FixturesAndFeaturesModel fixtureModel = db.Fetch<FixturesAndFeaturesModel>(new Sql().Select("*").From(TABLE_NAME).Where<FixturesAndFeaturesModel>(x => x.FeatureId == id, DatabaseContext.SqlSyntax)).FirstOrDefault();

            return fixtureModel;
        }
    }
}