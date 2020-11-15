﻿using System;
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
        public string Password { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Confirm Password")]
        public string Confirm { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Select Your Doctor From The List Below:")]
        public int Doctor { get; set; }
    }

    public class BloodSugar
    {
        [Required]
        [Display(Name = "Enter Blood Sugar")]
        public int BloodSugarValue { get; set; }
        public DateTime Now { get; set; }
        [Required]
        [Display(Name = "Time of day Blood Sugar was taken")]
        public String TimeOfDay { get; set; }
    }
    public class BloodPressure
    {
        [Required]
        [Display(Name = "Enter Systolic Blood Pressure")]
        public int BloodPressureSystValue { get; set; }
        [Display(Name = "Enter Diastolic Blood Pressure")]
        public int BloodPressureDiastValue { get; set; }
        [Required]
        [Display(Name = "Time Blood Pressure was taken")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime Hour { get; set; }
        [Required]
        [Display(Name = "Date Blood Pressure was taken")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
    }
    public class Pulse
    {
        [Required]
        [Display(Name = "Enter Pulse")]
        public int BloodPressureSystValue { get; set; }
        [Required]
        [Display(Name = "Time Pulse was taken")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime Hour { get; set; }
        [Required]
        [Display(Name = "Date Pulse was taken")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
    }
    public class Weight
    {
        [Required]
        [Display(Name = "Enter Weight")]
        public int BloodPressureSystValue { get; set; }
        [Required]
        [Display(Name = "Time Weight was taken")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime Hour { get; set; }
        [Required]
        [Display(Name = "Date Weight was taken")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
    }

}