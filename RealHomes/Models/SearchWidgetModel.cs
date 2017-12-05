
using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace RealHomes.Models
{
    public class SearchWidgetModel
    {
        public IEnumerable<LocationModel> Locations;

        public IEnumerable<CategoryModel> Categories;

        public IEnumerable<PropertyTypeModel> PropertyTypes;

        public IEnumerable<FixturesAndFeaturesModel> FixturesAndFeatures;

        public IEnumerable<ViewsModel> Views;

        public List<SelectListItem> Facilities;

        
    }
}