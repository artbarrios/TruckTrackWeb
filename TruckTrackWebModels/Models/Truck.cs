using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TruckTrackWeb.Models
{
    public class Truck
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string Name { get; set; }

        [Display(Name = "Tag Number")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string TagNumber { get; set; }

        [Display(Name = "Date Purchased")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime DatePurchased { get; set; }

        [Required]
        [Display(Name = "Vehicle  Type")]
        public int VehicleTypeId { get; set; }
        [ForeignKey("VehicleTypeId")]
        [JsonIgnore]
        public virtual VehicleType VehicleType { get; set; }

        [Display(Name = "Flowchart Diagram Data")]
        [DataType(DataType.Text)]
        [StringLength(4000, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string StopEventFlowchartDiagramData { get; set; }

        [JsonIgnore]
        public virtual List<Load> Loads { get; set; }

        [JsonIgnore]
        public virtual List<StopEvent> StopEvents { get; set; }

    } // public class Truck
}
