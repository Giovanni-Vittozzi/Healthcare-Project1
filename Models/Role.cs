using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HealthcareCompanion.Models
{
    public class Role
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please provide a role name")]
        public string Name { get; set; }
    }
}