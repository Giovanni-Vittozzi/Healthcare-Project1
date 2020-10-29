using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HealthcareCompanion.Models;
using System.Threading.Tasks;
using Owin;
using Microsoft.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using HealthcareCompanion.DataAccessLayer;

namespace HealthcareCompanion.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {

            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Registration()
        {

            return View();
        }

        [AllowAnonymous]//so this makes it ok 
        // distinguish patient registration from doctor registration
        [HttpPost]
        public async Task<ActionResult> Registration(Patient patient)
        {
            if (ModelState.IsValid)
            {
                bool processError = false;

                //First Create the user using Identity Objects
                var userStore        = new UserStore<IdentityUser>();
                var userManager      = new UserManager<IdentityUser>(userStore);
                string statusMessage = "";

                IdentityUser theUser     = new IdentityUser() { UserName = patient.Email, Email = patient.Email };
                IdentityResult theResult = await userManager.CreateAsync(theUser, patient.Password);

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
                //if (processError)
                //{
                //    ErrorModel error   = new ErrorModel();
                //    error.Location     = "Creating a new user in Identity";
                //    error.ErrorMessage = statusMessage;
                //    //Error Page for creating a user
                //    return View("Error", "Employee", error);
                    //create a view to refer for errors
                    //in employee controller (second attribute here is the controller
                //}

                //Add code to add the rest of the information for the user in the patient table
                PatientTier tier = new PatientTier();
                patient.userID       = theUser.Id;
                tier.insertPatient(patient);
                //boolean pending in the model and set it to true here to let the user know on login that its pending
                //field in patient table of bit type called pending
                //or we could check if they are only in the user role //pending boolean AND user role

                //List<IdentityUser> userList = userManager.Users.ToList<IdentityUser>();
                return RedirectToAction("Default/Index");
            }
            return View();
        }
    }
}