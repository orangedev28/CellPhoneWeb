using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanDTDD.Models;

namespace WebBanDTDD.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ProductsController : Controller
    {

        private Entities db = new Entities();

        // GET: Admin/Products
        public ActionResult Index(string Sorting_Order, string Search_Data, string Filter_Value, int? Page_No)
        {
            ViewBag.CurrentSortOrder = Sorting_Order;
            ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "Name_Description" : "";
            ViewBag.SortingPrice = Sorting_Order == "Price_Enroll" ? "Price_Description" : "Price_Enroll";
            ViewBag.SortingQuantity = Sorting_Order == "Quantity_Enroll" ? "Quantity_Description" : "Quantity_Enroll";
           
            if (Search_Data != null)
            {
                Page_No = 1;
            }
            else
            {
                Search_Data = Filter_Value;
            }

            ViewBag.FilterValue = Search_Data;

            var  products= from p in db.Products select p;

            if (!String.IsNullOrEmpty(Search_Data))
            {
                products = products.Where(p => p.Name.ToUpper().Contains(Search_Data.ToUpper()));
            }
            switch (Sorting_Order)
            {
                case "Name_Description":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                case "Price_Enroll":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "Price_Description":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "Quantity_Enroll":
                    products = products.OrderBy(p => p.Quantity);
                    break;
                case "Quantity_Description":
                    products = products.OrderByDescending(p => p.Quantity);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }

            int Size_Of_Page = 4;
            int No_Of_Page = (Page_No ?? 1);
            return View(products.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "Name");
            ViewBag.CateID = new SelectList(db.ProductCategories, "CateID", "Name");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ProductID,Name,Status,Image,Listimage,Price,PromotionPrice,VAT,Quantity,Warranty,Hot,Description,Detail,CateID,BrandID,SupplierID,Tags,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,ViewCount,Capacity,Color")] Product product)
        {
          
            if (ModelState.IsValid)
            {
                //set default
                if (product.Status == null)
                {
                    product.Status = true;
                }
                product.CreatedDate = DateTime.Now;


                HttpFileCollectionBase files = Request.Files;
                var Listimage = "";
                var Image = "";
                if (!string.IsNullOrEmpty(product.ToString()))
                {

                    for (int i = 0; i < files.Count; i++)
                    {
                        var x = files.GetKey(i);
                        if (files.GetKey(i) == "files")
                        {
                            HttpPostedFileBase file = Request.Files.Get(i);
                            Guid guid = Guid.NewGuid();
                            byte[] ByteImgArray;
                            ByteImgArray = ConvertToBytes(file);
                            var ImageQuality = ConfigurationManager.AppSettings["ImageQuality"];
                            if (ByteImgArray.Count() > 0)
                            {
                                var reduceIMage = ReduceImageSize(ByteImgArray, ImageQuality);

                                var fileName = Path.GetFileName(file.FileName);
                                var ext = Path.GetExtension(file.FileName);
                                string name = Path.GetFileNameWithoutExtension(fileName);
                                string myfile = String.Format("{0}_{1}{2}", name, guid, ext);
                                Listimage += "/Uploadimages/" + myfile + ",";
                                string serverMapPath = Server.MapPath("~/Uploadimages/");
                                string filePath = serverMapPath + "//" + myfile;
                                SaveFile(reduceIMage, filePath, file.FileName);
                            }

                        }
                        else if (files.GetKey(i) == "file")
                        {
                            HttpPostedFileBase file = Request.Files.Get(i);
                            Guid guid = Guid.NewGuid();
                            byte[] ByteImgArray;
                            ByteImgArray = ConvertToBytes(file);
                            var ImageQuality = ConfigurationManager.AppSettings["ImageQuality"];
                            if (ByteImgArray.Count() > 0)
                            {
                                var reduceIMage = ReduceImageSize(ByteImgArray, ImageQuality);
                                var fileName = Path.GetFileName(file.FileName);
                                var ext = Path.GetExtension(file.FileName);
                                string name = Path.GetFileNameWithoutExtension(fileName);
                                string myfile = String.Format("{0}_{1}{2}", name, guid, ext);
                                Image = "/Uploadimages/" + myfile;
                                string serverMapPath = Server.MapPath("~/Uploadimages/");
                                string filePath = serverMapPath + "//" + myfile;
                                SaveFile(reduceIMage, filePath, file.FileName);
                            }

                        }
                    }
                    
                    if (Listimage.Length > 1)
                        product.Listimage = Listimage.Remove(Listimage.Length - 1);
                    product.Image = Image;

                    if(string.IsNullOrEmpty(product.Image)|| string.IsNullOrEmpty(product.Listimage))
                    {
                        ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "Name", product.BrandID);
                        ViewBag.CateID = new SelectList(db.ProductCategories, "CateID", "Name", product.CateID);
                        ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", product.SupplierID);
                        return View(product);
                    }

                }

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "Name", product.BrandID);
            ViewBag.CateID = new SelectList(db.ProductCategories, "CateID", "Name", product.CateID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", product.SupplierID);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "Name", product.BrandID);
            ViewBag.CateID = new SelectList(db.ProductCategories, "CateID", "Name", product.CateID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", product.SupplierID);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,Name,Status,Image,Listimage,Price,PromotionPrice,VAT,Quantity,Warranty,Hot,Description,Detail,CateID,BrandID,SupplierID,Tags,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,ViewCount,Capacity,Color")] Product product)
        {
            product = db.Products.Find(product.ProductID);
            if (ModelState.IsValid)
            {
                //set default
                product.UpdatedDate = DateTime.Now;


                HttpFileCollectionBase files = Request.Files;
                var Listimage = "";
                var Image = "";
                if (!string.IsNullOrEmpty(product.ToString()))
                {

                    for (int i = 0; i < files.Count; i++)
                    {
                        var x = files.GetKey(i);
                        if (files.GetKey(i) == "files")
                        {
                            HttpPostedFileBase file = Request.Files.Get(i);
                            Guid guid = Guid.NewGuid();
                            byte[] ByteImgArray;
                            ByteImgArray = ConvertToBytes(file);
                            var ImageQuality = ConfigurationManager.AppSettings["ImageQuality"];
                            if (ByteImgArray.Count() > 0)
                            {
                                //delete fied old
                                try
                                {
                                    if (!string.IsNullOrEmpty(product.Listimage))
                                    {
                                        string[] filesold = product.Listimage.Split(',');
                                        foreach (var fileold in filesold)
                                        {
                                            string fullPath1 = Request.MapPath("~" + fileold);
                                            if (System.IO.File.Exists(fullPath1))
                                            {
                                                System.IO.File.Delete(fullPath1);
                                            }
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    string fullPath1 = Request.MapPath("~" + product.Listimage);
                                    if (System.IO.File.Exists(fullPath1))
                                    {
                                        System.IO.File.Delete(fullPath1);
                                    }
                                }




                                var reduceIMage = ReduceImageSize(ByteImgArray, ImageQuality);

                                var fileName = Path.GetFileName(file.FileName);
                                var ext = Path.GetExtension(file.FileName);
                                string name = Path.GetFileNameWithoutExtension(fileName);
                                string myfile = String.Format("{0}_{1}{2}", name, guid, ext);
                                Listimage += "/Uploadimages/" + myfile + ",";
                                string serverMapPath = Server.MapPath("~/Uploadimages/");
                                string filePath = serverMapPath + "//" + myfile;
                                SaveFile(reduceIMage, filePath, file.FileName);
                            }

                        }
                        else if (files.GetKey(i) == "file")
                        {
                            HttpPostedFileBase file = Request.Files.Get(i);
                            Guid guid = Guid.NewGuid();
                            byte[] ByteImgArray;
                            ByteImgArray = ConvertToBytes(file);
                            var ImageQuality = ConfigurationManager.AppSettings["ImageQuality"];
                            if (ByteImgArray.Count() > 0)
                            {
                                //delete file old
                                string fullPath = Request.MapPath("~" + product.Image);
                                if (System.IO.File.Exists(fullPath))
                                {
                                    System.IO.File.Delete(fullPath);
                                }

                                var reduceIMage = ReduceImageSize(ByteImgArray, ImageQuality);
                                var fileName = Path.GetFileName(file.FileName);
                                var ext = Path.GetExtension(file.FileName);
                                string name = Path.GetFileNameWithoutExtension(fileName);
                                string myfile = String.Format("{0}_{1}{2}", name, guid, ext);
                                Image = "/Uploadimages/" + myfile;
                                string serverMapPath = Server.MapPath("~/Uploadimages/");
                                string filePath = serverMapPath + "//" + myfile;
                                SaveFile(reduceIMage, filePath, file.FileName);
                            }

                        }
                    }

                    if (Listimage.Length > 1)
                        product.Listimage = Listimage.Remove(Listimage.Length - 1);
                    if (!string.IsNullOrEmpty(Image))
                    {
                        product.Image = Image;
                    }


                    if (string.IsNullOrEmpty(product.Image) || string.IsNullOrEmpty(product.Listimage))
                    {
                        ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "Name", product.BrandID);
                        ViewBag.CateID = new SelectList(db.ProductCategories, "CateID", "Name", product.CateID);
                        ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", product.SupplierID);
                        return View(product);
                    }

                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "Name", product.BrandID);
            ViewBag.CateID = new SelectList(db.ProductCategories, "CateID", "Name", product.CateID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", product.SupplierID);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            try
            {
                if (!string.IsNullOrEmpty(product.Image))
                {
                    string fullPath2 = Request.MapPath("~" + product.Image);
                    if (System.IO.File.Exists(fullPath2))
                    {
                        System.IO.File.Delete(fullPath2);
                    }
                }
                try
                {
                    if (!string.IsNullOrEmpty(product.Listimage))
                    {
                        string[] filesold = product.Listimage.Split(',');
                        foreach (var fileold in filesold)
                        {
                            string fullPath1 = Request.MapPath("~" + fileold);
                            if (System.IO.File.Exists(fullPath1))
                            {
                                System.IO.File.Delete(fullPath1);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    string fullPath1 = Request.MapPath("~" + product.Listimage);
                    if (System.IO.File.Exists(fullPath1))
                    {
                        System.IO.File.Delete(fullPath1);
                    }
                }
                db.Products.Remove(product);
                db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
           
           
            return RedirectToAction("Index");
            
        }

        // POST: Admin/Products/Delete/5
       // [HttpPost, ActionName("Delete")]
       // [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Product product = db.Products.Find(id);
            try
            {
                if(string.IsNullOrEmpty(product.Listimage))
                {
                    string[] filesold = product.Listimage.Split(',');
                    foreach (var fileold in filesold)
                    {
                        string fullPath1 = Request.MapPath("~" + fileold);
                        if (System.IO.File.Exists(fullPath1))
                        {
                            System.IO.File.Delete(fullPath1);
                        }
                    }
                }
               
               

            }
            catch (Exception ex)
            {
               
            }
            try
            {
                if (string.IsNullOrEmpty(product.Image))
                {
                    string fullPath2 = Request.MapPath("~" + product.Image);
                    if (System.IO.File.Exists(fullPath2))
                    {
                        System.IO.File.Delete(fullPath2);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] CoverImageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            CoverImageBytes = reader.ReadBytes((int)image.ContentLength);
            return CoverImageBytes;
        }
        public static byte[] ReduceImageSize(byte[] inputBytes, string ImageQuality)
        {
            Byte[] imageBytes;
            int jpegQuality;

            //string ImageQuality = "";/*ConfigurationManager.AppSettings["ImageQuality"];*/
            if (!string.IsNullOrEmpty(ImageQuality))
            {
                jpegQuality = Convert.ToInt32(ImageQuality);
            }
            else
            {
                jpegQuality = 25;
            }

            System.Drawing.Image image;

            using (var inputStream = new MemoryStream(inputBytes))
            {
                // Create an Encoder object based on the GUID  for the Quality parameter category.  
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                image = System.Drawing.Image.FromStream(inputStream);
                var jpegEncoder = ImageCodecInfo.GetImageDecoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                var encoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                encoderParameters.Param[0] = myEncoderParameter;
                using (var outputStream = new MemoryStream())
                {
                    image.Save(outputStream, jpegEncoder, encoderParameters);
                    imageBytes = outputStream.ToArray();
                }
            }
            return imageBytes;
        }
        public string SaveFile(byte[] file, string filePath, string filename)
        {
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
                System.IO.Directory.CreateDirectory(directoryPath);

            if (file != null)
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(file)))
                {
                    //var i = Image.FromFile(filePath + file);
                    //var i2 = new Bitmap(i);
                    if (filename.ToLower().Contains(".jpg") || filename.ToLower().Contains(".jpeg"))
                    {
                        image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        //i2.Save(filePath, ImageFormat.Jpeg);
                    }
                    else if (filename.ToLower().Contains(".png"))
                    {
                        image.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                        //i2.Save(filePath, ImageFormat.Png);
                    }
                    else if (filename.ToLower().Contains(".bmp"))
                    {
                        image.Save(filePath, System.Drawing.Imaging.ImageFormat.Bmp);
                        //i2.Save(filePath, ImageFormat.Bmp);
                    }
                    else if (filename.ToLower().Contains(".gif"))
                    {
                        image.Save(filePath, System.Drawing.Imaging.ImageFormat.Gif);
                        //i2.Save(filePath, ImageFormat.Gif);
                    }
                }
            }
            return string.Empty;
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
