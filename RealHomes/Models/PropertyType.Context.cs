﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class RealHomesEntities : DbContext
    {
        public RealHomesEntities()
            : base("name=RealHomesEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<RHCategory> RHCategories { get; set; }
        public virtual DbSet<RHLocation> RHLocations { get; set; }
        public virtual DbSet<RHRegion> RHRegions { get; set; }
        public virtual DbSet<RHCity> RHCities { get; set; }
        public virtual DbSet<RHFacility> RHFacilities { get; set; }
        public virtual DbSet<RHFixture> RHFixtures { get; set; }
        public virtual DbSet<RHMetroStation> RHMetroStations { get; set; }
        public virtual DbSet<RHNearestMetroStation> RHNearestMetroStations { get; set; }
        public virtual DbSet<RHNearestSchool> RHNearestSchools { get; set; }
        public virtual DbSet<RHSchool> RHSchools { get; set; }
        public virtual DbSet<RHUnitFacility> RHUnitFacilities { get; set; }
        public virtual DbSet<RHUnitFixture> RHUnitFixtures { get; set; }
        public virtual DbSet<RHUnitView> RHUnitViews { get; set; }
        public virtual DbSet<RHView> RHViews { get; set; }
    }
}
