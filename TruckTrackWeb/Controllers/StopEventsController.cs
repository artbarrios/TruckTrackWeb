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
    public class StopEventsController : Controller
    {
        private TruckTrackWebContext db = new TruckTrackWebContext();

        // GET: AddTruckToStopEvent
        public ActionResult AddTruckToStopEvent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StopEvent stopEvent = db.StopEvents.Find(id);
            var trucksAvailable = db.Trucks.ToList().Except(stopEvent.Trucks.ToList()).ToList();
            ViewBag.TruckId = new SelectList(trucksAvailable, "Id", "Name");
            if (stopEvent == null)
            {
                return HttpNotFound();
            }
            StopEventTruckViewModel viewModel = new StopEventTruckViewModel();
            viewModel.StopEventId = stopEvent.Id;
            viewModel.StopEvent_Title = stopEvent.Title;
            return View(viewModel);
        }

        // POST: StopEvents/AddTruckToStopEvent/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTruckToStopEvent([Bind(Include = "StopEventId,StopEvent_Title,TruckId,Truck_Name")] StopEventTruckViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                StopEvent stopEvent = db.StopEvents.Find(viewModel.StopEventId);
                Truck truck = db.Trucks.Find(viewModel.TruckId);
                stopEvent.Trucks.Add(truck);
                db.Entry(stopEvent).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("StopEvents/AddTruckToStopEvent/ - TruckId:" + truck.Id.ToString() + " to StopEventId: " + stopEvent.Id.ToString());
                return RedirectToAction("Details", new { id = viewModel.StopEventId });
            }
            return View(viewModel);
        }

        // GET: RemoveTruckFromStopEvent
        public ActionResult RemoveTruckFromStopEvent(int? stopEventId, int? truckId)
        {
            if (stopEventId == null || truckId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StopEvent stopEvent = db.StopEvents.Find(stopEventId);
            Truck truck = db.Trucks.Find(truckId);
            if (stopEvent == null || truck == null)
            {
                return HttpNotFound();
            }
            stopEvent.Trucks.Remove(truck);
            db.Entry(stopEvent).State = EntityState.Modified;
            db.SaveChanges();
            LogManager.Log("StopEvents/RemoveTruckFromStopEvent/ - TruckId:" + truck.Id.ToString() + " from StopEventId: " + stopEvent.Id.ToString());
            return RedirectToAction("Details", new { id = stopEventId });
        }

        // GET: StopEvents
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/StopEventsIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            return View(db.StopEvents.ToList());
        }

        // GET: StopEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StopEvent stopEvent = db.StopEvents.Find(id);
            if (stopEvent == null)
            {
                return HttpNotFound();
            }
            return View(stopEvent);
        }

        // GET: StopEvents/Create
        public ActionResult Create(int? id, string modelType)
        {
            if (modelType != null && modelType.Length > 0)
            {
            }
            else
            {
            }
            // modelType not always properly being passed as query string parameter so send it in ViewBag
            ViewBag.modelType = modelType;
            return View();
        }

        // POST: StopEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,StartDate,StartTime,EndDate,EndTime,IsAllDay,ImageFilename")] StopEvent stopEvent, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.StopEvents.Add(stopEvent);
                db.SaveChanges();
                LogManager.Log("StopEvents/Create - StopEventId:" + stopEvent.Id.ToString());
                if (modelType != null && modelType.Length > 0)
                {
                    if (modelType.Contains("Truck"))
                    {
                        db.Trucks.Find(id).StopEvents.Add(stopEvent);
                        db.SaveChanges();
                        LogManager.Log("StopEvents/Create added - StopEventId:" + stopEvent.Id.ToString() + " to TruckId: " + id.ToString());
                        controllerName = "Trucks";
                    }
                    return RedirectToAction("Details", controllerName, new { Id = id });
                }
                return RedirectToAction("Index");
            }

            return View(stopEvent);
        }

        // GET: StopEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StopEvent stopEvent = db.StopEvents.Find(id);
            if (stopEvent == null)
            {
                return HttpNotFound();
            }
            return View(stopEvent);
        }

        // POST: StopEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,StartDate,StartTime,EndDate,EndTime,IsAllDay,ImageFilename")] StopEvent stopEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stopEvent).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("StopEvents/Edit/ - StopEventId:" + stopEvent.Id.ToString());
                return RedirectToAction("Index");
            }
            return View(stopEvent);
        }

        // GET: StopEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StopEvent stopEvent = db.StopEvents.Find(id);
            if (stopEvent == null)
            {
                return HttpNotFound();
            }
            return View(stopEvent);
        }

        // POST: StopEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StopEvent stopEvent = db.StopEvents.Find(id);
            db.StopEvents.Remove(stopEvent);
            db.SaveChanges();
            LogManager.Log("StopEvents/Delete/ - StopEventId:" + stopEvent.Id.ToString());
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
