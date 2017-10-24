using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealHomes.Models
{
    public class NearestSchoolModel
    {
        public long Id { get; set; }
        public long PropId { get; set; }
        public Nullable<decimal> Distance { get; set; }
    }
}