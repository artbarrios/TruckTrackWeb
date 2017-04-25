namespace TruckTrackWeb.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TruckTrackWeb.Models.TruckTrackWebContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "TruckTrackWeb.Models.TruckTrackWebContext";
        }

        protected override void Seed(TruckTrackWeb.Models.TruckTrackWebContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            // seed data for TruckTrackWeb

            if (context.StopEvents.Count() == 0)
            {
                context.StopEvents.AddOrUpdate(
                    s => s.Id,
                    new StopEvent { Id = 1, Title = "Associated Grocers Baton Rouge", StartDate = Convert.ToDateTime("4/25/2017"), StartTime = DateTime.Now, EndDate = DateTime.Now.AddHours(1), EndTime = DateTime.Now.AddHours(1), IsAllDay = false, ImageFilename = "/Content/Images/SampleImage1Small.jpg" },
                    new StopEvent { Id = 2, Title = "Allied Tires New Orleans", StartDate = Convert.ToDateTime("4/26/2017"), StartTime = DateTime.Now, EndDate = DateTime.Now.AddHours(1), EndTime = DateTime.Now.AddHours(1), IsAllDay = false, ImageFilename = "/Content/Images/SampleImage2Small.jpg" },
                    new StopEvent { Id = 3, Title = "Auto Zone Gonzales", StartDate = Convert.ToDateTime("4/27/2017"), StartTime = DateTime.Now, EndDate = DateTime.Now.AddHours(1), EndTime = DateTime.Now.AddHours(1), IsAllDay = false, ImageFilename = "/Content/Images/SampleImage3Small.jpg" }
                    );
                context.SaveChanges();
            }  // save seed data for StopEvent

            if (context.Drivers.Count() == 0)
            {
                context.Drivers.AddOrUpdate(
                    d => d.Id,
                    new Driver { Id = 1, Firstname = "John", Lastname = "Miller", Email = "texas.toast@email.com", HomePhone = "(888) 555-1111", CellPhone = "(888) 555-1111", WorkPhone = "(888) 555-1111", DateOfBirth = Convert.ToDateTime("1960-01-10") },
                    new Driver { Id = 2, Firstname = "Mary", Lastname = "Jones", Email = "rocket.man@email.com", HomePhone = "(877) 555-2222", CellPhone = "(877) 555-2222", WorkPhone = "(877) 555-2222", DateOfBirth = Convert.ToDateTime("1983-03-14") },
                    new Driver { Id = 3, Firstname = "Steve", Lastname = "Moore", Email = "cargo.ship@email.com", HomePhone = "(866) 555-3333", CellPhone = "(866) 555-3333", WorkPhone = "(866) 555-3333", DateOfBirth = Convert.ToDateTime("1972-09-21") }
                    );
                context.SaveChanges();
            }  // save seed data for Driver

            if (context.VehicleTypes.Count() == 0)
            {
                context.VehicleTypes.AddOrUpdate(
                    v => v.Id,
                    new VehicleType { Id = 1, Name = "Semi-Trailer Truck" },
                    new VehicleType { Id = 2, Name = "Half-Track" },
                    new VehicleType { Id = 3, Name = "Pickup" }
                    );
                context.SaveChanges();
            }  // save seed data for VehicleType

            if (context.Trucks.Count() == 0)
            {
                context.Trucks.AddOrUpdate(
                    t => t.Id,
                    new Truck { Id = 1, Name = "Big Bertha", TagNumber = "AZB-252", DatePurchased = DateTime.Now, VehicleTypeId = 1, StopEventFlowchartDiagramData = "StopEventFlowchartDiagramData1" },
                    new Truck { Id = 2, Name = "Iron Man", TagNumber = "lOG-49302", DatePurchased = DateTime.Now, VehicleTypeId = 2, StopEventFlowchartDiagramData = "StopEventFlowchartDiagramData2" },
                    new Truck { Id = 3, Name = "Enterprise", TagNumber = "0AF-342DF", DatePurchased = DateTime.Now, VehicleTypeId = 3, StopEventFlowchartDiagramData = "StopEventFlowchartDiagramData3" }
                    );
                context.SaveChanges();
            }  // save seed data for Truck

            if (context.Loads.Count() == 0)
            {
                context.Loads.AddOrUpdate(
                    l => l.Id,
                    new Load { Id = 1, Name = "Fresh Fruit", TruckId = 1, IsTimeSensitive = false },
                    new Load { Id = 2, Name = "Car Parts", TruckId = 2, IsTimeSensitive = false },
                    new Load { Id = 3, Name = "Used Tires", TruckId = 3, IsTimeSensitive = false }
                    );
                context.SaveChanges();
            }  // save seed data for Load

            if (context.Pallets.Count() == 0)
            {
                context.Pallets.AddOrUpdate(
                    p => p.Id,
                    new Pallet { Id = 1, Name = "Carrots", Value = 1234.00m, LoadId = 1 },
                    new Pallet { Id = 2, Name = "Goodyear", Value = 2993.42m, LoadId = 2 },
                    new Pallet { Id = 3, Name = "V8 Engine", Value = 123.12m, LoadId = 3 }
                    );
                context.SaveChanges();
            }  // save seed data for Pallet

            if (context.StopEventTrucks.Count() == 0)
            {
                context.StopEventTrucks.AddOrUpdate(
                    s => s.Id,
                    new StopEventTrucks { Id = 1, StopEventId = 1, TruckId = 1 },
                    new StopEventTrucks { Id = 2, StopEventId = 2, TruckId = 2 },
                    new StopEventTrucks { Id = 3, StopEventId = 3, TruckId = 3 }
                    );
                context.SaveChanges();
            }  // save seed data for StopEventTrucks


        }
    }
}
