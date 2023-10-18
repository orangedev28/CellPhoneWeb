using PagedList;
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
    public class OrdersController : Controller
    {
        private Entities db = new Entities();

        // GET: Admin/Orders


        public ActionResult Index(string Sorting_Order, string Search_Data, string Filter_Value, int? Page_No)
        {
            ViewBag.CurrentSortOrder = Sorting_Order;
            ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "Name_Description" : "";
            ViewBag.SortingDeliveredDate = Sorting_Order == "DeliveredDate_Enroll" ? "DeliveredDate_Description" : "DeliveredDate_Enroll";
            ViewBag.SortingOrderDate = Sorting_Order == "OrderDate_Enroll" ? "OrderDate_Description" : "OrderDate_Enroll";

            if (Search_Data != null)
            {
                Page_No = 1;
            }
            else
            {
                Search_Data = Filter_Value;
            }

            ViewBag.FilterValue = Search_Data;

            var Order = from p in db.V_OrderTemp select p;



            if (!String.IsNullOrEmpty(Search_Data))
            {
                int x = Int32.Parse(Search_Data);
                Order = Order.Where(p => p.OrderID == x);
            }
            switch (Sorting_Order)
            {
                case "Name_Description":
                    Order = Order.OrderByDescending(p => p.OrderID);
                    break;
                case "DeliveredDate_Enroll":
                    Order = Order.OrderBy(p => p.DeliveredDate);
                    break;
                case "DeliveredDate_Description":
                    Order = Order.OrderByDescending(p => p.DeliveredDate);
                    break;
                case "OrderDate_Enroll":
                    Order = Order.OrderBy(p => p.OrderDate);
                    break;
                case "OrderDate_Description":
                    Order = Order.OrderByDescending(p => p.OrderDate);
                    break;
                default:
                    Order = Order.OrderBy(p => p.OrderID);
                    break;
            }

            int Size_Of_Page = 8;
            int No_Of_Page = (Page_No ?? 1);
            return View(Order.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Admin/Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Status,Delivered,Discount")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Admin/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,OrderDate,Status,Delivered,DeliveredDate,CustomerID,Discount")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Admin/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
