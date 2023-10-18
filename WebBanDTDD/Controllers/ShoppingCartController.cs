using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDTDD.Models;
using WebBanDTDD.ViewModels;

namespace WebBanDTDD.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly Entities _dbContext;

        public ShoppingCartController()
        {
            _dbContext = new Entities();
        }
        

        public ActionResult addShoppingcart(int id)
        {
            List<ShoppingCart> shoppingCarts = shoppingCart();

            ShoppingCart product = shoppingCarts.Find(p => p.Id == id);

            

            
                
                if (product == null)
                {
                    product = new ShoppingCart(id);
                    var check = _dbContext.Products.Where(p => p.ProductID == product.Id && product.Quantity <= p.Quantity).First();
                    if(check != null)
                    shoppingCarts.Add(product);

                    return RedirectToAction("Index");
                }
                else
                {
                    product.Quantity++;
                    return RedirectToAction("Index");
                }
           

          
           
        }
        //public ActionResult addShoppingcarts(int id,int quantity)
        //{
        //    List<ShoppingCart> shoppingCarts = shoppingCart();

        //    ShoppingCart product = shoppingCarts.Find(p => p.Id == id);
        //    if (product == null)
        //    {
        //        product = new ShoppingCart(id);
        //        product.Quantity = quantity;
        //        shoppingCarts.Add(product);
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
              
        //            product.Quantity+=quantity;
                
        //        return RedirectToAction("Index");
        //    }

        //}
        public ActionResult removeShoppingcart(int id)
        {
            List<ShoppingCart> shoppingCarts = shoppingCart();

            ShoppingCart product = shoppingCarts.Find(p => p.Id == id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                if(product.Quantity > 1)
                {
                    product.Quantity--;
                    return RedirectToAction("Index");
                }
                else
                {
                    shoppingCarts.RemoveAll(p => p.Id == id);
                    return RedirectToAction("Index");
                }
            }

        }
        public ActionResult removeItemShoppingCart(int id)
        {
            List<ShoppingCart> shoppingCarts = shoppingCart();

            ShoppingCart product = shoppingCarts.Find(p => p.Id == id);
            if (product != null)
            {
                shoppingCarts.RemoveAll(p=>p.Id==id);
                return RedirectToAction("Index");
            }
            else if(shoppingCarts.Count == 0)
            {
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("Index");
        }
        public ActionResult removeAllShoppingCart()
        {
            List<ShoppingCart> shoppingCarts = shoppingCart();
            shoppingCarts.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: ShoppingCart
        public ActionResult Index()
        {
            List<ShoppingCart> shoppingCarts=shoppingCart();
            if (shoppingCarts.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TotalQuantity = totalQuantity();
            ViewBag.TotalPrice=totalPrice();
            return View(shoppingCarts);
        }

        public ActionResult totalQuantity(string test)
        {
            var TotalQuantity = totalQuantity();

            // some code
            return Json(new { success = true, message = TotalQuantity }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult shoppingCartPartial()
        {

            ViewBag.TotalQuantity = totalQuantity();
            ViewBag.TotalPrice = totalPrice();
            return PartialView();
        }
        private int totalQuantity()
        {
            int totalQuantity = 0;
            List<ShoppingCart> shoppingCarts = Session["shoppingCart"] as List<ShoppingCart>;
            if(shoppingCarts != null)
            {
                totalQuantity = shoppingCarts.Sum(p => p.Quantity);
            }
            return totalQuantity;


        }
        private double totalPrice()
        {
            double totalPrice = 0;
            List<ShoppingCart> shoppingCarts = Session["shoppingCart"] as List<ShoppingCart>;
            if (shoppingCarts != null)
            {
                totalPrice = shoppingCarts.Sum(p => p.PriceTotal);
            }
            return totalPrice;
        }


        public List<ShoppingCart> shoppingCart()
        {
            List<ShoppingCart> shoppingCarts = Session["shoppingCart"] as List<ShoppingCart>;

            if (shoppingCarts == null)
            {
                shoppingCarts= new List<ShoppingCart>();
                Session["shoppingCart"] = shoppingCarts;
            }

            return shoppingCarts;
        }

        public ActionResult checkOut()
        {
            var userId = User.Identity.GetUserId();



            if (userId != null)
            {
                var user = _dbContext.AspNetUsers.Where(p => p.Id == userId).FirstOrDefault();

                Checkout checkout = new Checkout
                {
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,


                };

                return View(checkout);
            }

            return View();
        }
        [HttpPost]
        public ActionResult checkOut(Checkout checkout)
        {
            if (!ModelState.IsValid)
            {
                return View(checkout);
            }
            else
            {
                var user = User.Identity.GetUserId();
                //OrderType==1 thanh toán khi nhận hàng
                //OrderType==2 thanh toán online
                List<ShoppingCart> shoppingCarts = shoppingCart();
                if(shoppingCarts == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (checkout.Ordertype == 1)
                {
                    if (user != null)
                    {
                        var ordertemp = new Order
                        {

                            CustomerID = user,
                            OrderDate = DateTime.Now,
                            Status = false,
                            Delivered = 0,
                            DeliveredDate = DateTime.Now.AddDays(15),
                            DeliveryAddress = checkout.DeliveryAddress,
                            OrderType = checkout.Ordertype,
                            Discount = 0,

                        };
                        _dbContext.Orders.Add(ordertemp);
                        _dbContext.SaveChanges();
                        foreach (var item in shoppingCarts)
                        {
                            var oderdDetails = new OrderDetail
                            {
                                OrderID = ordertemp.OrderID,
                                ProductID = item.Id,
                                ProductName = item.Name,
                                Price = (int)item.Price,
                                Quantity = item.Quantity,

                            };
                            _dbContext.OrderDetails.Add(oderdDetails);
                            var product = _dbContext.Products.Find(item.Id);
                            product.Quantity -= item.Quantity;
                            _dbContext.Entry(product).State = EntityState.Modified;
                        }
                        _dbContext.SaveChanges();

                    }
                    else
                    {
                        var checkcustomerInfortemp = _dbContext.CustomerInfors.Where(p => p.Email == checkout.Email).Count();
                        var customerInfortemp = new CustomerInfor
                        {
                            UserName = checkout.UserName,
                            Email = checkout.Email,
                            PhoneNumber = checkout.PhoneNumber,
                        };
                        if (checkcustomerInfortemp < 0)
                        {

                            _dbContext.CustomerInfors.Add(customerInfortemp);
                            _dbContext.SaveChanges();
                        }
                        else
                        {
                            customerInfortemp.CustomerInforID = _dbContext.CustomerInfors.Where(p => p.Email == checkout.Email).First().CustomerInforID;
                        }

                        var ordertemp = new Order
                        {
                            //sử dụng khi người dùng không đăng ký
                            CustomerID = "0bd81a04-98ef-4a83-8cef-9198c858195e",
                            OrderDate = DateTime.Now,
                            Status = false,
                            Delivered = 0,
                            DeliveredDate = DateTime.Now.AddDays(15),
                            DeliveryAddress = checkout.DeliveryAddress,
                            Discount = 0,
                            CustomerInforID = customerInfortemp.CustomerInforID,
                            OrderType = checkout.Ordertype
                        };
                        _dbContext.Orders.Add(ordertemp);
                        _dbContext.SaveChanges();
                        foreach (var item in shoppingCarts)
                        {
                            var oderdDetails = new OrderDetail
                            {
                                OrderID = ordertemp.OrderID,
                                ProductID = item.Id,
                                ProductName = item.Name,
                                Price = (int)item.Price,
                                Quantity = item.Quantity,

                            };

                            _dbContext.OrderDetails.Add(oderdDetails);

                            var product = _dbContext.Products.Find(item.Id);
                            product.Quantity -= item.Quantity;
                            _dbContext.Entry(product).State = EntityState.Modified;

                        }
                        _dbContext.SaveChanges();
                    }
                    Session["shoppingCart"] = null;
                    return RedirectToAction("ConfirmPaymentClient");
                }
                else
                {
                    if (user != null)
                    {
                        var ordertemp = new Order
                        {

                            CustomerID = user,
                            OrderDate = DateTime.Now,
                            Status = false,
                            Delivered = 0,
                            DeliveredDate = DateTime.Now.AddDays(15),
                            DeliveryAddress = checkout.DeliveryAddress,
                            OrderType = checkout.Ordertype,
                            Discount = 0,

                        };
                        
                        _dbContext.Orders.Add(ordertemp);
                        _dbContext.SaveChanges();
                        Session["ordertempID"] = ordertemp.OrderID;
                        foreach (var item in shoppingCarts)
                        {
                            var oderdDetails = new OrderDetail
                            {
                                OrderID = ordertemp.OrderID,
                                ProductID = item.Id,
                                ProductName = item.Name,
                                Price = (int)item.Price,
                                Quantity = item.Quantity,

                            };
                            _dbContext.OrderDetails.Add(oderdDetails);
                            var product = _dbContext.Products.Find(item.Id);
                            product.Quantity -= item.Quantity;
                            _dbContext.Entry(product).State = EntityState.Modified;
                        }
                        _dbContext.SaveChanges();

                        return RedirectToAction("Payment");
                    }
                    else
                    {
                        var checkcustomerInfortemp = _dbContext.CustomerInfors.Where(p => p.Email == checkout.Email).Count();
                        var customerInfortemp = new CustomerInfor
                        {
                            UserName = checkout.UserName,
                            Email = checkout.Email,
                            PhoneNumber = checkout.PhoneNumber,
                        };
                        if (checkcustomerInfortemp < 0)
                        {

                            _dbContext.CustomerInfors.Add(customerInfortemp);
                            _dbContext.SaveChanges();
                        }
                        else
                        {
                            customerInfortemp.CustomerInforID = _dbContext.CustomerInfors.Where(p => p.Email == checkout.Email).First().CustomerInforID;
                        }

                        var ordertemp = new Order
                        {
                            //sử dụng khi người dùng không đăng ký
                            CustomerID = "0bd81a04-98ef-4a83-8cef-9198c858195e",
                            OrderDate = DateTime.Now,
                            Status = false,
                            Delivered = 0,
                            DeliveredDate = DateTime.Now.AddDays(15),
                            DeliveryAddress = checkout.DeliveryAddress,
                            Discount = 0,
                            CustomerInforID = customerInfortemp.CustomerInforID,
                            OrderType = checkout.Ordertype
                        };
                        _dbContext.Orders.Add(ordertemp);
                        _dbContext.SaveChanges();
                        Session["ordertempID"] = ordertemp.OrderID;
                        foreach (var item in shoppingCarts)
                        {
                            var oderdDetails = new OrderDetail
                            {
                                OrderID = ordertemp.OrderID,
                                ProductID = item.Id,
                                ProductName = item.Name,
                                Price = (int)item.Price,
                                Quantity = item.Quantity,

                            };

                            _dbContext.OrderDetails.Add(oderdDetails);

                            var product = _dbContext.Products.Find(item.Id);
                            product.Quantity -= item.Quantity;
                            _dbContext.Entry(product).State = EntityState.Modified;

                        }
                        _dbContext.SaveChanges();
                       return RedirectToAction("Payment");
                    }
                  
                }
              
            }
           
        }
        public ActionResult Payment()
        {

            int id = (int)Session["ordertempID"];

            var order = _dbContext.V_OrderTemp.Find(id);

            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = order.TotalOrder.ToString();
            string returnUrl = "https://localhost:1844/Home/ConfirmPaymentClient";
            string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = "1000";
            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);
            Session["shoppingCart"] = null;
            return Redirect(jmessage.GetValue("payUrl").ToString());
        }

        //Khi thanh toán xong ở cổng thanh toán Momo, Momo sẽ trả về một số thông tin, trong đó có errorCode để check thông tin thanh toán
        //errorCode = 0 : thanh toán thành công (Request.QueryString["errorCode"])
        //Tham khảo bảng mã lỗi tại: https://developers.momo.vn/#/docs/aio/?id=b%e1%ba%a3ng-m%c3%a3-l%e1%bb%97i
        public ActionResult ConfirmPaymentClient(Result result)
        {
            //lấy kết quả Momo trả về và hiển thị thông báo cho người dùng (có thể lấy dữ liệu ở đây cập nhật xuống db)
            //if (int.Parse(result.errorCode) == 0)
            //{
            //    int id = (int)Session["ordertempID"];

            //    var order = _dbContext.Orders.Find(id);
            //    order.Status = true;
            //    _dbContext.SaveChanges();
            //}
            string rMessage = result.message;
            string rOrderId = result.orderId;
            string rErrorCode = result.errorCode; // = 0: thanh toán thành công
            return View();
        }

        [HttpPost]
        public void SavePayment()
        {
            //cập nhật dữ liệu vào db
            String a = "";
        }

    }


}