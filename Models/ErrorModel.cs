using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HealthcareCompanion.Models
{
    public class ErrorModel
    {
        public string Location { get; set; }
        public string ErrorNumber { get; set; }
        public string ErrorMessage { get; set; }
        public string ActionTaken { get; set; }
        public string ReturnURLName { get; set; }
        public string ReturnURL { get; set; }
    }
}