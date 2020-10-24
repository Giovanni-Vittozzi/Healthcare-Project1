using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HealthcareCompanion.DataAccessLayer;
using HealthcareCompanion.Models;

namespace HealthcareCompanion.Controllers
{
    //[RequireHttps]
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public ActionResult PatientInfoRegistration()
        {

            return View();
        }
        [HttpPost]
        public ActionResult PatientInfoRegistration(Patient patient)
        {
            if (ModelState.IsValid)
            {
                //Identity here
                PatientTier tier = new PatientTier();
                tier.insertPatient(patient);

                RedirectToAction("Index");

            }
            return View();
        }
        [HttpGet]
        public ActionResult ListAllPatients()
        {
            PatientTier tier = new PatientTier();
            List<Patient> patientList = tier.getAllPatients();

            return View(patientList);
        }

        [HttpGet]
        public ActionResult AddDoctorInfo()
        {

            return View();
        }

        [HttpGet]
        public ActionResult About()
        {

            return View();
        }
    }
}