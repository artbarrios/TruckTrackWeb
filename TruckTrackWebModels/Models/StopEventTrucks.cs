using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TruckTrackWeb.Models
{
    public class StopEventTrucks
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Stop  Event")]
        public int StopEventId { get; set; }
        [ForeignKey("StopEventId")]
        [JsonIgnore]
        public virtual StopEvent StopEvent { get; set; }

        [Required]
        [Display(Name = "Truck")]
        public int TruckId { get; set; }
        [ForeignKey("TruckId")]
        [JsonIgnore]
        public virtual Truck Truck { get; set; }

    } // public class StopEventTrucks
}
