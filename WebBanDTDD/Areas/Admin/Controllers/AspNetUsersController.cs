using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanDTDD.Models;
using WebBanDTDD.ViewModels;

namespace WebBanDTDD.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class AspNetUsersController : Controller
    {
        private Entities db = new Entities();
        // GET: Admin/AspNetUsers
        public ActionResult Index()
        {
            return View(db.Acount_temp.ToList());
        }

        // GET: Admin/AspNetUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            var aspNetUser =db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: Admin/AspNetUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Acount_temp  acount_Temp)
        {

            var aspNetUser = db.AspNetUsers.Find(acount_Temp.Id);
            aspNetUser.Status = acount_Temp.Status;

            //var roles = UserManager.GetRolesAsync(acount_Temp.Id);
            //UserManager.RemoveFromRolesAsync(acount_Temp.Id, roles.ToString());

            ////then add new role 
            //UserManager.AddToRoleAsync(acount_Temp.Id, acount_Temp.Name);

            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetUser);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
