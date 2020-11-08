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
    //[RequireHttps]
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult PatientRegistration()
        {
            //var list = new List<string>() { "USA", "Test", "123" };
            DoctorTier tier         = new DoctorTier();
            List<SelectListItem> doctorList = tier.listAllDoctors();
            ViewBag.list = doctorList;
            return View();
        }
        [AllowAnonymous]//so this makes it ok 
        [HttpPost]
        public async Task<ActionResult> PatientRegistration(Patient patient)
        {
            if (ModelState.IsValid)
            {
                bool processError = false;
                //First Create the user using Identity Objects
                var    userStore     = new UserStore<IdentityUser>();
                var    userManager   = new UserManager<IdentityUser>(userStore);
                string statusMessage = "";

                IdentityUser   theUser   = new IdentityUser() { UserName = patient.Email, Email = patient.Email };
                IdentityResult theResult = await userManager.CreateAsync(theUser, patient.Password);

                if (theResult == IdentityResult.Success)
                {
                    //First check to see if User Role exists.  If not create it and add user to User role.
                    var roleStore        = new RoleStore<IdentityRole>();
                    var roleManager      = new RoleManager<IdentityRole>(roleStore);
                    IdentityRole theRole = await roleManager.FindByNameAsync("User");

                    if (theRole == null)
                    {
                        //The user role does not exist, create it.
                        theRole   = new IdentityRole("User");
                        theResult = null;
                        theResult = await roleManager.CreateAsync(theRole);
                        if (theResult == null)
                        {
                            statusMessage = string.Format("User Group Creation failed because : {0}", theResult.Errors.FirstOrDefault());
                            //Need to exit nicely here for some reason, we could not create the role with the DB
                            processError  = true;
                        }

                    }
                    if (!processError)
                    {
                        //The role exists, now add the user to the role
                        theResult     = await userManager.AddToRoleAsync(theUser.Id, "User");
                        statusMessage = string.Format("Identity User {0} was created successfully!<br /> {0} was added to the User group: {1}", theUser.UserName, theResult.Errors.FirstOrDefault());
                    }
                }
                else
                {
                    //could not create a user
                    statusMessage = string.Format("User Creation failed because : {0}", theResult.Errors.FirstOrDefault());
                    processError  = true;
                }

                //If an occurred with user creation, post the error to the end user.
                if (processError)
                {
                    ErrorModel error   = new ErrorModel();
                    error.Location     = "Creating a new user in Identity";
                    error.ErrorMessage = statusMessage;
                    //Error Page for creating a user
                    //return View("Error", "Employee", error); //need a proper view for this
                    //create a view to refer for errors
                    //in employee controller (second attribute here is the controller)
                }
                //Add code to add the rest of the information for the user in the patient table
                PatientTier tier = new PatientTier();
                patient.userID   = theUser.Id;
                patient.Pending  = true;
                tier.insertPatient(patient);
                //boolean pending in the model and set it to true here to let the user know on login that its pending
                //field in patient table of bit type called pending
                //or we could check if they are only in the user role //pending boolean AND user role

                List<IdentityUser> userList = userManager.Users.ToList<IdentityUser>();
                //is this the right list to return?
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult DoctorRegistration()
        {

            //Need to create View here
            return View();
        }

        [AllowAnonymous]//so this makes it ok 
        [HttpPost]
        public async Task<ActionResult> DoctorRegistration(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                bool processError = false;
                //First Create the user using Identity Objects
                var userStore        = new UserStore<IdentityUser>();
                var userManager      = new UserManager<IdentityUser>(userStore);
                string statusMessage = "";

                IdentityUser theUser     = new IdentityUser() { UserName = doctor.Email, Email = doctor.Email };
                IdentityResult theResult = await userManager.CreateAsync(theUser, doctor.Password);

                if (theResult == IdentityResult.Success)
                {
                    //First check to see if User Role exists.  If not create it and add user to User role.
                    var roleStore   = new RoleStore<IdentityRole>();
                    var roleManager = new RoleManager<IdentityRole>(roleStore);
                    IdentityRole theRole = await roleManager.FindByNameAsync("User");

                    if (theRole == null)
                    {
                        //The user role does not exist, create it.
                        theRole   = new IdentityRole("User");
                        theResult = null;
                        theResult = await roleManager.CreateAsync(theRole);
                        if (theResult == null)
                        {
                            statusMessage = string.Format("User Group Creation failed because : {0}", theResult.Errors.FirstOrDefault());
                            //Need to exit nicely here for some reason, we could not create the role with the DB
                            processError = true;
                        }
                    }
                    if (!processError)
                    {
                        //The role exists, now add the user to the role
                        theResult     = await userManager.AddToRoleAsync(theUser.Id, "User");
                        statusMessage = string.Format("Identity User {0} was created successfully!<br /> {0} was added to the User group: {1}", theUser.UserName, theResult.Errors.FirstOrDefault());
                    }
                }
                else
                {
                    //could not create a user
                    statusMessage = string.Format("User Creation failed because : {0}", theResult.Errors.FirstOrDefault());
                    processError  = true;
                }
                //If an occurred with user creation, post the error to the end user.
                if (processError)
                {
                    ErrorModel error   = new ErrorModel();
                    error.Location     = "Creating a new user in Identity";
                    error.ErrorMessage = statusMessage;
                    //Error Page for creating a user
                    return View("Error", "Default", error);//need a correct redirect here
                    //create a view to refer for errors
                    //in employee controller (second attribute here is the controller
                }
                //Add code to add the rest of the information for the user in the doctor table
                DoctorTier tier = new DoctorTier();
                doctor.userID   = theUser.Id;
                doctor.Pending  = true;
                tier.insertDoctor(doctor);
                //boolean pending in the model and set it to true here to let the user know on login that its pending
                //field in doctor table of bit type called pending
                //or we could check if they are only in the user role //pending boolean AND user role

                List<IdentityUser> userList = userManager.Users.ToList<IdentityUser>();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
                var authManager = HttpContext.GetOwinContext().Authentication;

                IdentityUser user       = userManager.Find(login.UserName, login.Password);
                PatientTier patientTier = new PatientTier();
                DoctorTier doctorTier   = new DoctorTier();
                var patientUser         = patientTier.isPendingPatient(login.UserName);
                var doctorUser          = doctorTier.isPendingDoctor(login.UserName);
                //isPending
                if (user != null)
                {
                    var ident = userManager.CreateIdentity(user,
                        DefaultAuthenticationTypes.ApplicationCookie);
                    authManager.SignIn(
                        new AuthenticationProperties { IsPersistent = false }, ident);
                    //this code right here officer  && !pendingDoctor
                    if (patientUser.pendingCheck       && patientUser.emailCheck && !doctorUser.emailCheck) { return Redirect(login.ReturnUrl ?? Url.Action("Pending", "Patient")); }
                    else if (!patientUser.pendingCheck && patientUser.emailCheck && !doctorUser.emailCheck) { return Redirect(login.ReturnUrl ?? Url.Action("NotPending", "Patient")); }
                    else if (doctorUser.pendingCheck   && doctorUser.emailCheck  && !patientUser.emailCheck) { return Redirect(login.ReturnUrl ?? Url.Action("Pending", "Doctor")); }
                    else if (!doctorUser.pendingCheck  && doctorUser.emailCheck  && !patientUser.emailCheck) { return Redirect(login.ReturnUrl ?? Url.Action("NotPending", "Doctor")); }
                }
            }
            ModelState.AddModelError("", "Invalid username or password");
            return View(login);
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