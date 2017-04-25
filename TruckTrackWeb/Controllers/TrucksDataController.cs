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
using TruckTrackWeb.Models;

namespace TruckTrackWeb.Controllers
{
    public class TrucksDataController : ApiController
    {
        private TruckTrackWebContext db = new TruckTrackWebContext();

        // GET: api/GetTruckLoads/?TruckId=1
        [Route("api/GetTruckLoads/")]
        public List<Load> GetTruckLoads(int TruckId)
        {
            Truck truck = db.Trucks.Find(TruckId);
            if (truck == null)
            {
                return null;
            }
            return truck.Loads;
        }

        // GET: api/GetTruckStopEvents/?TruckId=1
        [Route("api/GetTruckStopEvents/")]
        public List<StopEvent> GetTruckStopEvents(int TruckId)
        {
            Truck truck = db.Trucks.Find(TruckId);
            if (truck == null)
            {
                return null;
            }
            return truck.StopEvents;
        }

        // PUT: api/AddLoadToTruck/?TruckId=1&LoadId=1
        [HttpPut]
        [Route("api/AddLoadToTruck/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddLoadToTruck(int TruckId, int LoadId)
        {
            Truck truck = db.Trucks.Find(TruckId);
            Load load = db.Loads.Find(LoadId);
            if (truck != null && load != null)
            {
                try
                {
                    truck.Loads.Add(load);
                    db.Entry(truck).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/AddLoadToTruck - TruckId:" + truck.Id.ToString() + " LoadId:" + load.Id.ToString());
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                return NotFound();
            }
            return StatusCode(HttpStatusCode.NoContent);
        } // AddLoadToTruck

        // PUT: api/AddStopEventToTruck/?TruckId=1&StopEventId=1
        [HttpPut]
        [Route("api/AddStopEventToTruck/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddStopEventToTruck(int TruckId, int StopEventId)
        {
            Truck truck = db.Trucks.Find(TruckId);
            StopEvent stopEvent = db.StopEvents.Find(StopEventId);
            if (truck != null && stopEvent != null)
            {
                try
                {
                    truck.StopEvents.Add(stopEvent);
                    db.Entry(truck).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/AddStopEventToTruck - TruckId:" + truck.Id.ToString() + " StopEventId:" + stopEvent.Id.ToString());
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                return NotFound();
            }
            return StatusCode(HttpStatusCode.NoContent);
        } // AddStopEventToTruck

        // PUT: api/RemoveLoadFromTruck/?TruckId=1&LoadId=1
        // NOTE: the Load.TruckId property is not nullable so this routine must be omitted

        // PUT: api/RemoveStopEventFromTruck/?TruckId=1&StopEventId=1
        [HttpPut]
        [Route("api/RemoveStopEventFromTruck/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult RemoveStopEventFromTruck(int TruckId, int StopEventId)
        {
            Truck truck = db.Trucks.Find(TruckId);
            StopEvent stopEvent = db.StopEvents.Find(StopEventId);
            if (truck != null && stopEvent != null)
            {
                try
                {
                    truck.StopEvents.Remove(stopEvent);
                    db.Entry(truck).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/RemoveStopEventToTruck - TruckId:" + truck.Id.ToString() + " StopEventId:" + stopEvent.Id.ToString());
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                return NotFound();
            }
            return StatusCode(HttpStatusCode.NoContent);
        } // RemoveStopEventFromTruck

        // GET: api/TrucksData
        public IQueryable<Truck> GetTrucks()
        {
            return db.Trucks;
        }

        // GET: api/TrucksData/5
        [ResponseType(typeof(Truck))]
        public IHttpActionResult GetTruck(int id)
        {
            Truck truck = db.Trucks.Find(id);
            if (truck == null)
            {
                return NotFound();
            }

            return Ok(truck);
        }

        // PUT: api/TrucksData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTruck(int id, Truck truck)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != truck.Id)
            {
                return BadRequest();
            }

            db.Entry(truck).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                LogManager.Log("PUT: api/TrucksData/ - TruckId:" + truck.Id.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TruckExists(id))
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

        // POST: api/TrucksData
        [ResponseType(typeof(Truck))]
        public IHttpActionResult PostTruck(Truck truck)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Trucks.Add(truck);
            db.SaveChanges();
            LogManager.Log("POST: api/TrucksData/ - TruckId:" + truck.Id.ToString());

            return CreatedAtRoute("DefaultApi", new { id = truck.Id }, truck);
        }

        // DELETE: api/TrucksData/5
        [ResponseType(typeof(Truck))]
        public IHttpActionResult DeleteTruck(int id)
        {
            Truck truck = db.Trucks.Find(id);
            if (truck == null)
            {
                return NotFound();
            }

            db.Trucks.Remove(truck);
            db.SaveChanges();
            LogManager.Log("DELETE: api/TrucksData/ - TruckId:" + truck.Id.ToString());

            return Ok(truck);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TruckExists(int id)
        {
            return db.Trucks.Count(e => e.Id == id) > 0;
        }
    }
}
