using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace RealHomes.Models
{
    public class LocationModel
    {

        public Int64 iLocationID { get; }


        public Int64 iCityId { get; set; }

        public string sLocationName { get; set; }



    }
}