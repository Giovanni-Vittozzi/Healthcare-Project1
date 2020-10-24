using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthcareCompanion.Controllers
{
    public class DoctorController : Controller
    {
        // GET: Doctor
        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public ActionResult EditDoctorInfo()
        {

            return View();
        }
    }
}