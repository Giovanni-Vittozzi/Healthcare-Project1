using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HealthcareCompanion.Models
{
    public class Patient
    {
        public Boolean Pending { get; set; } //Pending approval by admin
        public int PatientID { get; set; }
        public string userID { get; set; }
        [Required(ErrorMessage = "This field is required.")]//can modify it from inherited or just put it in the Registration Model
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Address { get; set; }
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }
        [Display(Name = "Apt./Suite Number")]
        public string AptNum { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string City { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string State { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Zip Code")]
        public int ZipCode { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Confirm Password")]
        public string Confirm { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Select Your Doctor From The List Below:")]
        public int Doctor { get; set; }
    }
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