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
using System.Dynamic;

namespace TruckTrackWeb.Controllers
{
    public class StopEventsDataController : ApiController
    {
        private TruckTrackWebContext db = new TruckTrackWebContext();

        // GET: api/FullCalendarEvents?start=2017-02-01&end=2017-02-10
        // attribute routing used to get our parameters
        [Route("api/FullCalendarEvents")]
        [HttpGet]
        public List<ExpandoObject> GetFullCalendarEvents(string start = "", string end = "")
        {
            DateTime startDate = Convert.ToDateTime(start).Date;
            DateTime endDate = Convert.ToDateTime(end).Date;

            // get only the StopEvents between startDate and endDate
            List<StopEvent> StopEvents = (from stopEvent in db.StopEvents
                                                  where (stopEvent.StartDate >= startDate && stopEvent.EndDate < endDate)
                                                  select stopEvent).ToList();
            // convert the StopEvents into FullCalendarEvents

            List<ExpandoObject> FullCalendarEvents = new List<ExpandoObject>();
            dynamic fullCalendarEvent;

            foreach (StopEvent stopEvent in StopEvents)
            {
                fullCalendarEvent = new ExpandoObject();
                fullCalendarEvent.id = stopEvent.Id.ToString();
                fullCalendarEvent.title = stopEvent.Title.ToString();
                fullCalendarEvent.allDay = stopEvent.IsAllDay;
                fullCalendarEvent.start = stopEvent.StartDate.ToString("yyyy-MM-dd") + "T" + stopEvent.StartTime.ToString("HH:mm:ss");
                fullCalendarEvent.end = stopEvent.StartDate.ToString("yyyy-MM-dd") + "T" + stopEvent.StartTime.ToString("HH:mm:ss");
                // set the url to the details page for this event
                fullCalendarEvent.url = "/StopEvents/Details/" + stopEvent.Id.ToString();
                FullCalendarEvents.Add(fullCalendarEvent);
            }

            return FullCalendarEvents;
        }


        // GET: api/GetStopEventTrucks/?StopEventId=1
        [Route("api/GetStopEventTrucks/")]
        public List<Truck> GetStopEventTrucks(int StopEventId)
        {
            StopEvent stopEvent = db.StopEvents.Find(StopEventId);
            if (stopEvent == null)
            {
                return null;
            }
            return stopEvent.Trucks;
        }

        // PUT: api/AddTruckToStopEvent/?StopEventId=1&TruckId=1
        [HttpPut]
        [Route("api/AddTruckToStopEvent/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddTruckToStopEvent(int StopEventId, int TruckId)
        {
            StopEvent stopEvent = db.StopEvents.Find(StopEventId);
            Truck truck = db.Trucks.Find(TruckId);
            if (stopEvent != null && truck != null)
            {
                try
                {
                    stopEvent.Trucks.Add(truck);
                    db.Entry(stopEvent).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/AddTruckToStopEvent - StopEventId:" + stopEvent.Id.ToString() + " TruckId:" + truck.Id.ToString());
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
        } // AddTruckToStopEvent

        // PUT: api/RemoveTruckFromStopEvent/?StopEventId=1&TruckId=1
        [HttpPut]
        [Route("api/RemoveTruckFromStopEvent/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult RemoveTruckFromStopEvent(int StopEventId, int TruckId)
        {
            StopEvent stopEvent = db.StopEvents.Find(StopEventId);
            Truck truck = db.Trucks.Find(TruckId);
            if (stopEvent != null && truck != null)
            {
                try
                {
                    stopEvent.Trucks.Remove(truck);
                    db.Entry(stopEvent).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/RemoveTruckToStopEvent - StopEventId:" + stopEvent.Id.ToString() + " TruckId:" + truck.Id.ToString());
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
        } // RemoveTruckFromStopEvent

        // GET: api/StopEventsData
        public IQueryable<StopEvent> GetStopEvents()
        {
            return db.StopEvents;
        }

        // GET: api/StopEventsData/5
        [ResponseType(typeof(StopEvent))]
        public IHttpActionResult GetStopEvent(int id)
        {
            StopEvent stopEvent = db.StopEvents.Find(id);
            if (stopEvent == null)
            {
                return NotFound();
            }

            return Ok(stopEvent);
        }

        // PUT: api/StopEventsData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStopEvent(int id, StopEvent stopEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stopEvent.Id)
            {
                return BadRequest();
            }

            db.Entry(stopEvent).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                LogManager.Log("PUT: api/StopEventsData/ - StopEventId:" + stopEvent.Id.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StopEventExists(id))
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

        // POST: api/StopEventsData
        [ResponseType(typeof(StopEvent))]
        public IHttpActionResult PostStopEvent(StopEvent stopEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StopEvents.Add(stopEvent);
            db.SaveChanges();
            LogManager.Log("POST: api/StopEventsData/ - StopEventId:" + stopEvent.Id.ToString());

            return CreatedAtRoute("DefaultApi", new { id = stopEvent.Id }, stopEvent);
        }

        // DELETE: api/StopEventsData/5
        [ResponseType(typeof(StopEvent))]
        public IHttpActionResult DeleteStopEvent(int id)
        {
            StopEvent stopEvent = db.StopEvents.Find(id);
            if (stopEvent == null)
            {
                return NotFound();
            }

            db.StopEvents.Remove(stopEvent);
            db.SaveChanges();
            LogManager.Log("DELETE: api/StopEventsData/ - StopEventId:" + stopEvent.Id.ToString());

            return Ok(stopEvent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StopEventExists(int id)
        {
            return db.StopEvents.Count(e => e.Id == id) > 0;
        }
    }
}
