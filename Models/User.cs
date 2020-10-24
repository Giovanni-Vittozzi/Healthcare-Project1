using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HealthcareCompanion.Models
{
    public class User
    {
        [Required(ErrorMessage = "Please enter a User Name")]
        public string username { get; set; }

        [Required(ErrorMessage ="Please enter a Password")]
        public string password { get; set; }

 
    }
}