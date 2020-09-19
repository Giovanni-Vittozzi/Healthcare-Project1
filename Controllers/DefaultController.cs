using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Healthcare_Companion.Models;

namespace Healthcare_Companion.Controllers
{
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
        [HttpGet]
        public ActionResult About()
        {

            return View();
        }
    }
}