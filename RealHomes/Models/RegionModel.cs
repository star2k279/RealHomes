using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealHomes.Models
{
    public class RegionModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RegionModel()
        {
            this.LocationModel = new HashSet<LocationModel>();
        }

        public long Id { get; set; }
        public string RegionName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocationModel> LocationModel { get; set; }
    }
}