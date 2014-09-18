using LMS.Web.Infrastructure;
using LMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LMS.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel modelData, UserProfile user)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Client");
            }

            ModelState.AddModelError(string.Empty, "There was an error logging you in, please try again.");
            return View("../Home/Index", modelData);
        }
    }
}