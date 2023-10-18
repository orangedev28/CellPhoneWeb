using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanDTDD.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class homeController : Controller
    {
        // GET: Admin/home
        public ActionResult Index()
        {
            return View();
        }
    }
}