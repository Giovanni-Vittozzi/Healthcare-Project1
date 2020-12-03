using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HealthcareCompanion.DataAccessLayer;
using HealthcareCompanion.Models;
using System.Threading.Tasks;
using Owin;
using Microsoft.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using System.Web.UI.WebControls;

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
        public ActionResult ShowProfile()
        {
            DoctorTier tier   = new DoctorTier();
            Doctor     doctor = new Doctor();
            //need to get current signed in doctor
            if (Request.IsAuthenticated)
            {
                var          userStore   = new UserStore<IdentityUser>();
                var          userManager = new UserManager<IdentityUser>(userStore);
                IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                string       userEmail   = theUser.Email;
                string       userID      = theUser.Id;
                var          pendingDoc  = tier.isPendingDoctor(userEmail);
                doctor.DoctorID          = tier.getDoctorByID(userID);
                doctor                   = tier.retrieveDoctor(doctor.DoctorID);
                ViewBag.doctor           = doctor;
                return View(doctor);
            }
            return RedirectToAction("Index", "Doctor");
        }
        [HttpGet]
        public ActionResult EditDoctorInfo()
        {
            DoctorTier tier   = new DoctorTier();
            Doctor     doctor = new Doctor();
            //need to get current signed in doctor
            if (Request.IsAuthenticated)
            {
                var          userStore   = new UserStore<IdentityUser>();
                var          userManager = new UserManager<IdentityUser>(userStore);
                IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                string       userEmail   = theUser.Email;
                string       userID      = theUser.Id;
                var          pendingDoc  = tier.isPendingDoctor(userEmail);
                doctor.DoctorID          = tier.getDoctorByID(userID);
                doctor                   = tier.retrieveDoctor(doctor.DoctorID);
                ViewBag.doctor           = doctor;
            }
            return View();
        }
        [HttpPost]
        public ActionResult EditDoctorInfo(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                DoctorTier tier   = new DoctorTier();
                if (Request.IsAuthenticated)
                {
                    var userStore         = new UserStore<IdentityUser>();
                    var userManager       = new UserManager<IdentityUser>(userStore);
                    IdentityUser theUser  = userManager.FindById(User.Identity.GetUserId());
                    string userID         = theUser.Id;
                    doctor.DoctorID       = tier.getDoctorByID(userID);
                    tier.updateDoctorInfo(doctor);
                }
                return RedirectToAction("ShowProfile", "Doctor");
            }
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
        public ActionResult ApprovePatients(Doctor doctor)
        {
            DoctorTier tier    = new DoctorTier();
            //need to get current signed in doctor
            if (Request.IsAuthenticated)
            {
                var          userStore   = new UserStore<IdentityUser>();
                var          userManager = new UserManager<IdentityUser>(userStore);
                IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                string       userEmail   = theUser.Email;
                string       userID      = theUser.Id;
                var          pendingDoc  = tier.isPendingDoctor(userEmail);
                doctor.DoctorID          = tier.getDoctorByID(userID);
                if (!pendingDoc.pendingCheck)
                {
                    List<PatientFromDatabase> patientList = tier.listPendingPatients(doctor.DoctorID);
                    return View(patientList);
                }
            }
            return RedirectToAction("Pending", "Doctor");
        } 
        [HttpGet]
        public ActionResult ListPatients(Doctor doctor)
        {
            DoctorTier tier = new DoctorTier();
            //need to get current signed in doctor
            if (Request.IsAuthenticated)
            {
                var          userStore   = new UserStore<IdentityUser>();
                var          userManager = new UserManager<IdentityUser>(userStore);
                IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                string       userEmail   = theUser.Email;
                string       userID      = theUser.Id;
                var          pendingDoc  = tier.isPendingDoctor(userEmail);
                doctor.DoctorID          = tier.getDoctorByID(userID);
                if (!pendingDoc.pendingCheck)
                {
                    List<PatientFromDatabase> patientList = tier.listAllPatients(doctor.DoctorID);
                    return View(patientList);
                }
            }
            return RedirectToAction("Pending", "Doctor");
        }
        [HttpGet]
        public ActionResult Approve(int id)
        {
            PatientTier tier = new PatientTier();
            tier.approvePatient(id);
            return RedirectToAction("ApprovePatients", "Doctor");
        }
        [HttpGet]
        public ActionResult ChartBloodSugar(int patientID)
        {
            PatientTier tier        = new PatientTier();
            MedicalData medicalData = new MedicalData();
            if (Request.IsAuthenticated)
            {
                var          userStore   = new UserStore<IdentityUser>();
                var          userManager = new UserManager<IdentityUser>(userStore);
                IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                string       userID      = theUser.Id;
                string       email       = theUser.Email;
                medicalData.PatientID    = patientID;
                medicalData.TypeID       = 1;
                ViewBag.Email            = tier.getPatientEmail(patientID);
            }
            List<MedicalData> medicalDataList = tier.listMedicalDataByTypeID(medicalData.PatientID, medicalData.TypeID);
            List<string>      MonthList       = new List<string>();
            List<int>         YearList        = new List<int>();
            ViewBag.medicalDataList           = medicalDataList;
            if (medicalDataList != null)
            {
                foreach (var item in medicalDataList)
                {
                    DateTime date = new DateTime(2020, item.Now.Month, 1);
                    MonthList.Add(date.ToString("MMMM"));
                    YearList.Add(item.Now.Year);
                }
                MonthList = MonthList.Distinct().ToList();
                YearList  = YearList.Distinct().ToList();
                ViewBag.MonthList = MonthList;
                ViewBag.YearList  = YearList;
                ViewBag.Patient   = patientID;
            }
            return View();
        }
        [HttpPost, ActionName("ChartBloodSugar")]
        public ActionResult ChartBloodSugarNew(int patientID)
        {
            PatientTier tier     = new PatientTier();
            string      month    = Request.Form["MonthSelect"].ToString(); //Get selected month to change data to
            int?        monthInt = Convert.ToDateTime(month + " 01, 1900").Month;
            string      year     = Request.Form["YearSelect"].ToString(); //Get selected month to change data to
            int         yearInt  = Int32.Parse(year);
            return RedirectToAction("ChartBloodSugarByMonth", "Doctor", new { yearInt = yearInt, monthInt = monthInt, patientID = patientID});
        }
        [HttpGet]
        public ActionResult ChartBloodSugarByMonth(MedicalData medicalData, int? monthInt, int yearInt, int patientID)
        {
            PatientTier tier     = new PatientTier();
            if (Request.IsAuthenticated)
                {
                    var          userStore   = new UserStore<IdentityUser>();
                    var          userManager = new UserManager<IdentityUser>(userStore);
                    IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                    string       userID      = theUser.Id;
                    medicalData.PatientID    = patientID;
                    medicalData.TypeID       = 1;
                    ViewBag.Email = tier.getPatientEmail(patientID);
            }
            List<MedicalData> medicalDataList = tier.listMedicalDataByTypeIDByDate(medicalData.PatientID, medicalData.TypeID, monthInt, yearInt);
            ViewBag.medicalDataList           = medicalDataList;
            List<MedicalData> monthDataList   = tier.listMedicalDataByTypeID(medicalData.PatientID, medicalData.TypeID);
            List<string>      MonthList       = new List<string>();
            List<int>         YearList        = new List<int>();
            if (medicalDataList != null)
            {
                foreach (var item in monthDataList)
                {
                    DateTime date = new DateTime(2020, item.Now.Month, 1);
                    MonthList.Add(date.ToString("MMMM"));
                    YearList.Add(item.Now.Year);
                }
                MonthList = MonthList.Distinct().ToList();
                YearList  = YearList.Distinct().ToList();
                ViewBag.MonthList = MonthList;
                ViewBag.YearList  = YearList;
                ViewBag.Patient   = patientID;
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChartBloodSugarByMonth(int patientID)
        {
            PatientTier tier     = new PatientTier();
            string      month    = Request.Form["MonthSelect"].ToString(); //Get selected month to change data to
            int?        monthInt = Convert.ToDateTime(month + " 01, 1900").Month;
            string      year     = Request.Form["YearSelect"].ToString(); //Get selected month to change data to
            int         yearInt  = Int32.Parse(year);
            return RedirectToAction("ChartBloodSugarByMonth", "Doctor", new { yearInt = yearInt, monthInt= monthInt, patientID = patientID});
        }
        [HttpGet]
        public ActionResult ChartBloodPressure(int patientID)
        {
            MedicalData medicalData = new MedicalData();
            PatientTier tier        = new PatientTier();
            if (Request.IsAuthenticated)
            {
                var          userStore   = new UserStore<IdentityUser>();
                var          userManager = new UserManager<IdentityUser>(userStore);
                IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                string       userID      = theUser.Id;
                medicalData.PatientID    = patientID;
                medicalData.TypeID       = 2;
                ViewBag.Email            = tier.getPatientEmail(patientID);
            }
            List<MedicalData> medicalDataList = tier.listMedicalDataByTypeID(medicalData.PatientID, medicalData.TypeID);
            List<string>      MonthList       = new List<string>();
            List<int>         YearList        = new List<int>();
            ViewBag.medicalDataList           = medicalDataList;
            if (medicalDataList != null)
            {
                foreach (var item in medicalDataList)
                {
                    DateTime date = new DateTime(2020, item.Now.Month, 1);
                    MonthList.Add(date.ToString("MMMM"));
                    YearList.Add(item.Now.Year);
                }
                MonthList = MonthList.Distinct().ToList();
                YearList  = YearList.Distinct().ToList();
                ViewBag.MonthList = MonthList;
                ViewBag.YearList  = YearList;
                ViewBag.Patient   = patientID;
            }   
            return View();
        }
        [HttpPost, ActionName("ChartBloodPressure")]
        public ActionResult ChartBloodPressureNew(int patientID)
        {
            PatientTier tier     = new PatientTier();
            string      month    = Request.Form["MonthSelect"].ToString(); //Get selected month to change data to
            int?        monthInt = Convert.ToDateTime(month + " 01, 1900").Month;
            string      year     = Request.Form["YearSelect"].ToString(); //Get selected month to change data to
            int         yearInt  = Int32.Parse(year);
            return RedirectToAction("ChartBloodPressureByMonth", new { yearInt = yearInt, monthInt= monthInt, patientID = patientID});
        }
        [HttpGet]
        public ActionResult ChartBloodPressureByMonth(MedicalData medicalData, int? monthInt, int yearInt, int patientID)
        {
            PatientTier tier     = new PatientTier();
            if (Request.IsAuthenticated)
                {
                    var          userStore   = new UserStore<IdentityUser>();
                    var          userManager = new UserManager<IdentityUser>(userStore);
                    IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                    string       userID      = theUser.Id;
                    medicalData.PatientID    = patientID;
                    medicalData.TypeID       = 2;
                    ViewBag.Email            = tier.getPatientEmail(patientID);
            }
            List<MedicalData> medicalDataList = tier.listMedicalDataByTypeIDByDate(medicalData.PatientID, medicalData.TypeID, monthInt, yearInt);
            ViewBag.medicalDataList           = medicalDataList;
            List<MedicalData> monthDataList   = tier.listMedicalDataByTypeID(medicalData.PatientID, medicalData.TypeID);
            List<string>      MonthList       = new List<string>();
            List<int>         YearList        = new List<int>();
            if (medicalDataList != null)
            {
                foreach (var item in monthDataList)
                {
                    DateTime date = new DateTime(2020, item.Now.Month, 1);
                    MonthList.Add(date.ToString("MMMM"));
                    YearList.Add(item.Now.Year);
                }
                MonthList = MonthList.Distinct().ToList();
                YearList = YearList.Distinct().ToList();
                ViewBag.MonthList = MonthList;
                ViewBag.YearList  = YearList;
                ViewBag.Patient   = patientID;
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChartBloodPressureByMonth(int patientID)
        {
            PatientTier tier     = new PatientTier();
            string      month    = Request.Form["MonthSelect"].ToString(); //Get selected month to change data to
            int?        monthInt = Convert.ToDateTime(month + " 01, 1900").Month;
            string      year     = Request.Form["YearSelect"].ToString(); //Get selected month to change data to
            int         yearInt  = Int32.Parse(year);
            return RedirectToAction("ChartBloodPressureByMonth", "Doctor", new { yearInt = yearInt, monthInt= monthInt, patientID = patientID});
        }
        [HttpGet]
        public ActionResult ChartPulse(int patientID, MedicalData medicalData)
        {
            PatientTier tier = new PatientTier();
            if (Request.IsAuthenticated)
            {
                var          userStore   = new UserStore<IdentityUser>();
                var          userManager = new UserManager<IdentityUser>(userStore);
                IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                string       userID      = theUser.Id;
                medicalData.PatientID    = patientID;
                medicalData.TypeID       = 3;
                ViewBag.Email            = tier.getPatientEmail(patientID);
            }
            List<MedicalData> medicalDataList = tier.listMedicalDataByTypeID(medicalData.PatientID, medicalData.TypeID);
            List<string>      MonthList       = new List<string>();
            List<int>         YearList        = new List<int>();
            ViewBag.medicalDataList           = medicalDataList;
            if (medicalDataList != null)
            {
                foreach (var item in medicalDataList)
                {
                    DateTime date = new DateTime(2020, item.Now.Month, 1);
                    MonthList.Add(date.ToString("MMMM"));
                    YearList.Add(item.Now.Year);
                }
                MonthList = MonthList.Distinct().ToList();
                YearList  = YearList.Distinct().ToList();
                ViewBag.MonthList = MonthList;
                ViewBag.YearList  = YearList;
                ViewBag.Patient   = patientID;
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChartPulse(int patientID)
        {
            PatientTier tier     = new PatientTier();
            string      month    = Request.Form["MonthSelect"].ToString(); //Get selected month to change data to
            int?        monthInt = Convert.ToDateTime(month + " 01, 1900").Month;
            string      year     = Request.Form["YearSelect"].ToString(); //Get selected month to change data to
            int         yearInt  = Int32.Parse(year);
            return RedirectToAction("ChartPulseByMonth", new { yearInt = yearInt, monthInt= monthInt, patientID = patientID});
        }
        [HttpGet]
        public ActionResult ChartPulseByMonth(MedicalData medicalData, int? monthInt, int yearInt, int patientID)
        {
            PatientTier tier     = new PatientTier();
            if (Request.IsAuthenticated)
                {
                    var          userStore   = new UserStore<IdentityUser>();
                    var          userManager = new UserManager<IdentityUser>(userStore);
                    IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                    string       userID      = theUser.Id;
                    medicalData.PatientID    = patientID;
                    medicalData.TypeID       = 3;
                    ViewBag.Email            = tier.getPatientEmail(patientID);
            }
            List<MedicalData> medicalDataList = tier.listMedicalDataByTypeIDByDate(medicalData.PatientID, medicalData.TypeID, monthInt, yearInt);
            ViewBag.medicalDataList           = medicalDataList;
            List<MedicalData> monthDataList   = tier.listMedicalDataByTypeID(medicalData.PatientID, medicalData.TypeID);
            List<string>      MonthList       = new List<string>();
            List<int>         YearList        = new List<int>();
            if (medicalDataList != null)
            {
                foreach (var item in monthDataList)
                {
                    DateTime date = new DateTime(2020, item.Now.Month, 1);
                    MonthList.Add(date.ToString("MMMM"));
                    YearList.Add(item.Now.Year);
                }
                MonthList = MonthList.Distinct().ToList();
                YearList  = YearList.Distinct().ToList();
                ViewBag.MonthList = MonthList;
                ViewBag.YearList  = YearList;
                ViewBag.Patient   = patientID;
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChartPulseByMonth(int patientID)
        {
            PatientTier tier     = new PatientTier();
            string      month    = Request.Form["MonthSelect"].ToString(); //Get selected month to change data to
            int?        monthInt = Convert.ToDateTime(month + " 01, 1900").Month;
            string      year     = Request.Form["YearSelect"].ToString(); //Get selected month to change data to
            int         yearInt  = Int32.Parse(year);
            return RedirectToAction("ChartPulseByMonth", new { yearInt = yearInt, monthInt= monthInt, patientID = patientID});
        }
        [HttpGet]
        public ActionResult ChartWeight(MedicalData medicalData, int patientID)
        {
            PatientTier tier = new PatientTier();
            if (Request.IsAuthenticated)
            {
                var          userStore   = new UserStore<IdentityUser>();
                var          userManager = new UserManager<IdentityUser>(userStore);
                IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                string       userID      = theUser.Id;
                medicalData.PatientID    = patientID;
                medicalData.TypeID       = 4;
                ViewBag.Email            = tier.getPatientEmail(patientID);
            }
            List<MedicalData> medicalDataList = tier.listMedicalDataByTypeID(medicalData.PatientID, medicalData.TypeID);
            List<string>      MonthList       = new List<string>();
            List<int>         YearList        = new List<int>();
            ViewBag.medicalDataList           = medicalDataList;
            if (medicalDataList != null)
            {
                foreach (var item in medicalDataList)
                {
                    DateTime date = new DateTime(2020, item.Now.Month, 1);
                    MonthList.Add(date.ToString("MMMM"));
                    YearList.Add(item.Now.Year);
                }
                MonthList = MonthList.Distinct().ToList();
                YearList = YearList.Distinct().ToList();
                ViewBag.MonthList = MonthList;
                ViewBag.YearList  = YearList;
                ViewBag.Patient   = patientID;
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChartWeight(int patientID)
        {
            PatientTier tier     = new PatientTier();
            string      month    = Request.Form["MonthSelect"].ToString(); //Get selected month to change data to
            int?        monthInt = Convert.ToDateTime(month + " 01, 1900").Month;
            string      year     = Request.Form["YearSelect"].ToString(); //Get selected month to change data to
            int         yearInt  = Int32.Parse(year);
            return RedirectToAction("ChartWeightByMonth", new { yearInt = yearInt, monthInt= monthInt, patientID = patientID});
        }
        [HttpGet]
        public ActionResult ChartWeightByMonth(MedicalData medicalData, int? monthInt, int yearInt, int patientID)
        {
            PatientTier tier     = new PatientTier();
            if (Request.IsAuthenticated)
            {
                    var          userStore   = new UserStore<IdentityUser>();
                    var          userManager = new UserManager<IdentityUser>(userStore);
                    IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                    string       userID      = theUser.Id;
                    medicalData.PatientID    = patientID;
                    medicalData.TypeID       = 4;
                    ViewBag.Email            = tier.getPatientEmail(patientID);
            }
            List<MedicalData> medicalDataList = tier.listMedicalDataByTypeIDByDate(medicalData.PatientID, medicalData.TypeID, monthInt, yearInt);
            ViewBag.medicalDataList           = medicalDataList;
            List<MedicalData> monthDataList   = tier.listMedicalDataByTypeID(medicalData.PatientID, medicalData.TypeID);
            List<string>      MonthList       = new List<string>();
            List<int>         YearList        = new List<int>();
            if (medicalDataList != null)
            {
                foreach (var item in monthDataList)
                {
                    DateTime date = new DateTime(2020, item.Now.Month, 1);
                    MonthList.Add(date.ToString("MMMM"));
                    YearList.Add(item.Now.Year);
                }
                MonthList = MonthList.Distinct().ToList();
                YearList  = YearList.Distinct().ToList();
                ViewBag.MonthList = MonthList;
                ViewBag.YearList  = YearList;
                ViewBag.Patient   = patientID;
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChartWeightByMonth(int patientID)
        {
            PatientTier tier     = new PatientTier();
            string      month    = Request.Form["MonthSelect"].ToString(); //Get selected month to change data to
            int?        monthInt = Convert.ToDateTime(month + " 01, 1900").Month;
            string      year     = Request.Form["YearSelect"].ToString(); //Get selected month to change data to
            int         yearInt  = Int32.Parse(year);
            return RedirectToAction("ChartWeightByMonth", new { yearInt = yearInt, monthInt= monthInt, patientID = patientID});
        }

        [HttpPost]
        public ActionResult SignOut()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                HttpContext.GetOwinContext()
                           .Authentication
                           .SignOut(CookieAuthenticationDefaults.AuthenticationType);
            }
            return View();
            //return RedirectToAction("SignOut", "Patient");
        }
    }
}