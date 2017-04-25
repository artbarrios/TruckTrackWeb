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
    public class TrucksController : Controller
    {
        private TruckTrackWebContext db = new TruckTrackWebContext();

        // Trucks/TruckStopEventsFlowchartDiagram/1
        public ActionResult TruckStopEventsFlowchartDiagram(int? id)
        {
            Truck truck = db.Trucks.Find(id);

            if (truck.StopEventFlowchartDiagramData == null || truck.StopEventFlowchartDiagramData.Length == 0)
            {
                Flowchart flowchart = TruckStopEventsToFlowchart(truck);
                truck.StopEventFlowchartDiagramData = flowchart.ToJSON();
                db.Entry(truck).State = EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.FlowchartTitle = "StopEvent Diagram for " + truck.Name;
            ViewBag.FlowchartData = truck.StopEventFlowchartDiagramData;
            return View(truck);
        }

        // POST: Trucks/TruckStopEventsFlowchartDiagram/1
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult TruckStopEventsFlowchartDiagram([Bind(Include = "Id")] Truck truck, string flowchartData)
        {
            truck = db.Trucks.Find(truck.Id);
            if (flowchartData.Length > 0)
            {
                truck.StopEventFlowchartDiagramData = flowchartData;
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = truck.Id });
        }

        // TruckStopEventsToFlowchart
        public static Flowchart TruckStopEventsToFlowchart(Truck truck)
        {
            // converts the specified objects into a Flowchart object and returns it
            Flowchart flowchart = new Flowchart();
            FlowchartOperator fcOperator = null;
            FlowchartOperator fcOperatorPrevious = null;
            FlowchartConnector fcInput = null;
            FlowchartConnector fcOutput = null;
            FlowchartLink fcLink = null;
            int top = 0;
            int left = 0;
            int opCount = 0;

            // check for valid input
            if (truck != null)
            {
                flowchart.Id = truck.Id.ToString();
                // add operators
                opCount = 1;
                foreach (StopEvent stopEvent in truck.StopEvents)
                {
                    fcOperator = new FlowchartOperator();
                    fcOperator.Id = "op" + truck.Id.ToString() + stopEvent.Id.ToString();
                    fcOperator.Title = stopEvent.Title;
                    fcOperator.Top = top;
                    fcOperator.Left = left;
                    fcOperator.ImageSource = stopEvent.ImageFilename;
                    top += 20;
                    left += 200;
                    // inputs
                    fcInput = new FlowchartConnector();
                    fcInput.Id = fcOperator.Id + "in1";
                    fcInput.Label = "Arrival";
                    fcOperator.Inputs.Add(fcInput);
                    // outputs
                    fcOutput = new FlowchartConnector();
                    fcOutput.Id = fcOperator.Id + "out1";
                    fcOutput.Label = "Departure";
                    fcOperator.Outputs.Add(fcOutput);
                    // popup
                    fcOperator.Popup.header = "<h2>" + stopEvent.Title + "</h2>";
                    fcOperator.Popup.body = @"<p>Hours of Operation: 9:00AM to 5:00PM M-F</p><p>Item Image:</p><img src='" + stopEvent.ImageFilename + "' alt='Image'>";
                    // add the operator
                    flowchart.Operators.Add(fcOperator);
                    opCount += 1;
                }

                // add links
                foreach (FlowchartOperator myOperator in flowchart.Operators)
                {
                    if (fcOperatorPrevious != null)
                    {
                        fcLink = new FlowchartLink();
                        fcLink.Id = myOperator.Id + "lnk1";
                        fcLink.FromOperatorId = fcOperatorPrevious.Id;
                        fcLink.FromConnectorId = fcOperatorPrevious.Outputs.FirstOrDefault().Id;
                        fcLink.ToOperatorId = myOperator.Id;
                        fcLink.ToConnectorId = myOperator.Inputs.FirstOrDefault().Id;
                        flowchart.Links.Add(fcLink);
                    }
                    fcOperatorPrevious = myOperator;
                }
            }
            return flowchart;
        } // TruckStopEventsToFlowchart ()

        // GET: AddLoadToTruck
        public ActionResult AddLoadToTruck(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Truck truck = db.Trucks.Find(id);
            var loadsAvailable = db.Loads.ToList().Except(truck.Loads.ToList()).ToList();
            ViewBag.LoadId = new SelectList(loadsAvailable, "Id", "Name");
            if (truck == null)
            {
                return HttpNotFound();
            }
            TruckLoadViewModel viewModel = new TruckLoadViewModel();
            viewModel.TruckId = truck.Id;
            viewModel.Truck_Name = truck.Name;
            return View(viewModel);
        }

        // POST: Trucks/AddLoadToTruck/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddLoadToTruck([Bind(Include = "TruckId,Truck_Name,LoadId,Load_Name")] TruckLoadViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Truck truck = db.Trucks.Find(viewModel.TruckId);
                Load load = db.Loads.Find(viewModel.LoadId);
                truck.Loads.Add(load);
                db.Entry(truck).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("Trucks/AddLoadToTruck/ - LoadId:" + load.Id.ToString() + " to TruckId: " + truck.Id.ToString());
                return RedirectToAction("Details", new { id = viewModel.TruckId });
            }
            return View(viewModel);
        }

        // GET: RemoveLoadFromTruck
        // NOTE: the Load.TruckId property is not nullable so this routine must be omitted

        // GET: AddStopEventToTruck
        public ActionResult AddStopEventToTruck(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Truck truck = db.Trucks.Find(id);
            var stopEventsAvailable = db.StopEvents.ToList().Except(truck.StopEvents.ToList()).ToList();
            ViewBag.StopEventId = new SelectList(stopEventsAvailable, "Id", "Title");
            if (truck == null)
            {
                return HttpNotFound();
            }
            StopEventTruckViewModel viewModel = new StopEventTruckViewModel();
            viewModel.TruckId = truck.Id;
            viewModel.Truck_Name = truck.Name;
            return View(viewModel);
        }

        // POST: Trucks/AddStopEventToTruck/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStopEventToTruck([Bind(Include = "TruckId,Truck_Name,StopEventId,StopEvent_Title")] StopEventTruckViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Truck truck = db.Trucks.Find(viewModel.TruckId);
                StopEvent stopEvent = db.StopEvents.Find(viewModel.StopEventId);
                truck.StopEvents.Add(stopEvent);
                Flowchart flowchart = TruckStopEventsToFlowchart(truck);
                truck.StopEventFlowchartDiagramData = flowchart.ToJSON();
                db.Entry(truck).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("Trucks/AddStopEventToTruck/ - StopEventId:" + stopEvent.Id.ToString() + " to TruckId: " + truck.Id.ToString());
                return RedirectToAction("Details", new { id = viewModel.TruckId });
            }
            return View(viewModel);
        }

        // GET: RemoveStopEventFromTruck
        public ActionResult RemoveStopEventFromTruck(int? truckId, int? stopEventId)
        {
            if (truckId == null || stopEventId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Truck truck = db.Trucks.Find(truckId);
            StopEvent stopEvent = db.StopEvents.Find(stopEventId);
            if (truck == null || stopEvent == null)
            {
                return HttpNotFound();
            }
            truck.StopEvents.Remove(stopEvent);
                Flowchart flowchart = TruckStopEventsToFlowchart(truck);
                truck.StopEventFlowchartDiagramData = flowchart.ToJSON();
            db.Entry(truck).State = EntityState.Modified;
            db.SaveChanges();
            LogManager.Log("Trucks/RemoveStopEventFromTruck/ - StopEventId:" + stopEvent.Id.ToString() + " from TruckId: " + truck.Id.ToString());
            return RedirectToAction("Details", new { id = truckId });
        }

        // GET: Trucks
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/TrucksIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            var trucks = db.Trucks.Include(t => t.VehicleType);
            return View(trucks.ToList());
        }

        // GET: Trucks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Truck truck = db.Trucks.Find(id);
            if (truck == null)
            {
                return HttpNotFound();
            }
            return View(truck);
        }

        // GET: Trucks/Create
        public ActionResult Create(int? id, string modelType)
        {
            if (modelType != null && modelType.Length > 0)
            {
                ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Name", modelType.Contains("VehicleType") ? id : -1);
            }
            else
            {
                ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Name");
            }
            // modelType not always properly being passed as query string parameter so send it in ViewBag
            ViewBag.modelType = modelType;
            return View();
        }

        // POST: Trucks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,TagNumber,DatePurchased,VehicleTypeId,StopEventFlowchartDiagramData")] Truck truck, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.Trucks.Add(truck);
                db.SaveChanges();
                LogManager.Log("Trucks/Create - TruckId:" + truck.Id.ToString());
                if (modelType != null && modelType.Length > 0)
                {
                    if (modelType.Contains("VehicleType"))
                    {
                        db.VehicleTypes.Find(id).Trucks.Add(truck);
                        db.SaveChanges();
                        LogManager.Log("Trucks/Create added - TruckId:" + truck.Id.ToString() + " to VehicleTypeId: " + id.ToString());
                        controllerName = "VehicleTypes";
                    }
                    if (modelType.Contains("StopEvent"))
                    {
                        db.StopEvents.Find(id).Trucks.Add(truck);
                        db.SaveChanges();
                        LogManager.Log("Trucks/Create added - TruckId:" + truck.Id.ToString() + " to StopEventId: " + id.ToString());
                        controllerName = "StopEvents";
                    }
                    return RedirectToAction("Details", controllerName, new { Id = id });
                }
                return RedirectToAction("Index");
            }
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Name", truck.VehicleTypeId);

            return View(truck);
        }

        // GET: Trucks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Truck truck = db.Trucks.Find(id);
            if (truck == null)
            {
                return HttpNotFound();
            }
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Name", truck.VehicleTypeId);

            return View(truck);
        }

        // POST: Trucks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,TagNumber,DatePurchased,VehicleTypeId,StopEventFlowchartDiagramData")] Truck truck)
        {
            if (ModelState.IsValid)
            {
                db.Entry(truck).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("Trucks/Edit/ - TruckId:" + truck.Id.ToString());
                return RedirectToAction("Index");
            }
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Name", truck.VehicleTypeId);

            return View(truck);
        }

        // GET: Trucks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Truck truck = db.Trucks.Find(id);
            if (truck == null)
            {
                return HttpNotFound();
            }
            return View(truck);
        }

        // POST: Trucks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Truck truck = db.Trucks.Find(id);
            db.Trucks.Remove(truck);
            db.SaveChanges();
            LogManager.Log("Trucks/Delete/ - TruckId:" + truck.Id.ToString());
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
