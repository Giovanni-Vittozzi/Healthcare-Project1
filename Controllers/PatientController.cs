﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HealthcareCompanion.Models;
using HealthcareCompanion.DataAccessLayer;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

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
            var TimeOfDayList     = new List<string>() { "Morning", "Afternoon", "Evening", "Night" };
            ViewBag.TimeOfDayList = TimeOfDayList;
            return View();
        }
        [HttpPost]
        public ActionResult BloodSugar(MedicalData medicalData)
        {
            if (ModelState.IsValid)
            {
                PatientTier tier   = new PatientTier();
                medicalData.Value2 = 0; //this value is only needed for blood pressure
                if (Request.IsAuthenticated)
                {
                    var          userStore   = new UserStore<IdentityUser>();
                    var          userManager = new UserManager<IdentityUser>(userStore);
                    IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                    string       userID      = theUser.Id;
                    medicalData.PatientID    = tier.getPatientByID(userID);
                }
                medicalData.TypeID    = 1; //From the MedicalDataType Table, Blood Sugar is 1
                medicalData.TimeOfDay = Request.Form["TimeOfDay"]; //Get selected time of day the reading was taken
                tier.insertMedicalData(medicalData);
                return View();
            }
            return View();
        }
        [HttpGet]
        public ActionResult BloodPressure()
        {
            var TimeOfDayList = new List<string>() { "Morning", "Afternoon", "Evening", "Night" };
            ViewBag.TimeOfDayList = TimeOfDayList;
            return View();
        }
        [HttpPost]
        public ActionResult BloodPressure(MedicalData medicalData)
        {
            if (ModelState.IsValid)
            {
                PatientTier tier   = new PatientTier();
                if (Request.IsAuthenticated)
                {
                    var          userStore   = new UserStore<IdentityUser>();
                    var          userManager = new UserManager<IdentityUser>(userStore);
                    IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                    string       userID      = theUser.Id;
                    medicalData.PatientID    = tier.getPatientByID(userID);
                }
                medicalData.TypeID    = 2; //From the MedicalDataType Table, Blood Pressure is 2
                medicalData.TimeOfDay = Request.Form["TimeOfDay"]; //Get selected time of day the reading was taken
                tier.insertMedicalData(medicalData);
                return View();
            }
            return View();
        }
        [HttpGet]
        public ActionResult Pulse()
        {
            var TimeOfDayList = new List<string>() { "Morning", "Afternoon", "Evening", "Night" };
            ViewBag.TimeOfDayList = TimeOfDayList;
            return View();
        }
        [HttpPost]
        public ActionResult Pulse(MedicalData medicalData)
        {
            if (ModelState.IsValid)
            {
                PatientTier tier   = new PatientTier();
                medicalData.Value2 = 0; //this value is only needed for blood pressure
                if (Request.IsAuthenticated)
                {
                    var          userStore   = new UserStore<IdentityUser>();
                    var          userManager = new UserManager<IdentityUser>(userStore);
                    IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                    string       userID      = theUser.Id;
                    medicalData.PatientID    = tier.getPatientByID(userID);
                }
                medicalData.TypeID    = 3; //From the MedicalDataType Table, Pulse is 3
                medicalData.TimeOfDay = Request.Form["TimeOfDay"]; //Get selected time of day the reading was taken
                tier.insertMedicalData(medicalData);
                return View();
            }
            return View();

        }
        [HttpGet]
        public ActionResult Weight()
        {
            var TimeOfDayList     = new List<string>() { "Morning", "Afternoon", "Evening", "Night" };
            ViewBag.TimeOfDayList = TimeOfDayList;
            return View();
        }
        [HttpPost]
        public ActionResult Weight(MedicalData medicalData)
        {
            if (ModelState.IsValid)
            {
                PatientTier tier   = new PatientTier();
                medicalData.Value2 = 0; //this value is only needed for blood pressure
                if (Request.IsAuthenticated)
                {
                    var          userStore   = new UserStore<IdentityUser>();
                    var          userManager = new UserManager<IdentityUser>(userStore);
                    IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                    string       userID      = theUser.Id;
                    medicalData.PatientID    = tier.getPatientByID(userID);
                }
                medicalData.TypeID    = 4; //From the MedicalDataType Table, Weight is 4
                medicalData.TimeOfDay = Request.Form["TimeOfDay"]; //Get selected time of day the reading was taken
                tier.insertMedicalData(medicalData);
                return View();
            }
            return View();

        }
    }
}