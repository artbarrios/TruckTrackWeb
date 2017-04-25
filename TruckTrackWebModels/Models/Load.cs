using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TruckTrackWeb.Models
{
    public class Load
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Truck")]
        public int TruckId { get; set; }
        [ForeignKey("TruckId")]
        [JsonIgnore]
        public virtual Truck Truck { get; set; }

        [Display(Name = "Is Time Sensitive")]
        public bool IsTimeSensitive { get; set; }

        [JsonIgnore]
        public virtual List<Pallet> Pallets { get; set; }

    } // public class Load
}
