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
    public class VehicleTypesController : Controller
    {
        private TruckTrackWebContext db = new TruckTrackWebContext();

        // GET: AddTruckToVehicleType
        public ActionResult AddTruckToVehicleType(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleType vehicleType = db.VehicleTypes.Find(id);
            var trucksAvailable = db.Trucks.ToList().Except(vehicleType.Trucks.ToList()).ToList();
            ViewBag.TruckId = new SelectList(trucksAvailable, "Id", "Name");
            if (vehicleType == null)
            {
                return HttpNotFound();
            }
            VehicleTypeTruckViewModel viewModel = new VehicleTypeTruckViewModel();
            viewModel.VehicleTypeId = vehicleType.Id;
            viewModel.VehicleType_Name = vehicleType.Name;
            return View(viewModel);
        }

        // POST: VehicleTypes/AddTruckToVehicleType/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTruckToVehicleType([Bind(Include = "VehicleTypeId,VehicleType_Name,TruckId,Truck_Name")] VehicleTypeTruckViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                VehicleType vehicleType = db.VehicleTypes.Find(viewModel.VehicleTypeId);
                Truck truck = db.Trucks.Find(viewModel.TruckId);
                vehicleType.Trucks.Add(truck);
                db.Entry(vehicleType).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("VehicleTypes/AddTruckToVehicleType/ - TruckId:" + truck.Id.ToString() + " to VehicleTypeId: " + vehicleType.Id.ToString());
                return RedirectToAction("Details", new { id = viewModel.VehicleTypeId });
            }
            return View(viewModel);
        }

        // GET: RemoveTruckFromVehicleType
        // NOTE: the Truck.VehicleTypeId property is not nullable so this routine must be omitted

        // GET: VehicleTypes
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/VehicleTypesIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            return View(db.VehicleTypes.ToList());
        }

        // GET: VehicleTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleType vehicleType = db.VehicleTypes.Find(id);
            if (vehicleType == null)
            {
                return HttpNotFound();
            }
            return View(vehicleType);
        }

        // GET: VehicleTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VehicleTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] VehicleType vehicleType, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.VehicleTypes.Add(vehicleType);
                db.SaveChanges();
                LogManager.Log("VehicleTypes/Create - VehicleTypeId:" + vehicleType.Id.ToString());
                return RedirectToAction("Index");
            }

            return View(vehicleType);
        }

        // GET: VehicleTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleType vehicleType = db.VehicleTypes.Find(id);
            if (vehicleType == null)
            {
                return HttpNotFound();
            }
            return View(vehicleType);
        }

        // POST: VehicleTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] VehicleType vehicleType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicleType).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("VehicleTypes/Edit/ - VehicleTypeId:" + vehicleType.Id.ToString());
                return RedirectToAction("Index");
            }
            return View(vehicleType);
        }

        // GET: VehicleTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleType vehicleType = db.VehicleTypes.Find(id);
            if (vehicleType == null)
            {
                return HttpNotFound();
            }
            return View(vehicleType);
        }

        // POST: VehicleTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VehicleType vehicleType = db.VehicleTypes.Find(id);
            db.VehicleTypes.Remove(vehicleType);
            db.SaveChanges();
            LogManager.Log("VehicleTypes/Delete/ - VehicleTypeId:" + vehicleType.Id.ToString());
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
