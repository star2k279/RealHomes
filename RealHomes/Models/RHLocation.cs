//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RealHomes.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RHLocation
    {
        public long LocationId { get; set; }
        public long CityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    
        public virtual RHRegion RHRegion { get; set; }
    }
}
