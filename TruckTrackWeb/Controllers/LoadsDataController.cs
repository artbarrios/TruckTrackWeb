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
    public class LoadsDataController : ApiController
    {
        private TruckTrackWebContext db = new TruckTrackWebContext();

        // GET: api/GetLoadPallets/?LoadId=1
        [Route("api/GetLoadPallets/")]
        public List<Pallet> GetLoadPallets(int LoadId)
        {
            Load load = db.Loads.Find(LoadId);
            if (load == null)
            {
                return null;
            }
            return load.Pallets;
        }

        // PUT: api/AddPalletToLoad/?LoadId=1&PalletId=1
        [HttpPut]
        [Route("api/AddPalletToLoad/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddPalletToLoad(int LoadId, int PalletId)
        {
            Load load = db.Loads.Find(LoadId);
            Pallet pallet = db.Pallets.Find(PalletId);
            if (load != null && pallet != null)
            {
                try
                {
                    load.Pallets.Add(pallet);
                    db.Entry(load).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/AddPalletToLoad - LoadId:" + load.Id.ToString() + " PalletId:" + pallet.Id.ToString());
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
        } // AddPalletToLoad

        // PUT: api/RemovePalletFromLoad/?LoadId=1&PalletId=1
        // NOTE: the Pallet.LoadId property is not nullable so this routine must be omitted

        // GET: api/LoadsData
        public IQueryable<Load> GetLoads()
        {
            return db.Loads;
        }

        // GET: api/LoadsData/5
        [ResponseType(typeof(Load))]
        public IHttpActionResult GetLoad(int id)
        {
            Load load = db.Loads.Find(id);
            if (load == null)
            {
                return NotFound();
            }

            return Ok(load);
        }

        // PUT: api/LoadsData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLoad(int id, Load load)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != load.Id)
            {
                return BadRequest();
            }

            db.Entry(load).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                LogManager.Log("PUT: api/LoadsData/ - LoadId:" + load.Id.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoadExists(id))
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

        // POST: api/LoadsData
        [ResponseType(typeof(Load))]
        public IHttpActionResult PostLoad(Load load)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Loads.Add(load);
            db.SaveChanges();
            LogManager.Log("POST: api/LoadsData/ - LoadId:" + load.Id.ToString());

            return CreatedAtRoute("DefaultApi", new { id = load.Id }, load);
        }

        // DELETE: api/LoadsData/5
        [ResponseType(typeof(Load))]
        public IHttpActionResult DeleteLoad(int id)
        {
            Load load = db.Loads.Find(id);
            if (load == null)
            {
                return NotFound();
            }

            db.Loads.Remove(load);
            db.SaveChanges();
            LogManager.Log("DELETE: api/LoadsData/ - LoadId:" + load.Id.ToString());

            return Ok(load);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LoadExists(int id)
        {
            return db.Loads.Count(e => e.Id == id) > 0;
        }
    }
}
