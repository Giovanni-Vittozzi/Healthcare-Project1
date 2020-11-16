using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HealthcareCompanion.Models
{
    public class MedicalData
    {
        public int TypeID { get; set; }
        public int PatientID { get; set; }
        [Required]
        public int Value1 { get; set; }
        [Required]
        public int Value2 { get; set; }
        public DateTime Now { get; set; }
        [Required]
        [Display(Name = "Time of day reading was taken")]
        public String TimeOfDay { get; set; }
    }
}