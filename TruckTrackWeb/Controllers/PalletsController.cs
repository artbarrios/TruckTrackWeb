using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Configuration;
using TruckTrackWeb.Models;

namespace TruckTrackWeb.Controllers
{
    public class PalletsController : Controller
    {
        private TruckTrackWebContext db = new TruckTrackWebContext();

        // GET: Pallets
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/PalletsIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            var pallets = db.Pallets.Include(p => p.Load);
            return View(pallets.ToList());
        }

        // GET: Pallets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pallet pallet = db.Pallets.Find(id);
            if (pallet == null)
            {
                return HttpNotFound();
            }
            return View(pallet);
        }

        // GET: Pallets/Create
        public ActionResult Create(int? id, string modelType)
        {
            if (modelType != null && modelType.Length > 0)
            {
                ViewBag.LoadId = new SelectList(db.Loads, "Id", "Name", modelType.Contains("Load") ? id : -1);
            }
            else
            {
                ViewBag.LoadId = new SelectList(db.Loads, "Id", "Name");
            }
            // modelType not always properly being passed as query string parameter so send it in ViewBag
            ViewBag.modelType = modelType;
            return View();
        }

        // POST: Pallets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Value,LoadId")] Pallet pallet, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.Pallets.Add(pallet);
                db.SaveChanges();
                LogManager.Log("Pallets/Create - PalletId:" + pallet.Id.ToString());
                if (modelType != null && modelType.Length > 0)
                {
                    if (modelType.Contains("Load"))
                    {
                        db.Loads.Find(id).Pallets.Add(pallet);
                        db.SaveChanges();
                        LogManager.Log("Pallets/Create added - PalletId:" + pallet.Id.ToString() + " to LoadId: " + id.ToString());
                        controllerName = "Loads";
                    }
                    return RedirectToAction("Details", controllerName, new { Id = id });
                }
                return RedirectToAction("Index");
            }
            ViewBag.LoadId = new SelectList(db.Loads, "Id", "Name", pallet.LoadId);

            return View(pallet);
        }

        // GET: Pallets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pallet pallet = db.Pallets.Find(id);
            if (pallet == null)
            {
                return HttpNotFound();
            }
            ViewBag.LoadId = new SelectList(db.Loads, "Id", "Name", pallet.LoadId);

            return View(pallet);
        }

        // POST: Pallets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Value,LoadId")] Pallet pallet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pallet).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("Pallets/Edit/ - PalletId:" + pallet.Id.ToString());
                return RedirectToAction("Index");
            }
            ViewBag.LoadId = new SelectList(db.Loads, "Id", "Name", pallet.LoadId);

            return View(pallet);
        }

        // GET: Pallets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pallet pallet = db.Pallets.Find(id);
            if (pallet == null)
            {
                return HttpNotFound();
            }
            return View(pallet);
        }

        // POST: Pallets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pallet pallet = db.Pallets.Find(id);
            db.Pallets.Remove(pallet);
            db.SaveChanges();
            LogManager.Log("Pallets/Delete/ - PalletId:" + pallet.Id.ToString());
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
