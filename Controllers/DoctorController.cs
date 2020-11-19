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
        public ActionResult EditDoctorInfo(Doctor doctor)
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
                doctor                   = tier.retrieveDoctor(doctor.DoctorID);
                ViewBag.doctor           = doctor;
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
    }
}