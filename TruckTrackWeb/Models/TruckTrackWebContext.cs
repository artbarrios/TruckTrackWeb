using TruckTrackWeb.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace TruckTrackWeb.Models
{
    public class TruckTrackWebContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public TruckTrackWebContext() : base("name=TruckTrackWebContext")
        {
            Database.SetInitializer<TruckTrackWebContext>(new MigrateDatabaseToLatestVersion<TruckTrackWebContext, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }


        public System.Data.Entity.DbSet<TruckTrackWeb.Models.Truck> Trucks { get; set; }

        public System.Data.Entity.DbSet<TruckTrackWeb.Models.StopEvent> StopEvents { get; set; }

        public System.Data.Entity.DbSet<TruckTrackWeb.Models.Driver> Drivers { get; set; }

        public System.Data.Entity.DbSet<TruckTrackWeb.Models.Load> Loads { get; set; }

        public System.Data.Entity.DbSet<TruckTrackWeb.Models.Pallet> Pallets { get; set; }

        public System.Data.Entity.DbSet<TruckTrackWeb.Models.VehicleType> VehicleTypes { get; set; }

        public System.Data.Entity.DbSet<TruckTrackWeb.Models.StopEventTrucks> StopEventTrucks { get; set; }
    } // public class TruckTrackWebContext : DbContext
}
