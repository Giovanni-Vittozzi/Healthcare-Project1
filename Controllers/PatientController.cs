using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Healthcare_Companion.Models;

namespace Healthcare_Companion.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient
        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }
        
        [HttpGet]
        public ActionResult MedicalDataSelection()
        {

            return View();
        }

        [HttpPost]
        public ActionResult MedicalDataSelectio(Patient medicalDataSelection)
        {
            if (ModelState.IsValid)
            {
                return View();
            }
            return View();
        }


        [HttpGet]
        public ActionResult BloodSugar()
        {

            return View();
        }

        [HttpPost]
        public ActionResult BloodSugar(BloodSugar bloodSugar)
        {
            if (ModelState.IsValid)
            {
                return View();
            }
            return View();
        }

        [HttpGet]
        public ActionResult BloodPressure()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Pulse()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Weight()
        {

            return View();
        }
    }
}