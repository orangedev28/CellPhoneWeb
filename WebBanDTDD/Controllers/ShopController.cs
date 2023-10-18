using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDTDD.Models;
using WebBanDTDD.ViewModels;


namespace WebBanDTDD.Controllers
{
    public class ShopController : Controller
    {
        private readonly Entities _dbContext;


        public ShopController()
        {
            _dbContext = new Entities();
        }


        public ActionResult Index(int? Page, int? cate, string Search_Data, string Filter_Value)
       {
            // 1. Tham số int? dùng để thể hiện null và kiểu int
            // page có thể có giá trị là null và kiểu int.
            if (cate != null)
            {
                Session["cate"] = cate;
            }
            else
            {
                cate=(int?)Session["cate"];
            }
           
            Session["Namecate"] = _dbContext.ProductCategories.Where(c => c.CateID == cate).First().Name;

            if (Search_Data != null)
            {
                Page = 1;
            }
            else
            {
                Search_Data = Filter_Value;
            }

            ViewBag.FilterValue = Search_Data;

            var products = from p in _dbContext.Products 
                           where p.CateID==cate
                           select p;

            if (!String.IsNullOrEmpty(Search_Data))
            {
                products = products.Where(p => p.Name.ToUpper().Contains(Search_Data.ToUpper()));
            }
            if(Session["money"]!=null|| Session["color"] != null|| Session["capacity"] != null)
            {
                int money = 0;
                int capacity = 0;
                string tempmoney = (string)Session["money"];
                string color = (string)Session["color"];
                string tempcapacity = (string)Session["capacity"];
                if(tempmoney!=null)
                {
                     money = int.Parse(tempmoney);
                     
                }
                if (tempcapacity != null)
                {
                    capacity = int.Parse(tempcapacity);
                } 
                

                if(tempmoney != null && tempcapacity == null && tempcapacity == null)
                {
                    products = products.Where(p => p.Price >= money);
                }else if(tempmoney == null && tempcapacity != null && tempcapacity == null)
                {
                    products = products.Where(p => p.Color.ToUpper().Contains(color.ToUpper()));
                }
                else if(tempmoney == null && tempcapacity == null && tempcapacity != null)
                {
                    products = products.Where(p => p.Capacity == capacity);
                }
                else if(tempmoney != null && tempcapacity != null && tempcapacity == null)
                {
                    products = products.Where(p => p.Price >= money && p.Color.ToUpper().Contains(color.ToUpper()));
                }else if(tempmoney != null && tempcapacity == null && tempcapacity != null)
                {
                    products = products.Where(p => p.Price >= money && p.Capacity == capacity);
                }else if(tempmoney == null && tempcapacity != null && tempcapacity != null)
                {
                    products = products.Where(p =>p.Color.ToUpper().Contains(color.ToUpper()) && p.Capacity == capacity);
                }
                else
                {
                    products = products.Where(p => p.Price >= money && p.Color.ToUpper().Contains(color.ToUpper()) && p.Capacity == capacity);
                }
                Session["money"] = null;
                Session["color"] = null;
                Session["capacity"] = null;
            }
            products = products.OrderBy(p => p.ProductID);
            int Size_Of_Page = 6;
            int No_Of_Page = (Page ?? 1);

            return View(products.ToPagedList(No_Of_Page, Size_Of_Page));
        }


        public ActionResult Brand(int? page, int? brand)
        {
            // 1. Tham số int? dùng để thể hiện null và kiểu int
            // page có thể có giá trị là null và kiểu int.
            Session["brand"] = brand;
            Session["Namebrand"] = _dbContext.Brands.Where(c => c.BrandID == brand).First().Name;
            // 2. Nếu page = null thì đặt lại là 1.
            if (page == null) page = 1;

            // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo LinkID mới có thể phân trang.
            var links = (from l in _dbContext.Products
                         where l.BrandID == brand
                         select l).OrderBy(x => x.ProductID);



            // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 6;

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            // 5. Trả về các Link được phân trang theo kích thước và số trang.
            return View(links.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Filter(string money, string color, string capacity)
        {
            
            Session["money"] = money;
            Session["color"] = color;
            Session["capacity"] = capacity;
            return RedirectToAction("Index");
        }
        public ActionResult Product(int? id)
        {
            Product products =_dbContext.Products.Find(id);
            if(products == null)
            {
                return RedirectToAction("Index");
            }    

            return View(products);
        }
    }
}