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
    public class VehicleTypesDataController : ApiController
    {
        private TruckTrackWebContext db = new TruckTrackWebContext();

        // GET: api/GetVehicleTypeTrucks/?VehicleTypeId=1
        [Route("api/GetVehicleTypeTrucks/")]
        public List<Truck> GetVehicleTypeTrucks(int VehicleTypeId)
        {
            VehicleType vehicleType = db.VehicleTypes.Find(VehicleTypeId);
            if (vehicleType == null)
            {
                return null;
            }
            return vehicleType.Trucks;
        }

        // PUT: api/AddTruckToVehicleType/?VehicleTypeId=1&TruckId=1
        [HttpPut]
        [Route("api/AddTruckToVehicleType/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddTruckToVehicleType(int VehicleTypeId, int TruckId)
        {
            VehicleType vehicleType = db.VehicleTypes.Find(VehicleTypeId);
            Truck truck = db.Trucks.Find(TruckId);
            if (vehicleType != null && truck != null)
            {
                try
                {
                    vehicleType.Trucks.Add(truck);
                    db.Entry(vehicleType).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/AddTruckToVehicleType - VehicleTypeId:" + vehicleType.Id.ToString() + " TruckId:" + truck.Id.ToString());
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
        } // AddTruckToVehicleType

        // PUT: api/RemoveTruckFromVehicleType/?VehicleTypeId=1&TruckId=1
        // NOTE: the Truck.VehicleTypeId property is not nullable so this routine must be omitted

        // GET: api/VehicleTypesData
        public IQueryable<VehicleType> GetVehicleTypes()
        {
            return db.VehicleTypes;
        }

        // GET: api/VehicleTypesData/5
        [ResponseType(typeof(VehicleType))]
        public IHttpActionResult GetVehicleType(int id)
        {
            VehicleType vehicleType = db.VehicleTypes.Find(id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            return Ok(vehicleType);
        }

        // PUT: api/VehicleTypesData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVehicleType(int id, VehicleType vehicleType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vehicleType.Id)
            {
                return BadRequest();
            }

            db.Entry(vehicleType).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                LogManager.Log("PUT: api/VehicleTypesData/ - VehicleTypeId:" + vehicleType.Id.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleTypeExists(id))
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

        // POST: api/VehicleTypesData
        [ResponseType(typeof(VehicleType))]
        public IHttpActionResult PostVehicleType(VehicleType vehicleType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VehicleTypes.Add(vehicleType);
            db.SaveChanges();
            LogManager.Log("POST: api/VehicleTypesData/ - VehicleTypeId:" + vehicleType.Id.ToString());

            return CreatedAtRoute("DefaultApi", new { id = vehicleType.Id }, vehicleType);
        }

        // DELETE: api/VehicleTypesData/5
        [ResponseType(typeof(VehicleType))]
        public IHttpActionResult DeleteVehicleType(int id)
        {
            VehicleType vehicleType = db.VehicleTypes.Find(id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            db.VehicleTypes.Remove(vehicleType);
            db.SaveChanges();
            LogManager.Log("DELETE: api/VehicleTypesData/ - VehicleTypeId:" + vehicleType.Id.ToString());

            return Ok(vehicleType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VehicleTypeExists(int id)
        {
            return db.VehicleTypes.Count(e => e.Id == id) > 0;
        }
    }
}
