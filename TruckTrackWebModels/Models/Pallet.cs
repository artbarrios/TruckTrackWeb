using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TruckTrackWeb.Models
{
    public class Pallet
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string Name { get; set; }

        [Display(Name = "Value")]
        public decimal Value { get; set; }

        [Required]
        [Display(Name = "Load")]
        public int LoadId { get; set; }
        [ForeignKey("LoadId")]
        [JsonIgnore]
        public virtual Load Load { get; set; }

    } // public class Pallet
}
