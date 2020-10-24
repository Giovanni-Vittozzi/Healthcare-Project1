using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Owin;
using Microsoft.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using HealthcareCompanion.Models;
using HealthcareCompanion.DataAccessLayer;

namespace HealthcareCompanion.Controllers
{
    /// <summary>
    /// This class is primarily concerned with Website User Administration.
    /// You should be able to enter patient data and User Information.
    /// This is related to the Patient controller and may use methods in 
    /// that controller to edit the patient information that is not related
    /// to the website user data.
    /// Methods that are in this controller include
    /// 1) Index -> general tools that are available
    /// 2) AddUser -> This will just add the user without the information for the 
    ///     patient table
    /// 3) DisplayAllUsers -> This will display all of the Users in the DB
    /// 4) DeleteUser -> This will delete the User from the database.
    /// 
    /// In any case, items that just deal with the user is done in this controller.
    /// </summary>
    /// 
    /// 
    /// adding roles is most important next step, when i approve them we add them to the role
        
    //[RequireHttps]
    //[Authorize(Roles = "Admin")]
    public class UserAdminController : Controller
    {
        // GET: UserAdmin
        public ActionResult Index()
        {

            return View();
        }

        /// <summary>
        /// This is used to get a list of users within the identity database for the website.
        /// </summary>
        /// <returns>Returns a List of Identity Users to the view</returns>
        [HttpGet]
        public ActionResult GetAllUsers()
        {
            var userStore   = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);

            List<IdentityUser> userList = userManager.Users.ToList<IdentityUser>();

            return View(userList);
        }

        /// <summary>
        /// This method will return a list of available roles from the Identity database.
        /// </summary>
        /// <returns>Returns a list of IdentityRole</returns>

        [HttpGet]
        public ActionResult GetAllRoles()
        {
            var roleStore   = new RoleStore<IdentityRole>();
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            List<IdentityRole> roleList = roleManager.Roles.ToList<IdentityRole>();

            return View(roleList);
        }

        [HttpGet]
        public async Task<ActionResult> AllUserNoDetails()
        {
            PatientTier patientTier = new PatientTier();//added info for that user patient or doctor in my case
            List<UserInfo> userList = null;
            try
            {
                //userList = await patientTier.getUsersMissingPatientAsync();
            }catch(Exception ex)
            {
                ErrorModel error   = new ErrorModel();
                error.Location     = "Getting Users wit missing details.";
                error.ErrorMessage = ex.Message;
                error.ActionTaken  = "This error was generated when " +
                    "trying to access the users that do not have patient information attached to them.";

                return View("Error", "Patient", error);
            }

            return View(userList);
        }

        //[HttpGet]
        //public async Task<ActionResult> EditUser(string id)
        //{
        //    PatientTier patientTier = new PatientTier(); //put patient here instead of patient
        //    PatientFormDetail patient = null; //put patient here instead of patient

        //    //First get the details of the user.
        //    var userStore = new UserStore<IdentityUser>();
        //    var userManager = new UserManager<IdentityUser>(userStore);
        //    IdentityUser user = userManager.FindById<IdentityUser, string>(id);

        //    //Now get the patient information, if null is returned, the patient details do
        //    //not exist and return a different detail.

        //    //patient = await patientTier.getPatientFromUserIDAsync(id); //put patient here instead of patient

        //    //No Details exist return just a UserDetailView()
        //    return View(user);

        //}

        //[HttpPost]
        //public async Task<ActionResult> EditUser(IdentityUser user, string token)
        //{
        //    var userStore = new UserStore<IdentityUser>();
        //    var userManager = new UserManager<IdentityUser>(userStore);


        //    if (user.LockoutEnabled)
        //    {

        //        DateTimeOffset dst = new DateTimeOffset(DateTime.UtcNow);
        //        dst = dst.AddDays(10);
        //        await userManager.SetLockoutEnabledAsync(user.Id, user.LockoutEnabled);
        //        await userManager.SetLockoutEndDateAsync(user.Id, dst);
        //    }
        //    else
        //    {
        //        //Have not figured out how to set the date back to null.  Read an 
        //        //article that says it is the date that actually sets the lockout. 
        //        //Setting the Offset to the Current UTC Time, should unlock the account.
        //        await userManager.SetLockoutEndDateAsync(user.Id, new DateTimeOffset(DateTime.UtcNow));
        //        await userManager.SetLockoutEnabledAsync(user.Id, user.LockoutEnabled);

        //    }

        //    await userManager.SetPhoneNumberAsync(user.Id, user.PhoneNumber);


        //    return RedirectToAction("UserDetails", new { id = user.Id });
        //}

        /// <summary>
        /// This method is used to add a user to the database.  This will include all the information for Identity as 
        /// well as all the information needed for the company.
        /// </summary>
        /// <returns>returns a view/form</returns>

        [HttpGet]
        public ActionResult AddUser()
        {

            return View();
        }
        /// <summary>
        /// This method should handle the HttpPost call from the AddUser view.
        /// </summary>
        /// <param name="user">The NewUser containing all the information for both the IdentityUser as well
        /// as the information needed for the company.</param>
        /// <returns>Returns a view that confirms the addition of the site user.</returns>
        public async Task<ActionResult> AddUser(NewUser user)
        {
            if (ModelState.IsValid)
            {
                bool processError = false;
                //First Create the user using Identity Objects
                var userStore        = new UserStore<IdentityUser>();
                var userManager      = new UserManager<IdentityUser>(userStore);
                string statusMessage = "";

                IdentityUser theUser     = new IdentityUser() { UserName = user.Email, Email = user.Email };
                IdentityResult theResult = await userManager.CreateAsync(theUser, user.Password);

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

                    return View("Error","Patient", error);
                }
                List<IdentityUser> userList = userManager.Users.ToList<IdentityUser>();

                return View("GetAllUsers", userList);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddRole(Role role, string userID) //does this need , string userID as a parameter?
        {
            if (ModelState.IsValid)
            {
                var userStore     = new UserStore<IdentityUser>();
                var userManager   = new UserManager<IdentityUser>(userStore);
                IdentityUser user = userManager.FindById<IdentityUser, string>(userID);
                var roleStore     = new RoleStore<IdentityRole>();
                var roleManager   = new RoleManager<IdentityRole>(roleStore);

                if (await roleManager.RoleExistsAsync(role.Name))
                {
                    ErrorModel theError    = new ErrorModel();
                    theError.ReturnURLName = "Add Role";
                    theError.ReturnURL     = "AddRole";
                    theError.Location      = "While Adding a Role";
                    theError.ErrorMessage  = "The role being added already exists";
                    theError.ActionTaken   = "Please return to adding a role and try another role name, or return to the administration page";

                    return RedirectToAction("UserError", theError);
                }
                else
                {
                    await roleManager.CreateAsync(new IdentityRole(role.Name));
                }

                return RedirectToAction("GetAllRoles");
            }
            else
            {
                return View();
            }
            
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        //[HttpGet]
        //public ActionResult AddDetails(string userID)//users without contact information so no
        //{
        //    Patient patient = new Patient();
        //    patient.UserID  = userID;

        //    return View(patient);
        //} //put patient here instead of patient

        //[HttpPost]
        //public async Task<ActionResult> AddDetails(Patient patient)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        PatientTier tier = new PatientTier();
        //        await tier.insertPatientAsync(patient);

        //        return RedirectToAction("UserDetails", new { id = patient.UserID });
        //    }
        //    else
        //    {

        //        return View(patient);
        //    }
        //} //put patient here instead of patient

        //[HttpGet]
        //public async Task<ActionResult> UserDetails(string id)//useful pulls role info and also address 
        //{
        //    PatientTier patientTier   = new PatientTier();
        //    PatientFormDetail patient = null;
        //    UserRoleTier roleTier     = new UserRoleTier(); //put patient here instead of patient

        //    //First get the details of the user.
        //    var userStore     = new UserStore<IdentityUser>();
        //    var userManager   = new UserManager<IdentityUser>(userStore);
        //    IdentityUser user = userManager.FindById<IdentityUser, string>(id);
        //    var roleStore     = new RoleStore<IdentityRole>();
        //    var roleManager   = new RoleManager<IdentityRole>(roleStore);

        //    IList<string> roleList = userManager.GetRoles(user.Id);



        //    //Now get the patient information, if null is returned, the patient details do
        //    //not exist and return a different detail.

        //    patient               = await patientTier.getPatient FromUserIDAsync(id);
        //    ViewBag.AvailRoleList = roleTier.getAvailableRolesList(id);

        //    if (patient == null)
        //    {
        //        //No Details exist return just a UserDetailView()
        //        PatientUserDetail detail = new PatientUserDetail();
        //        detail.user     = user;
        //        detail.patient  = null;
        //        detail.roleList = roleList;

        //        return View(detail);
        //    }
        //    else
        //    {
        //        //Patient Details exist, return  the patient details view()
        //        PatientUserDetail detail = new PatientUserDetail();
        //        detail.user     = user;
        //        detail.patient  = patient;
        //        detail.roleList = roleList;

        //        return View("PatientDetail", detail);
        //    }
        //}

        //[HttpGet]
        //public ActionResult DeleteUser(string id) //useful
        //{
        //    //First get the details of the user.
        //    var userStore     = new UserStore<IdentityUser>();
        //    var userManager   = new UserManager<IdentityUser>(userStore);
        //    IdentityUser user = userManager.FindById<IdentityUser, string>(id);

        //    return View(user);
        //}

        //[HttpPost]
        //[ActionName("DeleteUser")]
        //public async Task<ActionResult> ConfirmDelete(IdentityUser user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        PatientTier theTier = new PatientTier();
        //        Patient patient     = theTier.getPatientFromUserID(user.Id);


        //        //This will delete the patient information if it exists for the user first.
        //        if (patient != null)
        //        {
        //            if (patient.PatientID != null)
        //            {
        //                int id = (int)patient.PatientID;
        //                await theTier.deletePatientAsync(id);
        //            }
        //        }
        //        var userStore   = new UserStore<IdentityUser>();
        //        var userManager = new UserManager<IdentityUser>(userStore);

        //        //This gets a reference to the current context in the store.
        //        var context = userStore.Context;

        //        IdentityUser theUser = await userManager.FindByIdAsync(user.Id);
        //        var logins       = theUser.Logins;
        //        var rolesForUser = await userManager.GetRolesAsync(user.Id);

        //        using (var transaction = context.Database.BeginTransaction())
        //        {
        //            foreach (var login in logins)
        //            {
        //                await userManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
        //            }

        //            if (rolesForUser.Count() > 0)
        //            {
        //                foreach (var item in rolesForUser.ToList())
        //                {
        //                    // item should be the name of the role
        //                    var result = await userManager.RemoveFromRoleAsync(user.Id, item);
        //                }
        //            }
        //            await userManager.DeleteAsync(theUser);
        //            transaction.Commit();
        //        }

        //        List<IdentityUser> userList = userManager.Users.ToList<IdentityUser>();

        //        return View("GetAllUsers", userList);
        //    }

        //    return View("DeleteUser", user);
        //}
        //////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        public ActionResult DeleteRole(string id) //maybe for later useful if we dont want to call patient role 
        {
            var roleStore     = new RoleStore<IdentityRole>();
            var roleManager   = new RoleManager<IdentityRole>(roleStore);
            IdentityRole role = roleManager.FindById<IdentityRole, string>(id);

            if (role.Users != null && role.Users.Count > 0)
            {
                var userStore   = new UserStore<IdentityUser>();
                var userManager = new UserManager<IdentityUser>(userStore);

                List<IdentityUser> userList = new List<IdentityUser>();
                IdentityUser user = null;

                foreach(IdentityUserRole userRole in role.Users)
                {
                    user = userManager.FindById(userRole.UserId);
                    userList.Add(user);
                }
                ViewBag.userList = userList;
            }

            return View(role);
        }

        [HttpPost]
        [ActionName("DeleteRole")]
        public async Task<ActionResult> ConfirmRoleDelete(IdentityRole role) //from a person
        {
            if (ModelState.IsValid)
            {
                var roleStore   = new RoleStore<IdentityRole>();
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                //This gets a reference to the current context in the Role store.
                var context = roleStore.Context;

                IdentityRole theRole = await roleManager.FindByIdAsync(role.Id);
                
                //First remove each user from the role
                if (theRole.Users.Count > 0)
                {
                    var userStore   = new UserStore<IdentityUser>();
                    var userManager = new UserManager<IdentityUser>(userStore);

                    using (var transaction = context.Database.BeginTransaction())
                    {
                        foreach (IdentityUserRole roleUser in theRole.Users)
                        {
                           await userManager.RemoveFromRoleAsync(roleUser.UserId, theRole.Name);
                        }
                        transaction.Commit();
                    }
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    await roleManager.DeleteAsync(theRole);
                    transaction.Commit();
                }
                List<IdentityRole> roleList = roleManager.Roles.ToList<IdentityRole>();

                return View("GetAllRoles", roleList);
            }

            return View("DeleteRole", role);
        }
        ///////////////////////////////////////////////////////////
        //[HttpGet]
        //public ActionResult ChangePassword(string id)
        //{

        //    if (id != null)
        //    {
        //        var userStore   = new UserStore<IdentityUser>();
        //        var userManager = new UserManager<IdentityUser>(userStore);

        //        IdentityUser user = userManager.FindById<IdentityUser, string>(id);

        //        PasswordChangeModel theModel = new PasswordChangeModel();
        //        theModel.Email               = user.Email;
        //        theModel.id                  = user.Id;

        //        return View(theModel);
        //    }
        //    else
        //    {
        //        ErrorModel error    = new ErrorModel();
        //        error.Location      = "Access Not Allowed";
        //        error.ErrorMessage  = "You can not access the change password page directly.";
        //        error.ActionTaken   = "Please go directly to the User List";
        //        error.ReturnURLName = "User List";
        //        error.ReturnURL     = "GetAllUsers";

        //        return RedirectToAction("UserError", error);
        //    }
        //}

        //[HttpPost]
        //public async Task<ActionResult> ChangePassword(PasswordChangeModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userStore   = new UserStore<IdentityUser>();
        //        var userManager = new UserManager<IdentityUser>(userStore);

        //        IdentityUser user = await userManager.FindByIdAsync(model.id);

        //        string newPassword    = model.Password;
        //        string hashedPassword = userManager.PasswordHasher.HashPassword(newPassword);

        //        await userStore.SetPasswordHashAsync(user, hashedPassword);
        //        await userStore.UpdateAsync(user);


        //        List<IdentityUser> userList = userManager.Users.ToList<IdentityUser>();

        //        return RedirectToAction("GetAllUsers", userList);
        //    }
        //    return View(model);
        //}
        ////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        public async Task<ActionResult> AddUserToRole(string roleName, string userID)
        {
            var userStore     = new UserStore<IdentityUser>();
            var userManager   = new UserManager<IdentityUser>(userStore);
            IdentityUser user = userManager.FindById<IdentityUser, string>(userID);
            var roleStore     = new RoleStore<IdentityRole>();
            var roleManager   = new RoleManager<IdentityRole>(roleStore);

            if (await roleManager.RoleExistsAsync(roleName))
            {
                user       = await userManager.FindByIdAsync(userID);
                var result = await userManager.AddToRoleAsync(user.Id, roleName);
            }

            return RedirectToAction("UserDetails", new { id = userID });
        }

        [HttpGet]
        public async Task<ActionResult> RemoveUserFromRole(string roleName, string userID)
        {
            var userStore     = new UserStore<IdentityUser>();
            var userManager   = new UserManager<IdentityUser>(userStore);
            IdentityUser user = userManager.FindById<IdentityUser, string>(userID);
            var roleStore     = new RoleStore<IdentityRole>();
            var roleManager   = new RoleManager<IdentityRole>(roleStore);

            if (await roleManager.RoleExistsAsync(roleName))
            {
                IdentityResult removeRoleResult = await userManager.RemoveFromRoleAsync(userID,roleName);
            }

            return RedirectToAction("UserDetails", new { id = userID });
        }

        [HttpGet]
        public ActionResult UserError(ErrorModel error)
        {

            return View(error);
        }

    }
}