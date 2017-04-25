using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TruckTrackWeb.Models
{
    public class StopEventTruckViewModel
    {

        [Key]
        public int Id { get; set; }

        public int StopEventId { get; set; }

        [Display(Name = "Title")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string StopEvent_Title { get; set; }

        public int TruckId { get; set; }

        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string Truck_Name { get; set; }

    } // public class StopEventTruckViewModel
}
