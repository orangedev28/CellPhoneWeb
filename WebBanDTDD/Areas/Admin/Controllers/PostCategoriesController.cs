using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanDTDD.Models;

namespace WebBanDTDD.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class PostCategoriesController : Controller
    {
        private Entities db = new Entities();

        // GET: Admin/PostCategories
        public ActionResult Index()
        {
            return View(db.PostCategories.ToList());
        }

        // GET: Admin/PostCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostCategory postCategory = db.PostCategories.Find(id);
            if (postCategory == null)
            {
                return HttpNotFound();
            }
            return View(postCategory);
        }

        // GET: Admin/PostCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/PostCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CateID,Name,SeoTitle,Status,Sort,ParentID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] PostCategory postCategory)
        {
            if (ModelState.IsValid)
            {
                db.PostCategories.Add(postCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(postCategory);
        }

        // GET: Admin/PostCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostCategory postCategory = db.PostCategories.Find(id);
            if (postCategory == null)
            {
                return HttpNotFound();
            }
            return View(postCategory);
        }

        // POST: Admin/PostCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CateID,Name,SeoTitle,Status,Sort,ParentID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] PostCategory postCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(postCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(postCategory);
        }

        // GET: Admin/PostCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostCategory postCategory = db.PostCategories.Find(id);
            if (postCategory == null)
            {
                return HttpNotFound();
            }
            db.PostCategories.Remove(postCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Admin/PostCategories/Delete/5
       [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PostCategory postCategory = db.PostCategories.Find(id);
            db.PostCategories.Remove(postCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
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
