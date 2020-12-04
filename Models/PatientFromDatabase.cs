using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HealthcareCompanion.Models
{
    public class PatientFromDatabase
    {
        public Boolean Pending { get; set; } //Pending approval by admin
        public int PatientID { get; set; }
        [Required(ErrorMessage = "This field is required.")]//can modify it from inherited or just put it in the Registration Model
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "This field is required.")]//can modify it from inherited or just put it in the Registration Model
        [Display(Name = "Patient Address")]
        public string PatientAddress { get; set; }
        [Required(ErrorMessage = "This field is required.")]//can modify it from inherited or just put it in the Registration Model
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
    }
}