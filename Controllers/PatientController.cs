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
    [Authorize(Roles = "Patient")]
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
            PatientTier tier = new PatientTier();
            //need to get current signed in doctor
            if (Request.IsAuthenticated)
            {
                var          userStore   = new UserStore<IdentityUser>();
                var          userManager = new UserManager<IdentityUser>(userStore);
                IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                string       userEmail   = theUser.Email;
                var          pendPatient = tier.isPendingPatient(userEmail);
                if (!pendPatient.pendingCheck)
                {
                    return View();
                }
            }
            return RedirectToAction("Pending", "Patient");
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
                    var userStore         = new UserStore<IdentityUser>();
                    var userManager       = new UserManager<IdentityUser>(userStore);
                    IdentityUser theUser  = userManager.FindById(User.Identity.GetUserId());
                    string userID         = theUser.Id;
                    medicalData.PatientID = tier.getPatientByID(userID);
                }
                medicalData.TypeID    = 1; //From the MedicalDataType Table, Blood Sugar is 1
                medicalData.TimeOfDay = Request.Form["TimeOfDay"]; //Get selected time of day the reading was taken
                tier.insertMedicalData(medicalData);
                return RedirectToAction("ViewMedicalData", "Patient");
            }
            return View();
        }
        [HttpGet]
        public ActionResult BloodPressure()
        {
            var TimeOfDayList     = new List<string>() { "Morning", "Afternoon", "Evening", "Night" };
            ViewBag.TimeOfDayList = TimeOfDayList;
            return View();
        }
        [HttpPost]
        public ActionResult BloodPressure(MedicalData medicalData)
        {
            if (ModelState.IsValid)
            {
                PatientTier tier = new PatientTier();
                if (Request.IsAuthenticated)
                {
                    var userStore         = new UserStore<IdentityUser>();
                    var userManager       = new UserManager<IdentityUser>(userStore);
                    IdentityUser theUser  = userManager.FindById(User.Identity.GetUserId());
                    string userID         = theUser.Id;
                    medicalData.PatientID = tier.getPatientByID(userID);
                }
                medicalData.TypeID    = 2; //From the MedicalDataType Table, Blood Pressure is 2
                medicalData.TimeOfDay = Request.Form["TimeOfDay"]; //Get selected time of day the reading was taken
                tier.insertMedicalData(medicalData);
                return RedirectToAction("ViewMedicalData", "Patient");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Pulse()
        {
            var TimeOfDayList     = new List<string>() { "Morning", "Afternoon", "Evening", "Night" };
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
                    var userStore         = new UserStore<IdentityUser>();
                    var userManager       = new UserManager<IdentityUser>(userStore);
                    IdentityUser theUser  = userManager.FindById(User.Identity.GetUserId());
                    string userID         = theUser.Id;
                    medicalData.PatientID = tier.getPatientByID(userID);
                }
                medicalData.TypeID    = 3; //From the MedicalDataType Table, Pulse is 3
                medicalData.TimeOfDay = Request.Form["TimeOfDay"]; //Get selected time of day the reading was taken
                tier.insertMedicalData(medicalData);
                return RedirectToAction("ViewMedicalData", "Patient");
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
                return RedirectToAction("ViewMedicalData", "Patient");
            }
            return View();
        }
        [HttpGet]
        public ActionResult ViewMedicalData(MedicalData medicalData)
        {
            PatientTier tier = new PatientTier();
            if (Request.IsAuthenticated)
                {
                    var          userStore   = new UserStore<IdentityUser>();
                    var          userManager = new UserManager<IdentityUser>(userStore);
                    IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                    string       userID      = theUser.Id;
                    medicalData.PatientID    = tier.getPatientByID(userID);
                }
            List<MedicalData> medicalDataList = tier.listMedicalData(medicalData.PatientID);
            return View(medicalDataList);
        }
        [HttpGet]
        public ActionResult ChartBloodSugar(MedicalData medicalData)
        {
            PatientTier tier = new PatientTier();
            if (Request.IsAuthenticated)
            {
                var          userStore   = new UserStore<IdentityUser>();
                var          userManager = new UserManager<IdentityUser>(userStore);
                IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                string       userID      = theUser.Id;
                medicalData.PatientID    = tier.getPatientByID(userID);
                medicalData.TypeID       = 1;
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
                ViewBag.YearList = YearList;
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChartBloodSugar()
        {
            PatientTier tier     = new PatientTier();
            string      month    = Request.Form["MonthSelect"].ToString(); //Get selected month to change data to
            int?        monthInt = Convert.ToDateTime(month + " 01, 1900").Month;
            string      year     = Request.Form["YearSelect"].ToString(); //Get selected month to change data to
            int         yearInt  = Int32.Parse(year);
            return RedirectToAction("ChartBloodSugarByMonth", new { yearInt = yearInt, monthInt= monthInt});
        }
        [HttpGet]
        public ActionResult ChartBloodSugarByMonth(MedicalData medicalData, int? monthInt, int yearInt)
        {
            PatientTier tier     = new PatientTier();
            if (Request.IsAuthenticated)
                {
                    var          userStore   = new UserStore<IdentityUser>();
                    var          userManager = new UserManager<IdentityUser>(userStore);
                    IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                    string       userID      = theUser.Id;
                    medicalData.PatientID    = tier.getPatientByID(userID);
                    medicalData.TypeID       = 1;
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
                ViewBag.YearList = YearList;
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChartBloodSugarByMonth()
        {
            PatientTier tier     = new PatientTier();
            string      month    = Request.Form["MonthSelect"].ToString(); //Get selected month to change data to
            int?        monthInt = Convert.ToDateTime(month + " 01, 1900").Month;
            string      year     = Request.Form["YearSelect"].ToString(); //Get selected month to change data to
            int         yearInt  = Int32.Parse(year);
            return RedirectToAction("ChartBloodSugarByMonth", new { yearInt = yearInt, monthInt= monthInt});
        }
        [HttpGet]
        public ActionResult ChartBloodPressure()
        {
            MedicalData medicalData = new MedicalData();
            PatientTier tier        = new PatientTier();
            if (Request.IsAuthenticated)
            {
                var          userStore   = new UserStore<IdentityUser>();
                var          userManager = new UserManager<IdentityUser>(userStore);
                IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                string       userID      = theUser.Id;
                medicalData.PatientID    = tier.getPatientByID(userID);
                medicalData.TypeID       = 2;
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
                ViewBag.YearList = YearList;
            }   
            return View();
        }
        [HttpPost, ActionName("ChartBloodPressure")]
        public ActionResult ChartBloodPressureNew()
        {
            PatientTier tier     = new PatientTier();
            string      month    = Request.Form["MonthSelect"].ToString(); //Get selected month to change data to
            int?        monthInt = Convert.ToDateTime(month + " 01, 1900").Month;
            string      year     = Request.Form["YearSelect"].ToString(); //Get selected month to change data to
            int         yearInt  = Int32.Parse(year);
            return RedirectToAction("ChartBloodPressureByMonth", new { yearInt = yearInt, monthInt= monthInt});
        }
        [HttpGet]
        public ActionResult ChartBloodPressureByMonth(MedicalData medicalData, int? monthInt, int yearInt)
        {
            PatientTier tier     = new PatientTier();
            if (Request.IsAuthenticated)
                {
                    var          userStore   = new UserStore<IdentityUser>();
                    var          userManager = new UserManager<IdentityUser>(userStore);
                    IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                    string       userID      = theUser.Id;
                    medicalData.PatientID    = tier.getPatientByID(userID);
                    medicalData.TypeID       = 2;
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
                ViewBag.YearList = YearList;
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChartBloodPressureByMonth()
        {
            PatientTier tier     = new PatientTier();
            string      month    = Request.Form["MonthSelect"].ToString(); //Get selected month to change data to
            int?        monthInt = Convert.ToDateTime(month + " 01, 1900").Month;
            string      year     = Request.Form["YearSelect"].ToString(); //Get selected month to change data to
            int         yearInt  = Int32.Parse(year);
            return RedirectToAction("ChartBloodPressureByMonth", new { yearInt = yearInt, monthInt= monthInt});
        }
        [HttpGet]
        public ActionResult ChartPulse(MedicalData medicalData)
        {
            PatientTier tier = new PatientTier();
            if (Request.IsAuthenticated)
            {
                var          userStore   = new UserStore<IdentityUser>();
                var          userManager = new UserManager<IdentityUser>(userStore);
                IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                string       userID      = theUser.Id;
                medicalData.PatientID    = tier.getPatientByID(userID);
                medicalData.TypeID       = 3;
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
                ViewBag.YearList = YearList;
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChartPulse()
        {
            PatientTier tier     = new PatientTier();
            string      month    = Request.Form["MonthSelect"].ToString(); //Get selected month to change data to
            int?        monthInt = Convert.ToDateTime(month + " 01, 1900").Month;
            string      year     = Request.Form["YearSelect"].ToString(); //Get selected month to change data to
            int         yearInt  = Int32.Parse(year);
            return RedirectToAction("ChartPulseByMonth", new { yearInt = yearInt, monthInt= monthInt});
        }
        [HttpGet]
        public ActionResult ChartPulseByMonth(MedicalData medicalData, int? monthInt, int yearInt)
        {
            PatientTier tier     = new PatientTier();
            if (Request.IsAuthenticated)
                {
                    var          userStore   = new UserStore<IdentityUser>();
                    var          userManager = new UserManager<IdentityUser>(userStore);
                    IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                    string       userID      = theUser.Id;
                    medicalData.PatientID    = tier.getPatientByID(userID);
                    medicalData.TypeID       = 3;
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
                ViewBag.YearList = YearList;
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChartPulseByMonth()
        {
            PatientTier tier     = new PatientTier();
            string      month    = Request.Form["MonthSelect"].ToString(); //Get selected month to change data to
            int?        monthInt = Convert.ToDateTime(month + " 01, 1900").Month;
            string      year     = Request.Form["YearSelect"].ToString(); //Get selected month to change data to
            int         yearInt  = Int32.Parse(year);
            return RedirectToAction("ChartPulseByMonth", new { yearInt = yearInt, monthInt= monthInt});
        }
        [HttpGet]
        public ActionResult ChartWeight(MedicalData medicalData)
        {
            PatientTier tier = new PatientTier();
            if (Request.IsAuthenticated)
            {
                var          userStore   = new UserStore<IdentityUser>();
                var          userManager = new UserManager<IdentityUser>(userStore);
                IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                string       userID      = theUser.Id;
                medicalData.PatientID    = tier.getPatientByID(userID);
                medicalData.TypeID       = 4;
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
                ViewBag.YearList = YearList;
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChartWeight()
        {
            PatientTier tier     = new PatientTier();
            string      month    = Request.Form["MonthSelect"].ToString(); //Get selected month to change data to
            int?        monthInt = Convert.ToDateTime(month + " 01, 1900").Month;
            string      year     = Request.Form["YearSelect"].ToString(); //Get selected month to change data to
            int         yearInt  = Int32.Parse(year);
            return RedirectToAction("ChartWeightByMonth", new { yearInt = yearInt, monthInt= monthInt});
        }
        [HttpGet]
        public ActionResult ChartWeightByMonth(MedicalData medicalData, int? monthInt, int yearInt)
        {
            PatientTier tier     = new PatientTier();
            if (Request.IsAuthenticated)
            {
                    var          userStore   = new UserStore<IdentityUser>();
                    var          userManager = new UserManager<IdentityUser>(userStore);
                    IdentityUser theUser     = userManager.FindById(User.Identity.GetUserId());
                    string       userID      = theUser.Id;
                    medicalData.PatientID    = tier.getPatientByID(userID);
                    medicalData.TypeID       = 4;
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
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChartWeightByMonth()
        {
            PatientTier tier     = new PatientTier();
            string      month    = Request.Form["MonthSelect"].ToString(); //Get selected month to change data to
            int?        monthInt = Convert.ToDateTime(month + " 01, 1900").Month;
            string      year     = Request.Form["YearSelect"].ToString(); //Get selected month to change data to
            int         yearInt  = Int32.Parse(year);
            return RedirectToAction("ChartWeightByMonth", new { yearInt = yearInt, monthInt= monthInt});
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