using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HealthcareCompanion.Models;
using HealthcareCompanion.DataAccessLayer;

namespace HealthcareCompanion.Controllers
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
        public ActionResult EditPatientInfo()
        {

            return View();
        }
        [HttpGet]
        public ActionResult Pending()
        {
            return View();
        }
        [HttpGet]
        public ActionResult NotPending()
        {
            return View();
        }
        [HttpGet]
        public ActionResult MedicalDataSelection()
        {

            return View();
        }
        [HttpPost]
        public ActionResult MedicalDataSelection(Patient medicalDataSelection)
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
                PatientTier tier = new PatientTier();
                //patient.userID = theUser.Id;

                int userID = 2;
                tier.insertBloodSugar(userID, bloodSugar);
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