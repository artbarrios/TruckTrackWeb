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
    public class LoadsController : Controller
    {
        private TruckTrackWebContext db = new TruckTrackWebContext();

        // GET: AddPalletToLoad
        public ActionResult AddPalletToLoad(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Load load = db.Loads.Find(id);
            var palletsAvailable = db.Pallets.ToList().Except(load.Pallets.ToList()).ToList();
            ViewBag.PalletId = new SelectList(palletsAvailable, "Id", "Name");
            if (load == null)
            {
                return HttpNotFound();
            }
            LoadPalletViewModel viewModel = new LoadPalletViewModel();
            viewModel.LoadId = load.Id;
            viewModel.Load_Name = load.Name;
            return View(viewModel);
        }

        // POST: Loads/AddPalletToLoad/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPalletToLoad([Bind(Include = "LoadId,Load_Name,PalletId,Pallet_Name")] LoadPalletViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Load load = db.Loads.Find(viewModel.LoadId);
                Pallet pallet = db.Pallets.Find(viewModel.PalletId);
                load.Pallets.Add(pallet);
                db.Entry(load).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("Loads/AddPalletToLoad/ - PalletId:" + pallet.Id.ToString() + " to LoadId: " + load.Id.ToString());
                return RedirectToAction("Details", new { id = viewModel.LoadId });
            }
            return View(viewModel);
        }

        // GET: RemovePalletFromLoad
        // NOTE: the Pallet.LoadId property is not nullable so this routine must be omitted

        // GET: Loads
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/LoadsIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            var loads = db.Loads.Include(l => l.Truck);
            return View(loads.ToList());
        }

        // GET: Loads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Load load = db.Loads.Find(id);
            if (load == null)
            {
                return HttpNotFound();
            }
            return View(load);
        }

        // GET: Loads/Create
        public ActionResult Create(int? id, string modelType)
        {
            if (modelType != null && modelType.Length > 0)
            {
                ViewBag.TruckId = new SelectList(db.Trucks, "Id", "Name", modelType.Contains("Truck") ? id : -1);
            }
            else
            {
                ViewBag.TruckId = new SelectList(db.Trucks, "Id", "Name");
            }
            // modelType not always properly being passed as query string parameter so send it in ViewBag
            ViewBag.modelType = modelType;
            return View();
        }

        // POST: Loads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,TruckId,IsTimeSensitive")] Load load, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.Loads.Add(load);
                db.SaveChanges();
                LogManager.Log("Loads/Create - LoadId:" + load.Id.ToString());
                if (modelType != null && modelType.Length > 0)
                {
                    if (modelType.Contains("Truck"))
                    {
                        db.Trucks.Find(id).Loads.Add(load);
                        db.SaveChanges();
                        LogManager.Log("Loads/Create added - LoadId:" + load.Id.ToString() + " to TruckId: " + id.ToString());
                        controllerName = "Trucks";
                    }
                    return RedirectToAction("Details", controllerName, new { Id = id });
                }
                return RedirectToAction("Index");
            }
            ViewBag.TruckId = new SelectList(db.Trucks, "Id", "Name", load.TruckId);

            return View(load);
        }

        // GET: Loads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Load load = db.Loads.Find(id);
            if (load == null)
            {
                return HttpNotFound();
            }
            ViewBag.TruckId = new SelectList(db.Trucks, "Id", "Name", load.TruckId);

            return View(load);
        }

        // POST: Loads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,TruckId,IsTimeSensitive")] Load load)
        {
            if (ModelState.IsValid)
            {
                db.Entry(load).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("Loads/Edit/ - LoadId:" + load.Id.ToString());
                return RedirectToAction("Index");
            }
            ViewBag.TruckId = new SelectList(db.Trucks, "Id", "Name", load.TruckId);

            return View(load);
        }

        // GET: Loads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Load load = db.Loads.Find(id);
            if (load == null)
            {
                return HttpNotFound();
            }
            return View(load);
        }

        // POST: Loads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Load load = db.Loads.Find(id);
            db.Loads.Remove(load);
            db.SaveChanges();
            LogManager.Log("Loads/Delete/ - LoadId:" + load.Id.ToString());
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
