using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDTDD.Models;

namespace WebBanDTDD.Controllers
{
    public class UserController : Controller
    {
        ApplicationDbContext context= new ApplicationDbContext();
        // GET: User
        public ActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                Session["UserName"] =User.Identity.GetUserName();

                if (!isAdminUser())
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "home", new { area = "Admin" });
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin" )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}