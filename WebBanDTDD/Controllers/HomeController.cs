using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDTDD.ViewModels;
using WebBanDTDD.Models;
namespace WebBanDTDD.Controllers
{
    public class HomeController : Controller
    {
        private readonly Entities _dbContext;

        public HomeController()
        {
            _dbContext = new Entities();
        }
        public ActionResult Index()
        {
           
            try {
                var listblogs = (from t in _dbContext.Posts
                                 orderby t.CreatedBy descending
                                 select t).Take(3);

                var HomeViewModel = new HomeViewModel
                {
                    NewArrivals = _dbContext.Products.Take(15),
                    BestSellers = _dbContext.Products.Take(10),
                    //LatestBlogs = listblogs
                };

                return View(HomeViewModel);

            }
            catch(Exception ex) { 
            }
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}