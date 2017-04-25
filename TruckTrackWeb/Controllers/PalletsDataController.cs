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
    public class PalletsDataController : ApiController
    {
        private TruckTrackWebContext db = new TruckTrackWebContext();

        // GET: api/PalletsData
        public IQueryable<Pallet> GetPallets()
        {
            return db.Pallets;
        }

        // GET: api/PalletsData/5
        [ResponseType(typeof(Pallet))]
        public IHttpActionResult GetPallet(int id)
        {
            Pallet pallet = db.Pallets.Find(id);
            if (pallet == null)
            {
                return NotFound();
            }

            return Ok(pallet);
        }

        // PUT: api/PalletsData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPallet(int id, Pallet pallet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pallet.Id)
            {
                return BadRequest();
            }

            db.Entry(pallet).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                LogManager.Log("PUT: api/PalletsData/ - PalletId:" + pallet.Id.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PalletExists(id))
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

        // POST: api/PalletsData
        [ResponseType(typeof(Pallet))]
        public IHttpActionResult PostPallet(Pallet pallet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pallets.Add(pallet);
            db.SaveChanges();
            LogManager.Log("POST: api/PalletsData/ - PalletId:" + pallet.Id.ToString());

            return CreatedAtRoute("DefaultApi", new { id = pallet.Id }, pallet);
        }

        // DELETE: api/PalletsData/5
        [ResponseType(typeof(Pallet))]
        public IHttpActionResult DeletePallet(int id)
        {
            Pallet pallet = db.Pallets.Find(id);
            if (pallet == null)
            {
                return NotFound();
            }

            db.Pallets.Remove(pallet);
            db.SaveChanges();
            LogManager.Log("DELETE: api/PalletsData/ - PalletId:" + pallet.Id.ToString());

            return Ok(pallet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PalletExists(int id)
        {
            return db.Pallets.Count(e => e.Id == id) > 0;
        }
    }
}
